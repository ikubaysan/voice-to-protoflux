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

            using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) throw new FileNotFoundException($"Resource '{resourceName}' not found.");
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();

                    // Deserialize JSON into List<ProtoFluxTypeInfo> object.
                    List<ProtoFluxTypeInfo>? types = JsonSerializer.Deserialize<List<ProtoFluxTypeInfo>>(json);

                    if (types == null) throw new JsonException("Failed to deserialize JSON into List<ProtoFluxTypeInfo>.");

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
}
