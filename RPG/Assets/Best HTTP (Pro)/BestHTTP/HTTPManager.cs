using System;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.Caching;

namespace BestHTTP
{
    /// <summary>
    /// 
    /// </summary>
    public static class HTTPManager
    {
        // Static constructor. Setup default values
        static HTTPManager()
        {
            MaxConnectionPerServer = 4;
            KeepAliveDefaultValue = true;
            MaxPathLength = 255;
            MaxConnectionIdleTime = TimeSpan.FromMinutes(2);
            IsCookiesEnabled = true;
            CookieJarSize = 10 * 1024 * 1024;
            EnablePrivateBrowsing = false;
            ConnectTimeout = TimeSpan.FromSeconds(20);
            RequestTimeout = TimeSpan.FromSeconds(60);
        }

        #region Global Options

        private static byte maxConnectionPerServer;
        /// <summary>
        /// The maximum active tcp connections that the client will maintain to a server. Default value is 4. Minimum value is 1.
        /// </summary>
        public static byte MaxConnectionPerServer
        {
            get{ return maxConnectionPerServer; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("MaxConnectionPerServer must be greater than 0!");
                maxConnectionPerServer = value;
            }
        }

        /// <summary>
        /// Default value of a http request's IsKeepAlive value. Default value is true. If you make rare request to the server it's should be changed to false.
        /// </summary>
        public static bool KeepAliveDefaultValue { get; set; }

        /// <summary>
        /// Set to true, if caching is prohibited.
        /// </summary>
        public static bool IsCachingDisabled { get; set; }

        /// <summary>
        /// How many time must be passed to destroy that connection after a connection finished it's last request. It's default value is two minutes.
        /// </summary>
        public static TimeSpan MaxConnectionIdleTime { get; set; }

        /// <summary>
        /// Set to false to disable all Cookie. It's default value is true.
        /// </summary>
        public static bool IsCookiesEnabled { get; set; }

        /// <summary>
        /// Size of the Cookie Jar in bytes. It's default value is 10485760 (10 MB).
        /// </summary>
        public static uint CookieJarSize { get; set; }

        /// <summary>
        /// If this property is set to true, then new cookies treated as session cookies and these cookies are not saved to disk. It's default value is false;
        /// </summary>
        public static bool EnablePrivateBrowsing { get; set; }

        /// <summary>
        /// Global, default value of the HTTPRequest's ConnectTimeout property. Default value is 20 seconds.
        /// </summary>
        public static TimeSpan ConnectTimeout { get; set; }

        /// <summary>
        /// Global, default value of the HTTPRequest's Timeout property. Default value is 60 seconds.
        /// </summary>
        public static TimeSpan RequestTimeout { get; set; }

        /// <summary>
        /// By default the plugin will save all cache and cookie data under the path returned by Application.persistentDataPath.
        /// You can assign a function to this delegate to return a custom root path to define a new path.
        /// <remarks>This delegate will be called on a non Unity thread!</remarks>
        /// </summary>
        public static System.Func<string> RootCacheFolderProvider { get; set; }

        /// <summary>
        /// On most systems the maximum length of a path is around 255 character. If a cache entity's path is longer than this value it doesn't get cached. There no patform independent API to query the exact value on the current system, but it's 
        /// exposed here and can be overridden. It's default value is 255.
        /// </summary>
        internal static int MaxPathLength { get; set; }

        #endregion

        #region Manager variables

        /// <summary>
        /// All connection has a reference in this Dictionary untill it's removed completly.
        /// </summary>
        private static Dictionary<string, List<HTTPConnection>> Connections = new Dictionary<string, List<HTTPConnection>>();

        /// <summary>
        /// Active connections. These connections all has a request to process.
        /// </summary>
        private static List<HTTPConnection> ActiveConnections = new List<HTTPConnection>();

        /// <summary>
        /// Free connections. They can be removed completly after a specified time.
        /// </summary>
        private static List<HTTPConnection> FreeConnections = new List<HTTPConnection>();

        /// <summary>
        /// Connections that recycled in the Update loop. If they are not used in the same loop to process a request, they will be transferred to the FreeConnections list.
        /// </summary>
        private static List<HTTPConnection> RecycledConnections = new List<HTTPConnection>();

        /// <summary>
        /// List of request that have to wait until there is a free connection to the server.
        /// </summary>
        private static List<HTTPRequest> RequestQueue = new List<HTTPRequest>();
        private static bool IsCallingCallbacks;

        internal static System.Object Locker = new System.Object();

        #endregion

        #region Public Interface

        public static void Setup()
        {
            HTTPUpdateDelegator.CheckInstance();
            HTTPCacheService.CheckSetup();
            Cookies.CookieJar.SetupFolder();
        }

