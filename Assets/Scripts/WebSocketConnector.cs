using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class WebSocketConnector : MonoBehaviour
{
    [SerializeField]
    private int _port;
    
    private WebSocket _webSocket;
    
    private async void Start()
    {
        _webSocket = new WebSocket($"ws://localhost:{_port}");
        
        _webSocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };
        
        _webSocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };
        
        _webSocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };
        
        _webSocket.OnMessage += (bytes) =>
        {
            Debug.Log("OnMessage!");
            // getting the message as a string
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("OnMessage! " + message);
        };
        
        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);
        
        await _webSocket.Connect();
    }

    private void Update()
    {
        _webSocket.DispatchMessageQueue();
    }

    private async void OnApplicationQuit()
    {
        await _webSocket.Close();
    }
    
    async void SendWebSocketMessage() {
        if (_webSocket.State == WebSocketState.Open)
        {
            await _webSocket.Send(new byte[] { 10, 20, 30 });
            await _webSocket.SendText("plain text message");
        }
    }
}
