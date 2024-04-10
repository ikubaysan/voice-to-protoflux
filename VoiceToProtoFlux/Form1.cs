using NAudio.Wave; // Include NAudio for audio handling
using System;
using System.Drawing; // For changing label colors
using System.Windows.Forms;
using System.Linq;
using VoiceToProtoFlux.Objects.ProtoFluxTypeObjects;
using VoiceToProtoFlux.Objects;
using System.Speech.Recognition;
using VoiceToProtoFlux.Objects.SpeechTranscriberObjects;

namespace VoiceToProtoFlux
{
    public partial class Form1 : Form
    {
        private SpeechTranscriber speechTranscriber;
        private string defaultMicrophoneName = "Unknown Microphone";
        private WebSocketServer webSocketServer;
        private ConfigManager configManager;

        public Form1()
        {
            InitializeComponent();
            configManager = new ConfigManager();
            // Subscribe to the Shown event
            this.Shown += Form1_Shown;
            IdentifyDefaultMicrophone();
            webSocketServer = new WebSocketServer("http://localhost:7159/");
            Task.Run(() => webSocketServer.StartAsync());

            ProtoFluxTypeInfoCollection typeInfoCollection = ProtoFluxTypeLoader.LoadProtoFluxTypes();
            speechTranscriber = new SpeechTranscriber(rawTranscriptionListBox, typeInfoCollection, webSocketServer);
            speechTranscriber.AudioLevelUpdated += SpeechTranscriber_AudioLevelUpdated;

            speechTranscriber.TranscriptionEnabledRequested += (sender, e) => EnableTranscription();
            speechTranscriber.TranscriptionDisabledRequested += (sender, e) => DisableTranscription();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            configManager.LoadConfig(); // Now this is called when the form is shown
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            webSocketServer?.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize labels to their default states
            isAudioDetectedLabel.Text = "Audio is not currently detected";
            isAudioDetectedLabel.ForeColor = Color.Red;
            isAudioDetectionConfirmedLabel.Text = "Audio detection not confirmed";
            isAudioDetectionConfirmedLabel.ForeColor = Color.Red;

            transcriptionEnabledCheckBox.Checked = false;
            return;
        }

        private void SpeechTranscriber_AudioLevelUpdated(object? sender, AudioLevelUpdatedEventArgs e)
        {
            UpdateAudioDetectionLabel(e.AudioLevel > 0);
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

        private void UpdateAudioDetectionLabel(bool detected)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool>(UpdateAudioDetectionLabel), new object[] { detected });
                return;
            }

            if (detected)
            {
                isAudioDetectedLabel.Text = "Audio is currently detected";
                isAudioDetectedLabel.ForeColor = Color.Green;
                // As soon as we detect any audio, we can confirm that the audio detection is working
                isAudioDetectionConfirmedLabel.Text = $"Audio detection confirmed for {defaultMicrophoneName}.";
                isAudioDetectionConfirmedLabel.ForeColor = Color.Green;
            }
            else
            {
                isAudioDetectedLabel.Text = "Audio not currently detected";
                isAudioDetectedLabel.ForeColor = Color.Red;
            }
        }

        private void transcriptionEnabledCheckBox_Click(object sender, EventArgs e)
        {
            if (transcriptionEnabledCheckBox.Checked)
            {
                speechTranscriber.StartRecognition();
            }
            else
            {
                speechTranscriber.StopRecognition();
            }
        }



        private void EnableTranscription()
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
                    speechTranscriber.StartRecognition();
                }
            }
        }

        private void DisableTranscription()
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
                    speechTranscriber.StopRecognition();
                }
            }
        }




    }
}
