using Azure.AI.TextAnalytics;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VoiceToProtoFlux.Objects.ProtoFluxParameterObjects;
using VoiceToProtoFlux.Objects.ProtoFluxTypeObjects;
using VoiceToProtoFlux.Objects.TranscriptionObjects;
using VoiceToProtoFlux.Objects.WebsocketServerObjects;

namespace VoiceToProtoFlux.Objects.SpeechTranscriberObjects
{
    public class AzureSpeechTranscriber
    {
        private SpeechRecognizer recognizer;
        private ConfigManager configManager;
        private ProtoFluxTypeInfoCollection protoFluxTypeInfoCollection;
        private WebSocketServer webSocketServer;

        public event Action<bool> RecognitionEnabledChanged;

        private bool _recognitionEnabled = false;
        public bool recognitionEnabled
        {
            get => _recognitionEnabled;
            private set
            {
                if (_recognitionEnabled != value)
                {
                    _recognitionEnabled = value;
                    // Trigger the event when the value changes.
                    RecognitionEnabledChanged?.Invoke(_recognitionEnabled);
                }
            }
        }




        public AzureSpeechTranscriber(ConfigManager configManager, WebSocketServer webSocketServer, ProtoFluxTypeInfoCollection protoFluxTypeInfoCollection, List<string> grammarPhrases)
        {
            this.configManager = configManager;
            this.webSocketServer = webSocketServer;
            this.protoFluxTypeInfoCollection = protoFluxTypeInfoCollection;
            InitializeSpeechRecognizer(grammarPhrases);
        }

        private void InitializeSpeechRecognizer(List<string> grammarPhrases)
        {
            var speechConfig = SpeechConfig.FromSubscription(configManager.ApiKey, configManager.Region);
            recognizer = new SpeechRecognizer(speechConfig);

            // Initialize phrase list grammar from the recognizer
            var phraseList = PhraseListGrammar.FromRecognizer(recognizer);

            // Populate the phrase list with unique words from typeInfoCollection
            foreach (var phrase in grammarPhrases)
            {
                phraseList.AddPhrase(phrase);
            }

            // Attach event handlers using named methods
            recognizer.Recognized += OnRecognized;
            recognizer.Canceled += OnCanceled;
            recognizer.SessionStopped += OnSessionStopped;
            recognizer.SessionStarted += OnSessionStarted;
        }

        private async void OnRecognized(object sender, SpeechRecognitionEventArgs e)
        {
            if (e.Result.Reason == ResultReason.RecognizedSpeech)
            {
                Debug.WriteLine($"Recognized text: {e.Result.Text}");

                // Split the recognized text into individual words
                List<string> recognizedWords = e.Result.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                Debug.WriteLine($"Recognized words: {string.Join(", ", recognizedWords)}");

                // Initialize typeWords and parameterWords as empty lists
                List<string> typeWords = new List<string>();
                List<string> parameterWords = new List<string>();

                // Attempt to locate the last occurrence of "of" followed by "type"
                int lastOfTypeIndex = recognizedWords.FindLastIndex(word => word.Equals("of", StringComparison.OrdinalIgnoreCase));
                if (lastOfTypeIndex != -1 && lastOfTypeIndex + 1 < recognizedWords.Count && recognizedWords[lastOfTypeIndex + 1].Equals("type", StringComparison.OrdinalIgnoreCase))
                {
                    // Take all words up until the last "of type"
                    typeWords = recognizedWords.Take(lastOfTypeIndex).ToList();

                    // Take all words after the last "of type"
                    if (lastOfTypeIndex + 2 < recognizedWords.Count) // Check if there are words after the last "of type"
                    {
                        parameterWords = recognizedWords.Skip(lastOfTypeIndex + 2).ToList(); // Skip the last "of" and "type", take the rest
                    }
                }
                else
                {
                    // If "of type" is not found, consider all recognized words as type words
                    typeWords = new List<string>(recognizedWords);
                }

                // Debug output for verification
                Debug.WriteLine($"Type words: {string.Join(", ", typeWords)}");
                Debug.WriteLine($"Parameter words: {string.Join(", ", parameterWords)}");

                List<ProtoFluxTypeInfo> bestTypeMatches = protoFluxTypeInfoCollection.FindBestMatchingTypeInfoByWords(typeWords);
                if (bestTypeMatches.Count == 0)
                {
                    Debug.WriteLine("No type matches found.");
                    return;
                }
                ProtoFluxTypeInfo bestTypeMatch = bestTypeMatches[0];

                Debug.WriteLine($"Best type match: {bestTypeMatch.FullName}");

                List<ProtoFluxParameter> protoFluxParameters = new List<ProtoFluxParameter>();

                if (bestTypeMatch.ParameterCount > 0)
                {
                    ProtoFluxParameter? bestParameterMatch = ProtoFluxParameterCollection.Instance.FindBestMatchingParameterByWords(parameterWords);
                    if (bestParameterMatch != null)
                    {
                        protoFluxParameters.Add(bestParameterMatch);
                        Debug.WriteLine($"Best parameter match: {bestParameterMatch.Name}");
                    }
                    else
                    {
                        Debug.WriteLine($"No parameter matches found.");
                    }
                }

                Transcription transcription = new Transcription(protoFluxTypeInfo: bestTypeMatch, providedParameters: protoFluxParameters);

                await webSocketServer.BroadcastMessageAsync(transcription.ToWebsocketString());

            }
            else
            { 
                
            }
        }

        private void OnCanceled(object sender, SpeechRecognitionCanceledEventArgs e)
        {
            Debug.WriteLine($"Recognition canceled. Reason: {e.Reason}; ErrorDetails: {e.ErrorDetails}");
        }
        private void OnSessionStarted(object sender, SessionEventArgs e)
        {
            Debug.WriteLine("Session started.");
        }

        private void OnSessionStopped(object sender, SessionEventArgs e)
        {
            Debug.WriteLine("Session stopped.");
        }

        public async Task StartRecognitionAsync()
        {
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            await webSocketServer.SendCommandToClient(WebSocketServer.CommandName.ENABLE_LISTENING);
            recognitionEnabled = true;
        }

        public async Task StopRecognitionAsync()
        {
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            await webSocketServer.SendCommandToClient(WebSocketServer.CommandName.DISABLE_LISTENING);
            recognitionEnabled = false;
        }
    }
}
