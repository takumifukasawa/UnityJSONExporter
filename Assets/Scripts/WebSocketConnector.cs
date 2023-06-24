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

        _webSocket.OnOpen += () => { Debug.Log("Connection open!"); };

        _webSocket.OnError += (e) => { Debug.Log("Error! " + e); };

        _webSocket.OnClose += (e) => { Debug.Log("Connection closed!"); };

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
#if !UNITY_WEBGL || UNITY_EDITOR
        _webSocket.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        await _webSocket.Close();
    }

    async void SendWebSocketMessage()
    {
        if (_webSocket.State == WebSocketState.Open)
        {
            float t = 1.2f;
            byte[] floats = BitConverter.GetBytes(t);
            await _webSocket.Send(floats);
            await _webSocket.SendText($"plain text message - t: {t.ToString()}");
        }
    }
}