        public static HTTPRequest SendRequest(string url, Action<HTTPRequest, HTTPResponse> callback)
        {
            return SendRequest(new HTTPRequest(new Uri(url), HTTPMethods.Get, callback));
        }

        public static HTTPRequest SendRequest(string url, HTTPMethods methodType, Action<HTTPRequest, HTTPResponse> callback)
        {
            return SendRequest(new HTTPRequest(new Uri(url), methodType, callback));
        }

        public static HTTPRequest SendRequest(string url, HTTPMethods methodType, bool isKeepAlive, Action<HTTPRequest, HTTPResponse> callback)
        {
            return SendRequest(new HTTPRequest(new Uri(url), methodType, isKeepAlive, callback));
        }

        public static HTTPRequest SendRequest(string url, HTTPMethods methodType, bool isKeepAlive, bool disableCache, Action<HTTPRequest, HTTPResponse> callback)
        {
            return SendRequest(new HTTPRequest(new Uri(url), methodType, isKeepAlive, disableCache, callback));
        }

        public static HTTPRequest SendRequest(HTTPRequest request)
        {
            lock (Locker)
            {
                Setup();

                // TODO: itt meg csak adja hozza egy sorhoz, es majd a LateUpdate-ben hivodjon a SendRequestImpl.
                //  Igy ha egy callback-ben kuldenenk ugyanarra a szerverre request-et, akkor feltudjuk hasznalni az elozo connection-t.
                if (IsCallingCallbacks)
                {
                    request.State = HTTPRequestStates.Queued;
                    RequestQueue.Add(request);
                }
                else
                    SendRequestImpl(request);

                return request;
            }
        }

        #endregion

        #region Private Functions

        private static void SendRequestImpl(HTTPRequest request)
        {
            HTTPConnection conn = FindOrCreateFreeConnection(request);

            if (conn != null)
            {
                // found a free connection: put it in the ActiveConnection list(they will be checked periodically in the OnUpdate call)
                if (ActiveConnections.Find((c) => c == conn) == null)
                    ActiveConnections.Add(conn);

                FreeConnections.Remove(conn);

                request.State = HTTPRequestStates.Processing;

                request.Prepare();

                // then start process the request
                conn.Process(request);
            }
            else
            {
                // If no free connection found and creation prohibited, we will put back to the queue
                request.State = HTTPRequestStates.Queued;
                RequestQueue.Add(request);
            }
        }

        private static string GetKeyForRequest(HTTPRequest request)
        {
            // proxyUri + requestUri
            // HTTP and HTTPS needs different connections.
            return (request.Proxy != null ? new UriBuilder(request.Proxy.Address.Scheme, request.Proxy.Address.Host, request.Proxy.Address.Port).Uri.ToString() : string.Empty) +
                                            new UriBuilder(request.CurrentUri.Scheme, request.CurrentUri.Host, request.CurrentUri.Port).Uri.ToString();
        }

        private static HTTPConnection FindOrCreateFreeConnection(HTTPRequest request)
        {
            HTTPConnection conn = null;
            List<HTTPConnection> connections;

            string serverUrl = GetKeyForRequest(request);

            if (Connections.TryGetValue(serverUrl, out connections))
            {
                // search for a Free connection
                for (int i = 0; i < connections.Count && conn == null; ++i)
                {
                    var tmpConn = connections[i];

                    if (tmpConn != null && 
                        tmpConn.IsFree &&
                        (!tmpConn.HasProxy || 
                         tmpConn.LastProcessedUri == null || 
                         tmpConn.LastProcessedUri.Host.Equals(request.CurrentUri.Host, StringComparison.OrdinalIgnoreCase)))
                        conn = tmpConn;
                }
            }
            else
                Connections.Add(serverUrl, connections = new List<HTTPConnection>(MaxConnectionPerServer));

            // No free connection found?
            if (conn == null)
            {
                // Max connection reached?
                if (connections.Count == MaxConnectionPerServer)
                    return null;

                // if no, create a new one
                connections.Add(conn = new HTTPConnection(serverUrl));
            }

            return conn;
        }

        private static void RecycleConnection(HTTPConnection conn)
        {
            conn.Recycle();

            RecycledConnections.Add(conn);
        }

        #endregion

        #region Internal Helper Functions

        /// <summary>
        /// Will return the HTTPConnection object that processing the given request.
        /// </summary>
        internal static HTTPConnection GetConnectionWith(HTTPRequest request)
        {
            lock (Locker)
            {
                for (int i = 0; i < ActiveConnections.Count; ++i)
                {
                    var connection = ActiveConnections[i];
                    if (connection.CurrentRequest == request)
                        return connection;
                }

                return null;
            }
        }

