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

        public void AddTranscription(Transcription transcription)
        {
            transcriptions.Add(transcription);
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
