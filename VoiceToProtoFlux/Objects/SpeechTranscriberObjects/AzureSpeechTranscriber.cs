using Azure.AI.TextAnalytics;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VoiceToProtoFlux.Objects.SpeechTranscriberObjects
{
    public class AzureSpeechTranscriber
    {
        private SpeechRecognizer recognizer;
        private ConfigManager configManager;

        public AzureSpeechTranscriber(ConfigManager configManager)
        {
            this.configManager = configManager;
            InitializeSpeechRecognizer();
        }

        private void InitializeSpeechRecognizer()
        {
            var speechConfig = SpeechConfig.FromSubscription(configManager.ApiKey, configManager.Region);
            recognizer = new SpeechRecognizer(speechConfig);

            recognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    Debug.WriteLine($"Recognized: {e.Result.Text}");
                }
            };

            recognizer.Canceled += (s, e) =>
            {
                Debug.WriteLine($"Recognition canceled. Reason: {e.Reason}; ErrorDetails: {e.ErrorDetails}");
            };

            recognizer.SessionStopped += (s, e) =>
            {
                Debug.WriteLine("Session stopped.");
            };
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
