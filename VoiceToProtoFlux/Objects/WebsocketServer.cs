using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace VoiceToProtoFlux.Objects
{
    public class WebSocketServer
    {
        private HttpListener httpListener;
        private List<WebSocket> clients = new List<WebSocket>();
        private bool isListening = false;

        public WebSocketServer(string uriPrefix)
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(uriPrefix);
        }

        public async Task StartAsync()
        {
            isListening = true;
            httpListener.Start();
            System.Diagnostics.Debug.WriteLine($"WebSocketServer started at {httpListener.Prefixes.FirstOrDefault()}");

            while (isListening)
            {
                HttpListenerContext listenerContext = await httpListener.GetContextAsync();
                if (listenerContext.Request.IsWebSocketRequest)
                {
                    HandleWebSocketRequestAsync(listenerContext);
                }
                else
                {
                    listenerContext.Response.StatusCode = 400;
                    listenerContext.Response.Close();
                }
            }
        }

        private async void HandleWebSocketRequestAsync(HttpListenerContext listenerContext)
        {
            WebSocketContext webSocketContext = await listenerContext.AcceptWebSocketAsync(null);
            WebSocket webSocket = webSocketContext.WebSocket;
            clients.Add(webSocket);

            try
            {
                byte[] receiveBuffer = new byte[1024];
                while (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    // Echo test, optional
                    // await webSocket.SendAsync(new ArraySegment<byte>(receiveBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            finally
            {
                if (webSocket != null)
                    clients.Remove(webSocket);
            }
        }

        public async Task BroadcastMessageAsync(string message)
        {
            foreach (var client in clients)
            {
                if (client.State == WebSocketState.Open)
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    await client.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        public void Stop()
        {
            isListening = false;
            httpListener.Stop();
        }
    }
}
