using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceToProtoFlux.Objects.TranscriptionObjects
{
    public class Transcription
    {
        public string FullName { get; set; }
        public string NiceName { get; set; }
        public int ParameterCount { get; set; }
        public float Confidence { get; set; }


        public Transcription(string fullName, string niceName, int parameterCount, float confidence)
        {
            FullName = fullName;
            NiceName = niceName;
            ParameterCount = parameterCount;
            Confidence = confidence;
        }

        public string ToWebsocketString()
        {
            string buttonRelayArgument = "";
            buttonRelayArgument += FullName;
            return $"{ParameterCount}|{buttonRelayArgument}|{NiceName}";
        }

        public override string ToString()
        {
            return $"{FullName} (Confidence: {Confidence:N2})";
        }
    }
}
