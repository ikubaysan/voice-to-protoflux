using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceToProtoFlux.Objects.TranscriptionObjects
{
    public class TranscriptionCollection
    {
        public List<Transcription> transcriptions = new List<Transcription>();
        public int MaxAlternatesCount { get; set; } = 5; // Default to 5, can be adjusted

        public void AddTranscription(string text, float confidence)
        {
            if (transcriptions.Count >= MaxAlternatesCount)
            {
                transcriptions.RemoveAt(0); // Ensure we do not exceed MaxAlternatesCount
            }
            transcriptions.Add(new Transcription(text, confidence));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var transcription in transcriptions)
            {
                sb.AppendLine(transcription.ToString());
            }
            return sb.ToString().TrimEnd(); // Remove the last newline for a clean output
        }
    }
}
