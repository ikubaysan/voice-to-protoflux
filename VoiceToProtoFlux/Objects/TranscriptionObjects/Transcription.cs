using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceToProtoFlux.Objects.ProtoFluxParameterObjects;
using VoiceToProtoFlux.Objects.ProtoFluxTypeObjects;

namespace VoiceToProtoFlux.Objects.TranscriptionObjects
{
    public class Transcription
    {
        public int ParameterCount { get; set; }
        public ProtoFluxTypeInfo ProtoFluxTypeInfo { get; set; }
        public List<ProtoFluxParameter> ProvidedParameters { get; set; }
        public float Confidence { get; set; }


        public Transcription(ProtoFluxTypeInfo protoFluxTypeInfo, List<ProtoFluxParameter> providedParameters, float confidence)
        {
            ProtoFluxTypeInfo = protoFluxTypeInfo;
            ProvidedParameters = providedParameters;
            Confidence = confidence;
        }

        public string ToWebsocketString()
        {
            string buttonRelayArgument = "";
            buttonRelayArgument += ProtoFluxTypeInfo.FullName;
            string parameterName = " ";
            string niceName = ProtoFluxTypeInfo.NiceName;

            if (ProvidedParameters.Count > 0)
            { 
                // get the 1st parameter name
                parameterName = ProvidedParameters[0].Name;

                if (!ProtoFluxTypeInfo.RequiresObjectParameter)
                { 
                    // If a value parameter is required and not an object parameter, the parameter name should be in lowercase
                    parameterName = parameterName.ToLower();
                }

                // Replace <T> with the actual type
                if (niceName.Contains("<T>"))
                {
                    niceName = niceName.Replace("<T>", $"<{parameterName}>");
                }
            }

            return $"{ParameterCount}|{parameterName}|{buttonRelayArgument}|{niceName}";
        }

        public override string ToString()
        {
            return $"{ProtoFluxTypeInfo.FullName} (Confidence: {Confidence:N2})";
        }
    }
}
