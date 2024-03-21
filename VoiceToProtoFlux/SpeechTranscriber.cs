using System;
using System.Collections.Generic;
using System.Speech.Recognition;
using System.Windows.Forms;

namespace VoiceToProtoFlux
{
    public class SpeechTranscriber
    {
        private readonly SpeechRecognitionEngine recognizer;
        private readonly List<string> transcriptionHistory = new List<string>();
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
            // Assuming you have a method to extract phrases from protoFluxTypes
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

            // Initialize a StringBuilder to concatenate matches and their confidence
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (var alternate in e.Result.Alternates)
            {
                // Append each match and its confidence to the StringBuilder
                sb.AppendFormat("{0} (Confidence: {1:N2}), ", alternate.Text, alternate.Confidence);
            }

            // Remove the last comma and space
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 2, 2);
            }

            // Call AddTranscription with the concatenated string of matches and confidences
            AddTranscription(sb.ToString());
        }

        private void AddTranscription(string transcription)
        {
            // Your existing thread-safe AddTranscription logic
            if (transcriptionListBox.InvokeRequired)
            {
                transcriptionListBox.Invoke(new Action<string>(AddTranscription), new object[] { transcription });
            }
            else
            {
                transcriptionHistory.Add(transcription);
                if (transcriptionHistory.Count > 10)
                {
                    transcriptionHistory.RemoveAt(0);
                }

                transcriptionListBox.Items.Clear();
                foreach (var item in transcriptionHistory)
                {
                    transcriptionListBox.Items.Add(item);
                }
            }
        }
    }
}
