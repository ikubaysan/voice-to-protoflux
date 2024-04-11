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

            typeInfoCollection = ProtoFluxTypeLoader.LoadProtoFluxTypes();
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
            ProtoFluxParameterCollection.Instance.GetParameterNames().ForEach(word => grammarWords.Add(word));

            // Pass the list to AzureSpeechTranscriber
            azureSpeechTranscriber = new AzureSpeechTranscriber(
                configManager,
                webSocketServer,
                typeInfoCollection,
                grammarWords.ToList()
                );

            var messageHandler = new WebSocketServerReceivedMessageHandler(webSocketServer, azureSpeechTranscriber, typeInfoCollection);

            // Adjusting the event subscription to accommodate async method
            webSocketServer.OnMessageReceived += (message) =>
            {
                // The _ = is used to explicitly discard the Task returned by the async method.
                // This acknowledges that we're intentionally not awaiting the Task.
                _ = messageHandler.OnMessageReceivedAsync(message);
            };

            Task.Run(() => webSocketServer.StartAsync());

            transcriptionEnabledCheckBox.Checked = false;
            azureSpeechTranscriber.RecognitionEnabledChanged += OnRecognitionEnabledChanged;
        }


        private void Form1_Shown(object sender, EventArgs e)
        {
            if (configManager.TerminalMessageBoxNeeded)
            {
                MessageBox.Show(configManager.TerminalMessageBoxText);
                Application.Exit();
            }

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            webSocketServer?.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            return;
        }

        private void OnRecognitionEnabledChanged(bool enabled)
        {
            // Marshal the call onto the UI thread
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(OnRecognitionEnabledChanged), enabled);
            }
            else
            {
                transcriptionEnabledCheckBox.Checked = enabled;
            }
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
    }
}
