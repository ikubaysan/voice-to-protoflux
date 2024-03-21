using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace VoiceToProtoFlux
{
    public static class ProtoFluxTypeLoader
    {
        public static List<ProtoFluxTypeInfo> LoadProtoFluxTypes()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "VoiceToProtoFlux.Resources.ProtoFluxTypes.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                List<ProtoFluxTypeInfo> types = JsonSerializer.Deserialize<List<ProtoFluxTypeInfo>>(json);
                return types ?? new List<ProtoFluxTypeInfo>(); // Ensure a non-null list is returned
            }
        }
    }
}
