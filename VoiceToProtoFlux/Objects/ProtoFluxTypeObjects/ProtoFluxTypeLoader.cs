using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace VoiceToProtoFlux.Objects.ProtoFluxTypeObjects
{
    public static class ProtoFluxTypeLoader
    {
        public static ProtoFluxTypeInfoCollection LoadProtoFluxTypes()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "VoiceToProtoFlux.Resources.ProtoFluxTypes.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();

                // Deserialize into List<ProtoFluxTypeInfo>, assuming JSON structure matches the class structure
                List<ProtoFluxTypeInfo> types = JsonSerializer.Deserialize<List<ProtoFluxTypeInfo>>(json);

                ProtoFluxTypeInfoCollection typeInfoCollection = new ProtoFluxTypeInfoCollection();

                foreach (ProtoFluxTypeInfo type in types)
                {
                    typeInfoCollection.AddTypeInfo(type);
                }

                return typeInfoCollection;
            }
        }
    }
}
