using System.Collections.Generic;
using System.Linq;

namespace VoiceToProtoFlux.Objects.ProtoFluxTypeObjects
{
    public class ProtoFluxTypeInfoCollection
    {
        public List<ProtoFluxTypeInfo> typeInfos = new List<ProtoFluxTypeInfo>();
        private Dictionary<string, ProtoFluxTypeInfo> phraseMap = new Dictionary<string, ProtoFluxTypeInfo>(StringComparer.OrdinalIgnoreCase);

        public void AddTypeInfo(ProtoFluxTypeInfo typeInfo)
        {
            typeInfos.Add(typeInfo);
            foreach (var phrase in typeInfo.Phrases)
            {
                if (!phraseMap.ContainsKey(phrase))
                {
                    phraseMap[phrase] = typeInfo;
                }
            }
        }

        public ProtoFluxTypeInfo GetTypeInfoByPhrase(string phrase)
        {
            phraseMap.TryGetValue(phrase, out ProtoFluxTypeInfo typeInfo);
            return typeInfo;
        }

        // Method to determine a unique set of the WordsOfNiceName values from all ProtoFluxTypeInfo objects
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
