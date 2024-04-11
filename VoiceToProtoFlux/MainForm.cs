using NAudio.Wave; // Include NAudio for audio handling
using System;
using System.Drawing; // For changing label colors
using System.Windows.Forms;
using System.Linq;
using VoiceToProtoFlux.Objects.ProtoFluxTypeObjects;
using VoiceToProtoFlux.Objects;
using System.Speech.Recognition;
using VoiceToProtoFlux.Objects.SpeechTranscriberObjects;
using VoiceToProtoFlux.Objects.WebsocketServerObjects;

namespace VoiceToProtoFlux
{
    public partial class MainForm : Form
    {
        private string defaultMicrophoneName = "Unknown Microphone";
        private WebSocketServer webSocketServer;
        private ConfigManager configManager;
        private AzureSpeechTranscriber azureSpeechTranscriber;
        private ProtoFluxTypeInfoCollection typeInfoCollection;

        public MainForm()
        {
            InitializeComponent();
            configManager = new ConfigManager();
            // Subscribe to the Shown event
            this.Shown += Form1_Shown;
            IdentifyDefaultMicrophone();
            webSocketServer = new WebSocketServer("http://localhost:7159/");
            Task.Run(() => webSocketServer.StartAsync());

            typeInfoCollection = ProtoFluxTypeLoader.LoadProtoFluxTypes();

            /*
            speechTranscriber = new SpeechTranscriber(rawTranscriptionListBox, typeInfoCollection, webSocketServer);
            speechTranscriber.AudioLevelUpdated += SpeechTranscriber_AudioLevelUpdated;
            speechTranscriber.TranscriptionEnabledRequested += (sender, e) => EnableTranscription();
            speechTranscriber.TranscriptionDisabledRequested += (sender, e) => DisableTranscription();
            */
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            configManager.LoadConfig(); // Now this is called when the form is shown

            // Convert HashSet to List
            HashSet<string> grammarWords = typeInfoCollection.GetUniqueWordsOfNiceNames();

            List<string> additionalWords = new List<string>
            {
                "Of",
                "Type"
            };

            foreach (string word in additionalWords)
            {
                grammarWords.Add(word);
            }

            SynonymManager.GetAllWords().ForEach(word => grammarWords.Add(word));

            // Pass the list to AzureSpeechTranscriber
            azureSpeechTranscriber = new AzureSpeechTranscriber(
                configManager, 
                webSocketServer,
                typeInfoCollection, 
                grammarWords.ToList()
                );
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            webSocketServer?.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            transcriptionEnabledCheckBox.Checked = false;
            return;
        }

        private void IdentifyDefaultMicrophone()
        {
            if (WaveIn.DeviceCount == 0)
            {
                MessageBox.Show("No microphones found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // Close the form if no microphones are found
                return;
            }

            // Assuming the default microphone is always at index 0 (common but not guaranteed)
            var defaultMic = WaveIn.GetCapabilities(0);
            defaultMicrophoneName = defaultMic.ProductName;

            // No longer start listening here, SpeechRecognitionEngine will handle it
            // Update default microphone name label
            defaultMicrophoneNameLabel.Text = $"Your default mic: {defaultMicrophoneName}";
        }

        private void transcriptionEnabledCheckBox_Click(object sender, EventArgs e)
        {
            if (transcriptionEnabledCheckBox.Checked)
            {
                //speechTranscriber.StartRecognition();
                azureSpeechTranscriber.StartRecognitionAsync();
            }
            else
            {
                //speechTranscriber.StopRecognition();
                azureSpeechTranscriber.StopRecognitionAsync();
            }
        }

        private async void EnableTranscription()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(EnableTranscription));
            }
            else
            {
                if (!transcriptionEnabledCheckBox.Checked)
                {
                    transcriptionEnabledCheckBox.Checked = true;
                    //speechTranscriber.StartRecognition();
                    await azureSpeechTranscriber.StartRecognitionAsync();
                }
            }
        }

        private async void DisableTranscription()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(DisableTranscription));
            }
            else
            {
                if (transcriptionEnabledCheckBox.Checked)
                {
                    transcriptionEnabledCheckBox.Checked = false;
                    // speechTranscriber.StopRecognition();
                    await azureSpeechTranscriber.StopRecognitionAsync();
                }
            }
        }
    }
}
