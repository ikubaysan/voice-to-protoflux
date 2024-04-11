using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceToProtoFlux.Objects.ProtoFluxParameterObjects
{
    public class ProtoFluxParameter
    {
        public string Name { get; set; }

        public ProtoFluxParameter(string name)
        {
            Name = name;
        }
    }
}
