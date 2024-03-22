using System.Collections.Generic;
using System.Linq;

namespace VoiceToProtoFlux.Objects.ProtoFluxTypeObjects
{
    public class ProtoFluxTypeInfoCollection
    {
        public List<ProtoFluxTypeInfo> typeInfos = new List<ProtoFluxTypeInfo>();

        public void AddTypeInfo(ProtoFluxTypeInfo typeInfo)
        {
            typeInfos.Add(typeInfo);
        }

        // Method to determine a unique set of WordsOfNiceName
        public HashSet<string> GetUniqueWordsOfNiceName()
        {
            var uniqueWords = new HashSet<string>();
            foreach (var typeInfo in typeInfos)
            {
                foreach (var word in typeInfo.WordsOfNiceName)
                {
                    uniqueWords.Add(word);
                }
            }
            return uniqueWords;
        }
    }
}
