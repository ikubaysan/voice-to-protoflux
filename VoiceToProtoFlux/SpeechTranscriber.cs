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

        // Define your specific phrases here
        private readonly string[] phrases = {
            "hello", "testing",
            "dynamic", "impulse", "receiver", "trigger", "float", "integer",
            "string", "data", "model", "store", "DynamicVariable", "DynamicImpulseReceiver", "DynamicImpulseTrigger"
        };

        private Grammar ConstructCustomGrammar()
        {
            var choices = new Choices(phrases);
            var gb = new GrammarBuilder(choices);
            return new Grammar(gb);
        }

        public SpeechTranscriber(ListBox listBox, CheckBox checkBox)
        {
            this.transcriptionListBox = listBox;
            this.transcriptionEnabledCheckBox = checkBox;

            recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            recognizer.LoadGrammar(ConstructCustomGrammar());
            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            recognizer.SetInputToDefaultAudioDevice();
        }

        public void StartRecognition()
        {
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void StopRecognition()
        {
            recognizer.RecognizeAsyncStop();
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (!transcriptionEnabledCheckBox.Checked) return;

            // Directly use the recognized text since it's based on your custom grammar
            string transcription = e.Result.Text;

            AddTranscription(transcription);
        }

        private void AddTranscription(string transcription)
        {
            // Ensure operation is thread-safe
            if (transcriptionListBox.InvokeRequired)
            {
                transcriptionListBox.Invoke(new Action<string>(AddTranscription), new object[] { transcription });
            }
            else
            {
                transcriptionHistory.Add(transcription);
                // Keep only the last 5 messages
                if (transcriptionHistory.Count > 5)
                {
                    transcriptionHistory.RemoveAt(0);
                }

                // Update the ListBox
                transcriptionListBox.Items.Clear(); // Make sure this operation is on the UI thread
                foreach (var item in transcriptionHistory)
                {
                    transcriptionListBox.Items.Add(item); // Add each item in the history
                }
            }
        }
    }
}
