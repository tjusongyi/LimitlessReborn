using UnityEngine;
using System.Collections;
using BestHTTP;
using System;

public class HttpManager : Singleton<HttpManager>
{

    public static string baseUrl = "http://127.0.0.1:8000/";

    //HttpManager()
    //{

    //}

    public void SendRequest(Uri uri, Action<HTTPRequest, HTTPResponse> callback)
    {
        HTTPRequest request = new HTTPRequest(uri, callback);
        
    }
}
