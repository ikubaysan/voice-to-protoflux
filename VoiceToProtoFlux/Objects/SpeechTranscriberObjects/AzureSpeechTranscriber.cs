using Azure.AI.TextAnalytics;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VoiceToProtoFlux.Objects.ProtoFluxTypeObjects;

namespace VoiceToProtoFlux.Objects.SpeechTranscriberObjects
{
    public class AzureSpeechTranscriber
    {
        private SpeechRecognizer recognizer;
        private ConfigManager configManager;

        public AzureSpeechTranscriber(ConfigManager configManager, List<string> grammarPhrases)
        {
            this.configManager = configManager;
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

        private void OnRecognized(object sender, SpeechRecognitionEventArgs e)
        {
            if (e.Result.Reason == ResultReason.RecognizedSpeech)
            {
                Debug.WriteLine($"Recognized: {e.Result.Text}");
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
        }

        public async Task StopRecognitionAsync()
        {
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
        }
    }
}