        /// <summary>
        /// Will return where the various caches should be saved.
        /// </summary>
        internal static string GetRootCacheFolder()
        {
            try
            {
                if (RootCacheFolderProvider != null)
                    return RootCacheFolderProvider();
            }
            catch(Exception ex)
            {
                Debug.LogError(ex.Message + " " + ex.StackTrace);
            }

#if NETFX_CORE
            return Windows.Storage.ApplicationData.Current.LocalFolder.Path;
#else
            return Application.persistentDataPath;
#endif
        }

        #endregion

        #region MonoBehaviour Events (Called from HTTPUpdateDelegator)

        /// <summary>
        /// Update function that should be called regularly from a Unity event(Update, LateUpdate). Callbacks are dispatched from this function.
        /// </summary>
        public static void OnUpdate()
        {
            lock (Locker)
            {
                IsCallingCallbacks = true;
                try
                {
                    for (int i = 0; i < ActiveConnections.Count; ++i)
                    {
                        HTTPConnection conn = ActiveConnections[i];

                        switch (conn.State)
                        {
                            case HTTPConnectionStates.Processing:
                                conn.HandleProgressCallback();

                                if (conn.CurrentRequest.UseStreaming && conn.CurrentRequest.Response != null && conn.CurrentRequest.Response.HasStreamedFragments())
                                    conn.HandleCallback();

                                if ((!conn.CurrentRequest.UseStreaming || conn.CurrentRequest.EnableTimoutForStreaming) &&
                                    DateTime.UtcNow - conn.StartTime > conn.CurrentRequest.Timeout)
                                    conn.Abort(HTTPConnectionStates.TimedOut);

                                break;

                            case HTTPConnectionStates.Redirected:
                                // If the server redirected us, we need to find or create a connection to the new server and send out the request again.
                                SendRequest(conn.CurrentRequest);

                                RecycleConnection(conn);
                                break;

                            case HTTPConnectionStates.WaitForRecycle:
                                // If it's a streamed request, it's finished now
                                conn.CurrentRequest.FinishStreaming();

                                // Call the callback
                                conn.HandleCallback();

                                // Then recycle the connection
                                RecycleConnection(conn);
                                break;

                            case HTTPConnectionStates.Upgraded:
                                // The connection upgraded to an other protocol
                                conn.HandleCallback();
                                break;

                            case HTTPConnectionStates.WaitForProtocolShutdown:
                                var ws = conn.CurrentRequest.Response as WebSocket.WebSocketResponse;
                                ws.HandleEvents();
                                if (ws.IsClosed)
                                {
                                    conn.HandleCallback();

                                    // After both sending and receiving a Close message, an endpoint considers the WebSocket connection closed and MUST close the underlying TCP connection.
                                    conn.Dispose();
                                    RecycleConnection(conn);
                                }
                                break;

                            case HTTPConnectionStates.Closed:
                                // If it's a streamed request, it's finished now
                                conn.CurrentRequest.FinishStreaming();

                                // Call the callback
                                conn.HandleCallback();

                                // It will remove from the ActiveConnections
                                RecycleConnection(conn);
                                break;
                        }
                    }
                }
                finally
                {
                    IsCallingCallbacks = false;
                }

                if (RecycledConnections.Count > 0)
                {
                    for (int i = 0; i < RecycledConnections.Count; ++i)
                    {
                        var connection = RecycledConnections[i];
                        // If in a callback made a request that aquired this connection, then we will not remove it from the
                        //  active connections.
                        if (connection.IsFree)
                        {
                            ActiveConnections.Remove(connection);
                            FreeConnections.Add(connection);
                        }
                    }
                    RecycledConnections.Clear();
                }

                if (FreeConnections.Count > 0)
                    for (int i = 0; i < FreeConnections.Count; i++)
                    {
                        var connection = FreeConnections[i];

                        if (connection.IsRemovable)
                        {
                            // Remove the connection from the connection reference table
                            List<HTTPConnection> connections = null;
                            if (Connections.TryGetValue(connection.ServerAddress, out connections))
                                connections.Remove(connection);

                            // Dispose the connection
                            connection.Dispose();

                            FreeConnections.RemoveAt(i);
                            i--;
                        }
                    }

                // We create an array from the queue and clear it. When we call the SendRequest while still no room for new connections, the same queue will be rebuilt.
                if (RequestQueue.Count > 0)
                {
                    var queue = RequestQueue.ToArray();
                    RequestQueue.Clear();

                    for (int i = 0; i < queue.Length; ++i)
                        SendRequest(queue[i]);
                }
            }
        }

        internal static void OnQuit()
        {
            lock (Locker)
            {
                Caching.HTTPCacheService.SaveLibrary();

                // Close all tcp connections when the application is terminating.
                foreach (var kvp in Connections)
                {
                    foreach (var conn in kvp.Value)
                        conn.Dispose();
                    kvp.Value.Clear();
                }
                Connections.Clear();
            }
        }

        #endregion
    }
}