using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace VoiceToProtoFlux.Objects.WebsocketServerObjects
{
    public class WebSocketServer
    {
        private HttpListener httpListener;
        private List<WebSocket> clients = new List<WebSocket>();
        private bool isListening = false;
        public delegate void MessageReceivedHandler(string message);
        public event MessageReceivedHandler? OnMessageReceived;

        public enum CommandName
        {
            DISABLE_LISTENING,
            ENABLE_LISTENING
        }

        private string GetCommandString(CommandName commandName)
        {
            switch (commandName)
            {
                case CommandName.DISABLE_LISTENING:
                    return "DISABLE_LISTENING";
                case CommandName.ENABLE_LISTENING:
                    return "ENABLE_LISTENING";
                default:
                    throw new ArgumentOutOfRangeException(nameof(commandName), commandName, null);
            }
        }

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
                    // Replace the above line with the following block
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(receiveBuffer, 0, result.Count);
                        System.Diagnostics.Debug.WriteLine($"Message received: {message}");
                        OnMessageReceived?.Invoke(message);
                    }
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

        public async Task SendCommandToClient(CommandName commandName)
        {
            string commandToClient = $"Type1_{GetCommandString(commandName)}";
            await BroadcastMessageAsync(commandToClient);
            System.Diagnostics.Debug.WriteLine($"Sent command to client: {commandToClient}");
        }

        public void Stop()
        {
            isListening = false;
            httpListener.Stop();
        }
    }
}
