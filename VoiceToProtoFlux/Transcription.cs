using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
