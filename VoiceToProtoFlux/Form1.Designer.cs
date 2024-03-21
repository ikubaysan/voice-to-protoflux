namespace VoiceToProtoFlux
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            microphoneListBox = new ListBox();
            isAudioDetectedLabel = new Label();
            isAudioDetectionConfirmedLabel = new Label();
            rawTranscriptionListBox = new ListBox();
            transcriptionEnabledCheckBox = new CheckBox();
            label2 = new Label();
            label3 = new Label();
            interpretedTranscriptionListBox = new ListBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(53, 29);
            label1.Name = "label1";
            label1.Size = new Size(115, 15);
            label1.TabIndex = 0;
            label1.Text = "Select a Microphone";
            // 
            // microphoneListBox
            // 
            microphoneListBox.FormattingEnabled = true;
            microphoneListBox.ItemHeight = 15;
            microphoneListBox.Items.AddRange(new object[] { "a", "b", "c" });
            microphoneListBox.Location = new Point(53, 77);
            microphoneListBox.Name = "microphoneListBox";
            microphoneListBox.Size = new Size(404, 94);
            microphoneListBox.TabIndex = 1;
            microphoneListBox.SelectedIndexChanged += microphoneListBox_SelectedIndexChanged;
            // 
            // isAudioDetectedLabel
            // 
            isAudioDetectedLabel.AutoSize = true;
            isAudioDetectedLabel.Location = new Point(53, 195);
            isAudioDetectedLabel.Name = "isAudioDetectedLabel";
            isAudioDetectedLabel.Size = new Size(105, 15);
            isAudioDetectedLabel.TabIndex = 2;
            isAudioDetectedLabel.Text = "Is Audio Detected?";
            // 
            // isAudioDetectionConfirmedLabel
            // 
            isAudioDetectionConfirmedLabel.AutoSize = true;
            isAudioDetectionConfirmedLabel.Location = new Point(53, 226);
            isAudioDetectionConfirmedLabel.Name = "isAudioDetectionConfirmedLabel";
            isAudioDetectionConfirmedLabel.Size = new Size(105, 15);
            isAudioDetectionConfirmedLabel.TabIndex = 3;
            isAudioDetectionConfirmedLabel.Text = "Is Audio Detected?";
            // 
            // rawTranscriptionListBox
            // 
            rawTranscriptionListBox.FormattingEnabled = true;
            rawTranscriptionListBox.ItemHeight = 15;
            rawTranscriptionListBox.Location = new Point(53, 330);
            rawTranscriptionListBox.Name = "rawTranscriptionListBox";
            rawTranscriptionListBox.Size = new Size(404, 229);
            rawTranscriptionListBox.TabIndex = 4;
            // 
            // transcriptionEnabledCheckBox
            // 
            transcriptionEnabledCheckBox.AutoSize = true;
            transcriptionEnabledCheckBox.Location = new Point(53, 256);
            transcriptionEnabledCheckBox.Name = "transcriptionEnabledCheckBox";
            transcriptionEnabledCheckBox.Size = new Size(140, 19);
            transcriptionEnabledCheckBox.TabIndex = 5;
            transcriptionEnabledCheckBox.Text = "transcriptionEnabled?";
            transcriptionEnabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(196, 299);
            label2.Name = "label2";
            label2.Size = new Size(100, 15);
            label2.TabIndex = 6;
            label2.Text = "Raw Transcription";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(670, 299);
            label3.Name = "label3";
            label3.Size = new Size(136, 15);
            label3.TabIndex = 8;
            label3.Text = "Interpreted Transcription";
            // 
            // interpretedTranscriptionListBox
            // 
            interpretedTranscriptionListBox.FormattingEnabled = true;
            interpretedTranscriptionListBox.ItemHeight = 15;
            interpretedTranscriptionListBox.Location = new Point(527, 330);
            interpretedTranscriptionListBox.Name = "interpretedTranscriptionListBox";
            interpretedTranscriptionListBox.Size = new Size(404, 229);
            interpretedTranscriptionListBox.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1067, 571);
            Controls.Add(label3);
            Controls.Add(interpretedTranscriptionListBox);
            Controls.Add(label2);
            Controls.Add(transcriptionEnabledCheckBox);
            Controls.Add(rawTranscriptionListBox);
            Controls.Add(isAudioDetectionConfirmedLabel);
            Controls.Add(isAudioDetectedLabel);
            Controls.Add(microphoneListBox);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ListBox microphoneListBox;
        private Label isAudioDetectedLabel;
        private Label isAudioDetectionConfirmedLabel;
        private ListBox rawTranscriptionListBox;
        private CheckBox transcriptionEnabledCheckBox;
        private Label label2;
        private Label label3;
        private ListBox interpretedTranscriptionListBox;
    }
}
