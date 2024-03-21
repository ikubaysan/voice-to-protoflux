using NAudio.Wave; // Include NAudio for audio handling
using System;
using System.Drawing; // For changing label colors
using System.Windows.Forms;
using System.Linq;


namespace VoiceToProtoFlux
{
    public partial class Form1 : Form
    {
        private WaveInEvent? waveSource = null;
        private bool audioDetected = false;
        private SpeechTranscriber? speechTranscriber = null;

        public Form1()
        {
            InitializeComponent();
            PopulateMicrophones();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize both labels to their default states
            isAudioDetectedLabel.Text = "Audio is not currently detected";
            isAudioDetectedLabel.ForeColor = Color.Red;
            // Initialize isAudioDetectionConfirmedLabel
            isAudioDetectionConfirmedLabel.Text = "Audio detection not confirmed";
            isAudioDetectionConfirmedLabel.ForeColor = Color.Red;
        }

        private void PopulateMicrophones()
        {
            microphoneListBox.Items.Clear();
            for (int n = 0; n < WaveIn.DeviceCount; n++)
            {
                var device = WaveIn.GetCapabilities(n);
                microphoneListBox.Items.Add(device.ProductName);
            }
        }

        private void microphoneListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            StartListeningToMicrophone(microphoneListBox.SelectedIndex);
            ResetAudioDetectionLabel();
            // Also reset isAudioDetectionConfirmedLabel
            isAudioDetectionConfirmedLabel.Text = "Audio detection not confirmed";
            isAudioDetectionConfirmedLabel.ForeColor = Color.Red;
        }

        private void StartListeningToMicrophone(int deviceNumber)
        {
            StopListening(); // Stop previous listening

            waveSource = new WaveInEvent();
            waveSource.DeviceNumber = deviceNumber;
            waveSource.WaveFormat = new WaveFormat(44100, 1); // CD quality audio in mono

            waveSource.DataAvailable += OnDataAvailable;
            waveSource.StartRecording();
        }

        private void StopListening()
        {
            if (waveSource != null)
            {
                waveSource.StopRecording();
                waveSource.DataAvailable -= OnDataAvailable;
                waveSource.Dispose();
                waveSource = null;
            }
        }

        private void OnDataAvailable(object? sender, WaveInEventArgs e)
        {
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index]);
                if (sample > 500)
                {
                    if (!audioDetected)
                    {
                        audioDetected = true;
                        UpdateAudioDetectionLabel(true);
                    }
                    return;
                }
            }

            if (audioDetected)
            {
                audioDetected = false;
                UpdateAudioDetectionLabel(false);
            }
        }

        private void ResetAudioDetectionLabel()
        {
            UpdateAudioDetectionLabel(false);
        }

        private void UpdateAudioDetectionLabel(bool detected)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool>(UpdateAudioDetectionLabel), new object[] { detected });
                return;
            }

            string micName = microphoneListBox.SelectedItem?.ToString() ?? "Unknown Microphone";

            if (detected)
            {
                isAudioDetectedLabel.Text = "Audio is currently detected";
                isAudioDetectedLabel.ForeColor = Color.Green;
                isAudioDetectionConfirmedLabel.Text = $"Audio detection confirmed for {micName}.";
                isAudioDetectionConfirmedLabel.ForeColor = Color.Green;

                // Initialize the speech transcriber once if not already started
                if (speechTranscriber == null)
                {
                    speechTranscriber = new SpeechTranscriber(rawTranscriptionListBox, transcriptionEnabledCheckBox);
                    speechTranscriber.StartRecognition();
                }
            }
            else
            {
                isAudioDetectedLabel.Text = "Audio not currently detected";
                isAudioDetectedLabel.ForeColor = Color.Red;

                // Consider whether you want to stop recognition here or not.
                // If you stop and dispose of the recognizer, you'll need to create a new instance when audio is detected again.
                // This example does not stop the SpeechTranscriber instance but you could manage its lifecycle according to your needs.
            }
        }




    }
}