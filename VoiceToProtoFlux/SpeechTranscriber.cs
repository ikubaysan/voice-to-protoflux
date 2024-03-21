using System;
using System.Collections.Generic;
using System.Speech.Recognition;
using System.Text;
using System.Windows.Forms;

namespace VoiceToProtoFlux
{
    public class SpeechTranscriber
    {
        private readonly SpeechRecognitionEngine recognizer;
        private readonly List<TranscriptionCollection> transcriptionHistory = new List<TranscriptionCollection>();
        private readonly ListBox transcriptionListBox;
        private readonly CheckBox transcriptionEnabledCheckBox;
        private readonly int MaxAlternatesCount = 5; // Max number of alternates to consider
        private readonly List<ProtoFluxTypeInfo> protoFluxTypes;
        private readonly WebSocketServer webSocketServer;
        private readonly ProtoFluxTypeInfoCollection protoFluxTypeCollection;

        public SpeechTranscriber(ListBox listBox, CheckBox checkBox, ProtoFluxTypeInfoCollection protoFluxTypeCollection, WebSocketServer webSocketServer)
        {
            this.transcriptionListBox = listBox;
            this.transcriptionEnabledCheckBox = checkBox;
            this.protoFluxTypeCollection = protoFluxTypeCollection;
            this.webSocketServer = webSocketServer;

            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            recognizer.LoadGrammar(ConstructCustomGrammar());

            recognizer.MaxAlternates = MaxAlternatesCount;

            recognizer.EndSilenceTimeout = TimeSpan.FromSeconds(2);
            recognizer.EndSilenceTimeoutAmbiguous = TimeSpan.FromSeconds(2);

            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            recognizer.SetInputToDefaultAudioDevice();
        }

        private Grammar ConstructCustomGrammar()
        {
            // Initialize a Choices object to accumulate all phrases from each ProtoFluxTypeInfo
            var choices = new Choices();

            // Iterate through each ProtoFluxTypeInfo to add its phrases to the Choices
            foreach (var typeInfo in protoFluxTypeCollection.typeInfos)
            {
                foreach (var phrase in typeInfo.Phrases)
                {
                    // Adding each phrase as an individual choice
                    choices.Add(phrase);
                }
            }

            // Construct the GrammarBuilder with the accumulated Choices
            var grammarBuilder = new GrammarBuilder();
            grammarBuilder.Append(choices);

            // Create the Grammar from the GrammarBuilder
            return new Grammar(grammarBuilder);
        }

        public void StartRecognition()
        {
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void StopRecognition()
        {
            recognizer.RecognizeAsyncStop();
        }

        private void Recognizer_SpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
        {
            if (!transcriptionEnabledCheckBox.Checked) return;

            // Create a new TranscriptionsCollection for the current recognition event
            var transcriptionCollection = new TranscriptionCollection();

            foreach (var alternate in e.Result.Alternates)
            {
                // Add each transcription alternative to the TranscriptionsCollection
                transcriptionCollection.AddTranscription(alternate.Text, alternate.Confidence);
            }

            // Add the TranscriptionsCollection to the history
            DisplayTranscriptionCollection(transcriptionCollection);
        }

        private async void DisplayTranscriptionCollection(TranscriptionCollection transcriptionCollection)
        {
            // Adjusted to handle TranscriptionsCollection
            if (transcriptionListBox.InvokeRequired)
            {
                transcriptionListBox.Invoke(new Action<TranscriptionCollection>(DisplayTranscriptionCollection), new object[] { transcriptionCollection });
            }
            else
            {
                transcriptionHistory.Add(transcriptionCollection);
                // Limit the history size for display
                if (transcriptionHistory.Count > 10)
                {
                    transcriptionHistory.RemoveAt(0);
                }

                transcriptionListBox.Items.Clear();
                foreach (var item in transcriptionHistory)
                {
                    transcriptionListBox.Items.Add(item.ToString());
                }
            }


            StringBuilder messageBuilder = new StringBuilder();
            foreach (var transcription in transcriptionCollection.transcriptions)
            {
                messageBuilder.Append($"{transcription.Text};{transcription.Confidence}|");
            }

            string message = messageBuilder.ToString().TrimEnd('|');
            await webSocketServer.BroadcastMessageAsync(message);


        }
    }


}
