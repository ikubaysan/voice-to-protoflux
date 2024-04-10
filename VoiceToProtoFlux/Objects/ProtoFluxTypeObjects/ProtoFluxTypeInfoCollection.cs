using System.Collections.Generic;
using System.Linq;

namespace VoiceToProtoFlux.Objects.ProtoFluxTypeObjects
{
    public class ProtoFluxTypeInfoCollection
    {
        public List<ProtoFluxTypeInfo> typeInfos = new List<ProtoFluxTypeInfo>();
        private Dictionary<string, ProtoFluxTypeInfo> phraseMap = new Dictionary<string, ProtoFluxTypeInfo>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, ProtoFluxTypeInfo> nicePathMap = new Dictionary<string, ProtoFluxTypeInfo>(StringComparer.OrdinalIgnoreCase);

        public void AddTypeInfo(ProtoFluxTypeInfo typeInfo)
        {

            if (typeInfo.ParameterCount > 1)
            {
                return; // TODO: Currently don't support multiple parameters
            }

            typeInfos.Add(typeInfo);
            foreach (var phrase in typeInfo.Phrases)
            {
                if (!phraseMap.ContainsKey(phrase))
                {
                    phraseMap[phrase] = typeInfo;
                }
            }

            if (!nicePathMap.ContainsKey(typeInfo.NicePath))
            {
                nicePathMap[typeInfo.NicePath] = typeInfo;
            }
        }


        public List<ProtoFluxTypeInfo> FindBestMatchingTypeInfoByWords(List<string> words)
        {
            // Returning a list because there could be multiple best matches
            List<ProtoFluxTypeInfo> bestMatches = new List<ProtoFluxTypeInfo>();
            int bestMatchCount = 0;
            foreach (var typeInfo in typeInfos)
            {
                int matchCount = words.Count(word => typeInfo.WordsOfNiceName.Contains(word));
                if (matchCount > bestMatchCount)
                {
                    bestMatches.Clear(); // Clear previous best matches as we found a better match
                    bestMatches.Add(typeInfo); // Add this typeInfo as the current best match
                    bestMatchCount = matchCount; // Update the best match count
                }
                else if (matchCount == bestMatchCount && bestMatchCount > 0)
                {
                    // If this item matches as well as the current best, add it to the list of best matches
                    bestMatches.Add(typeInfo);
                }
            }
            return bestMatches; // Return the list of best matches (can be empty if no matches found)
        }



        public ProtoFluxTypeInfo? GetTypeInfoByPhrase(string phrase)
        {
            phraseMap.TryGetValue(phrase, out ProtoFluxTypeInfo? typeInfo);
            return typeInfo;
        }

        public ProtoFluxTypeInfo? GetTypeInfoByNicePath(string nicePath)
        {
            nicePathMap.TryGetValue(nicePath, out ProtoFluxTypeInfo? typeInfo);
            return typeInfo;
        }

        // Method to determine a unique set of the WordsOfNiceName values from all ProtoFluxTypeInfo objects
        public HashSet<string> GetUniqueWordsOfNiceNames()
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
