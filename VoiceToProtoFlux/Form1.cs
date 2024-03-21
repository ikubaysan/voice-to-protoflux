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
        private string defaultMicrophoneName = "Unknown Microphone";

        public Form1()
        {
            InitializeComponent();
            IdentifyDefaultMicrophone();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize labels to their default states
            isAudioDetectedLabel.Text = "Audio is not currently detected";
            isAudioDetectedLabel.ForeColor = Color.Red;
            isAudioDetectionConfirmedLabel.Text = "Audio detection not confirmed";
            isAudioDetectionConfirmedLabel.ForeColor = Color.Red;

            // Update default microphone name label
            defaultMicrophoneNameLabel.Text = $"Your default mic: {defaultMicrophoneName}";
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

            // Automatically start listening to the default microphone
            StartListeningToMicrophone(0);
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
                isAudioDetectionConfirmedLabel.Text = $"Audio detection confirmed for {defaultMicrophoneName}.";
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
            }
        }
    }
}
