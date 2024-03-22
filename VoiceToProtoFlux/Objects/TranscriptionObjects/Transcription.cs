using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceToProtoFlux.Objects.ProtoFluxParameterObjects;

namespace VoiceToProtoFlux.Objects.TranscriptionObjects
{
    public class Transcription
    {
        public string FullName { get; set; }
        public string NiceName { get; set; }
        public int ParameterCount { get; set; }
        public List<ProtoFluxParameter> ProvidedParameters { get; set; }
        public float Confidence { get; set; }


        public Transcription(string fullName, string niceName, int parameterCount, List<ProtoFluxParameter> providedParameters, float confidence)
        {
            FullName = fullName;
            NiceName = niceName;
            ParameterCount = parameterCount;
            ProvidedParameters = providedParameters;
            Confidence = confidence;
        }

        public string ToWebsocketString()
        {
            string buttonRelayArgument = "";
            buttonRelayArgument += FullName;


            string parameterName = "";
            if (ProvidedParameters.Count > 0)
            { 
                // get the 1st parameter name
                parameterName = ProvidedParameters[0].Name;
            }

            return $"{ParameterCount}|{parameterName}|{buttonRelayArgument}|{NiceName}";
        }

        public override string ToString()
        {
            return $"{FullName} (Confidence: {Confidence:N2})";
        }
    }
}
