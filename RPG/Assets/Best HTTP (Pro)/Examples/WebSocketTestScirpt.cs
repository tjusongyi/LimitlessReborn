using BestHTTP.WebSocket;
using System;
using UnityEngine;

public class WebSocketTestScirpt : MonoBehaviour
{
    string address = "ws://echo.websocket.org";
    string msgToSend = "Hello World!";
    string Text = string.Empty;

    WebSocket webSocket;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.LoadLevel(0);
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUI.TextArea(new Rect(5, 10, Screen.width - 10, Screen.height - 180), Text);
        GUILayout.EndVertical();

        address = GUI.TextField(new Rect(5, Screen.height - 160, Screen.width - 10, 30), address);

        if (webSocket != null && webSocket.IsOpen)
            msgToSend = GUI.TextField(new Rect(5, Screen.height - 120, Screen.width - 10, 30), msgToSend);

        if (webSocket == null && GUI.Button(new Rect(5, Screen.height - 65, 120, 60), "Open Web Socket"))
        {
            webSocket = new WebSocket(new Uri(address));
            webSocket.OnOpen += OnOpen;
            webSocket.OnMessage += OnMessageReceived;
            webSocket.OnClosed += OnClosed;
            webSocket.OnError += OnError;

            webSocket.Open();

            Text += "Opening Web Socket...\n";
        }

        if (webSocket != null && webSocket.IsOpen && GUI.Button(new Rect(130, Screen.height - 65, 110, 60), "Send"))
        {
            Text += "Sending message...\n";
            webSocket.Send(msgToSend);
        }

        if (webSocket != null && webSocket.IsOpen && GUI.Button(new Rect(250, Screen.height - 65, 110, 60), "Close"))
            webSocket.Close(1000, "Bye!");
            
        if (GUI.Button(new Rect(Screen.width - 115, Screen.height - 65, 110, 60), "Back"))
            Application.LoadLevel(0);
    }

    // Called when the web socket is open, and we are ready to send and receive data
    void OnOpen(WebSocket ws)
    {
        Text += string.Format("-WebSocket Open!\n");
    }

    // Called when we received a text message from the server
    void OnMessageReceived(WebSocket ws, string message)
    {
        Text += string.Format("-Message received: {0}\n", message);
    }

    // Called when the web socket closed
    void OnClosed(WebSocket ws, UInt16 code, string message)
    {
        Text += string.Format("-WebSocket closed! Code: {0} Message: {1}\n", code, message);
        webSocket = null;
    }

    // Called when an error occured on client side
    void OnError(WebSocket ws, Exception ex)
    {
        string errorMsg = string.Empty;
        if (ws.InternalRequest.Response != null)
            errorMsg = string.Format("Status Code from Server: {0} and Message: {1}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message);

        Text += string.Format("-An error occured: {0}\n", (ex != null ? ex.Message : "Unknown Error " + errorMsg));

        webSocket = null;
    }
}