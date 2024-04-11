using System;
using System.Text;
using System.Threading.Tasks;
using VoiceToProtoFlux.Objects.ProtoFluxTypeObjects;
using VoiceToProtoFlux.Objects.SpeechTranscriberObjects;
using VoiceToProtoFlux.Objects.WebsocketServerObjects;

namespace VoiceToProtoFlux.Objects.WebsocketServerObjects
{
    public class WebSocketServerReceivedMessageHandler
    {
        private WebSocketServer webSocketServer;
        private AzureSpeechTranscriber azureSpeechTranscriber;
        private ProtoFluxTypeInfoCollection protoFluxTypeInfoCollection;

        public WebSocketServerReceivedMessageHandler(WebSocketServer webSocketServer, AzureSpeechTranscriber azureSpeechTranscriber, ProtoFluxTypeInfoCollection protoFluxTypeInfoCollection)
        {
            this.webSocketServer = webSocketServer;
            this.azureSpeechTranscriber = azureSpeechTranscriber;
            this.protoFluxTypeInfoCollection = protoFluxTypeInfoCollection;
        }

        public async Task OnMessageReceivedAsync(string message)
        {
            if (message.StartsWith("Search:"))
            {
                await HandleSearchCommandAsync(message.Substring("Search:".Length).Trim());
            }
            else if (message == "EnableListening")
            {
                await HandleEnableListeningCommandAsync();
            }
            else if (message == "DisableListening")
            {
                await HandleDisableListeningCommandAsync();
            }
            // Add more commands as needed
        }

        private async Task HandleSearchCommandAsync(string searchQuery)
        {
            // Convert the search query to lower case for case-insensitive comparison
            var keywords = searchQuery.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Filter ProtoFluxTypeInfoCollection based on the keywords
            var matchedTypeInfos = protoFluxTypeInfoCollection.typeInfos
                .Where(typeInfo => keywords.All(keyword => typeInfo.NiceName.ToLower().Contains(keyword)))
                .ToList();

            // Sort the results alphabetically by NicePath for consistency
            matchedTypeInfos.Sort((a, b) => string.Compare(a.NicePath, b.NicePath, StringComparison.OrdinalIgnoreCase));

            // Build the response message with the matched type information
            var responseMessage = new StringBuilder("Type2_");

            // Append each matched type info's NicePath to the response message, separated by new lines
            foreach (var typeInfo in matchedTypeInfos)
            {
                responseMessage.AppendLine(typeInfo.NicePath);
            }

            // Trim the final new line to ensure the message format is correct
            var trimmedResponse = responseMessage.ToString().TrimEnd();

            // Send the response message back to the client via WebSocket
            await webSocketServer.BroadcastMessageAsync(trimmedResponse);
        }

        private async Task HandleEnableListeningCommandAsync()
        {
            System.Diagnostics.Debug.WriteLine("EnableListening command received via WebSocket");
            await azureSpeechTranscriber.StartRecognitionAsync();
        }

        private async Task HandleDisableListeningCommandAsync()
        {
            System.Diagnostics.Debug.WriteLine("DisableListening command received via WebSocket");
            await azureSpeechTranscriber.StopRecognitionAsync();
        }

        // You can add more methods to handle different types of messages
    }
}
