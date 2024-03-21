using System;
using System.Collections.Generic;
using System.Speech.Recognition;
using System.Windows.Forms;

namespace VoiceToProtoFlux
{


    public class Transcription
    {
        public string Text { get; set; }
        public float Confidence { get; set; }

        public Transcription(string text, float confidence)
        {
            Text = text;
            Confidence = confidence;
        }

        public override string ToString()
        {
            return $"{Text} (Confidence: {Confidence:N2})";
        }
    }


    public class TranscriptionCollection
    {
        private List<Transcription> transcriptions = new List<Transcription>();
        public int MaxAlternatesCount { get; set; } = 5; // Default to 5, can be adjusted

        public void AddTranscription(string text, float confidence)
        {
            if (transcriptions.Count >= MaxAlternatesCount)
            {
                transcriptions.RemoveAt(0); // Ensure we do not exceed MaxAlternatesCount
            }
            transcriptions.Add(new Transcription(text, confidence));
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            foreach (var transcription in transcriptions)
            {
                sb.AppendLine(transcription.ToString());
            }
            return sb.ToString().TrimEnd(); // Remove the last newline for a clean output
        }
    }

    public class SpeechTranscriber
    {
        private readonly SpeechRecognitionEngine recognizer;
        private readonly List<TranscriptionCollection> transcriptionHistory = new List<TranscriptionCollection>();
        private readonly ListBox transcriptionListBox;
        private readonly CheckBox transcriptionEnabledCheckBox;
        private readonly int MaxAlternatesCount = 5; // Specify the number of alternates to consider
        private readonly List<ProtoFluxTypeInfo> protoFluxTypes;

        public SpeechTranscriber(ListBox listBox, CheckBox checkBox, List<ProtoFluxTypeInfo> protoFluxTypes)
        {
            this.transcriptionListBox = listBox;
            this.transcriptionEnabledCheckBox = checkBox;
            this.protoFluxTypes = protoFluxTypes;

            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            recognizer.LoadGrammar(ConstructCustomGrammar());

            recognizer.MaxAlternates = MaxAlternatesCount; // Set the maximum number of alternates

            recognizer.EndSilenceTimeout = TimeSpan.FromSeconds(2);
            recognizer.EndSilenceTimeoutAmbiguous = TimeSpan.FromSeconds(2);

            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            recognizer.SetInputToDefaultAudioDevice();
        }

        private Grammar ConstructCustomGrammar()
        {
            var phrases = protoFluxTypes.ConvertAll(typeInfo => typeInfo.NiceName);
            var choices = new Choices(phrases.ToArray());
            var gb = new GrammarBuilder(choices);
            return new Grammar(gb);
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
            var transcriptions = new TranscriptionCollection();

            foreach (var alternate in e.Result.Alternates)
            {
                // Add each transcription alternative to the TranscriptionsCollection
                transcriptions.AddTranscription(alternate.Text, alternate.Confidence);
            }

            // Add the TranscriptionsCollection to the history
            DisplayTranscriptionCollections(transcriptions);
        }

        private void DisplayTranscriptionCollections(TranscriptionCollection transcriptions)
        {
            // Adjusted to handle TranscriptionsCollection
            if (transcriptionListBox.InvokeRequired)
            {
                transcriptionListBox.Invoke(new Action<TranscriptionCollection>(DisplayTranscriptionCollections), new object[] { transcriptions });
            }
            else
            {
                transcriptionHistory.Add(transcriptions);
                // Limit the history size for demonstration
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
        }
    }


}
