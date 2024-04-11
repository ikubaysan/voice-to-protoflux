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
        public ProtoFluxTypeInfo ProtoFluxTypeInfo { get; set; }
        public List<ProtoFluxParameter> ProvidedParameters { get; set; }


        public Transcription(ProtoFluxTypeInfo protoFluxTypeInfo, List<ProtoFluxParameter> providedParameters)
        {
            ProtoFluxTypeInfo = protoFluxTypeInfo;
            ProvidedParameters = providedParameters;
        }

        public string ToWebsocketString()
        {
            string parameterName = " ";
            string niceName = ProtoFluxTypeInfo.NiceName;

            if (ProtoFluxTypeInfo.ParameterCount > 0)
            { 
                // get the 1st parameter name
                parameterName = ProvidedParameters[0].Name;

                /*
                if (!ProtoFluxTypeInfo.RequiresObjectParameter)
                { 
                    // If a value parameter is required and not an object parameter, the parameter name should be in lowercase
                    parameterName = parameterName.ToLower();
                }
                */

                // TODO: For object parameters, it's not clear or consistent as to when the parameter should be capitalized. 
                // So for now, we'll always capitalize.
                parameterName = parameterName.ToLower();


                // Replace <T> with the actual type
                if (niceName.Contains("<T>"))
                {
                    niceName = niceName.Replace("<T>", $"<{parameterName}>");
                }
            }

            // the button relay argument is ProtoFluxTypeInfo.FullName
            return $"Type0_{ProtoFluxTypeInfo.ParameterCount}|{parameterName}|{ProtoFluxTypeInfo.FullName}|{ProtoFluxTypeInfo.NicePath}|{niceName}";
        }

        public override string ToString()
        {
            return $"{ProtoFluxTypeInfo.FullName} w/ {ProvidedParameters} parameters: {string.Join(", ", ProvidedParameters.Select(p => p.Name))}";
        }
    }
}
