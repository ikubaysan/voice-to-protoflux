using System.Collections.Generic;
using VoiceToProtoFlux.Objects.ProtoFluxParameterObjects;

namespace VoiceToProtoFlux.Objects
{
    public class ProtoFluxParameterCollection
    {
        private static readonly ProtoFluxParameterCollection instance = new ProtoFluxParameterCollection();
        public Dictionary<string, ProtoFluxParameter> Parameters { get; private set; }

        private ProtoFluxParameterCollection()
        {
            // Assuming case sensitivity is not required for the initialization
            Parameters = new Dictionary<string, ProtoFluxParameter>(System.StringComparer.OrdinalIgnoreCase)
            {
                { "Int", new ProtoFluxParameter("Int") },
                { "Bool", new ProtoFluxParameter("Bool") },
                { "Float", new ProtoFluxParameter("Float") },
                { "Color", new ProtoFluxParameter("Color") },
                { "ColorX", new ProtoFluxParameter("ColorX") },
                { "String", new ProtoFluxParameter("String") },
                { "Double", new ProtoFluxParameter("Double") },
                { "Uint", new ProtoFluxParameter("Uint") },
                { "Ulong", new ProtoFluxParameter("Ulong") },
                { "Slot", new ProtoFluxParameter("Slot") },
                { "URI", new ProtoFluxParameter("URI") }
            };
        }

        public static ProtoFluxParameterCollection Instance
        {
            get { return instance; }
        }

        public ProtoFluxParameter GetDefaultParameter()
        {
            return Parameters["Int"];
        }

        public ProtoFluxParameter? GetParameterByName(string name, bool caseSensitive = false)
        {
            if (caseSensitive)
            {
                // Perform a case-sensitive search directly using the dictionary
                if (Parameters.ContainsKey(name))
                {
                    return Parameters[name];
                }
            }
            else
            {
                // Perform a case-insensitive search
                foreach (var entry in Parameters)
                {
                    if (string.Equals(entry.Key, name, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return entry.Value;
                    }
                }
            }
            return null;
        }
    }
}
