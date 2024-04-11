namespace VoiceToProtoFlux
{
    partial class MainForm
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
            rawTranscriptionListBox = new ListBox();
            transcriptionEnabledCheckBox = new CheckBox();
            label2 = new Label();
            label3 = new Label();
            interpretedTranscriptionListBox = new ListBox();
            defaultMicrophoneNameLabel = new Label();
            SuspendLayout();
            // 
            // rawTranscriptionListBox
            // 
            rawTranscriptionListBox.FormattingEnabled = true;
            rawTranscriptionListBox.Location = new Point(61, 440);
            rawTranscriptionListBox.Margin = new Padding(3, 4, 3, 4);
            rawTranscriptionListBox.Name = "rawTranscriptionListBox";
            rawTranscriptionListBox.Size = new Size(461, 304);
            rawTranscriptionListBox.TabIndex = 4;
            // 
            // transcriptionEnabledCheckBox
            // 
            transcriptionEnabledCheckBox.AutoSize = true;
            transcriptionEnabledCheckBox.Location = new Point(61, 341);
            transcriptionEnabledCheckBox.Margin = new Padding(3, 4, 3, 4);
            transcriptionEnabledCheckBox.Name = "transcriptionEnabledCheckBox";
            transcriptionEnabledCheckBox.Size = new Size(175, 24);
            transcriptionEnabledCheckBox.TabIndex = 5;
            transcriptionEnabledCheckBox.Text = "transcriptionEnabled?";
            transcriptionEnabledCheckBox.UseVisualStyleBackColor = true;
            transcriptionEnabledCheckBox.Click += transcriptionEnabledCheckBox_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(224, 399);
            label2.Name = "label2";
            label2.Size = new Size(125, 20);
            label2.TabIndex = 6;
            label2.Text = "Raw Transcription";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(766, 399);
            label3.Name = "label3";
            label3.Size = new Size(171, 20);
            label3.TabIndex = 8;
            label3.Text = "Interpreted Transcription";
            // 
            // interpretedTranscriptionListBox
            // 
            interpretedTranscriptionListBox.FormattingEnabled = true;
            interpretedTranscriptionListBox.Location = new Point(602, 440);
            interpretedTranscriptionListBox.Margin = new Padding(3, 4, 3, 4);
            interpretedTranscriptionListBox.Name = "interpretedTranscriptionListBox";
            interpretedTranscriptionListBox.Size = new Size(461, 304);
            interpretedTranscriptionListBox.TabIndex = 7;
            // 
            // defaultMicrophoneNameLabel
            // 
            defaultMicrophoneNameLabel.AutoSize = true;
            defaultMicrophoneNameLabel.Location = new Point(61, 39);
            defaultMicrophoneNameLabel.Name = "defaultMicrophoneNameLabel";
            defaultMicrophoneNameLabel.Size = new Size(130, 20);
            defaultMicrophoneNameLabel.TabIndex = 0;
            defaultMicrophoneNameLabel.Text = "Default Mic Name";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1219, 761);
            Controls.Add(label3);
            Controls.Add(interpretedTranscriptionListBox);
            Controls.Add(label2);
            Controls.Add(transcriptionEnabledCheckBox);
            Controls.Add(rawTranscriptionListBox);
            Controls.Add(defaultMicrophoneNameLabel);
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ListBox rawTranscriptionListBox;
        private CheckBox transcriptionEnabledCheckBox;
        private Label label2;
        private Label label3;
        private ListBox interpretedTranscriptionListBox;
        private Label defaultMicrophoneNameLabel;
    }
}
