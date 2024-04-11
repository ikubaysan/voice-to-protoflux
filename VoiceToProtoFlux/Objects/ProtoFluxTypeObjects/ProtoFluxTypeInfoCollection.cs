using System.Collections.Generic;
using System.Linq;

namespace VoiceToProtoFlux.Objects.ProtoFluxTypeObjects
{
    public class ProtoFluxTypeInfoCollection
    {
        public List<ProtoFluxTypeInfo> typeInfos = new List<ProtoFluxTypeInfo>();
        private Dictionary<string, ProtoFluxTypeInfo> nicePathMap = new Dictionary<string, ProtoFluxTypeInfo>(StringComparer.OrdinalIgnoreCase);

        public void AddTypeInfo(ProtoFluxTypeInfo typeInfo)
        {

            if (typeInfo.ParameterCount > 1)
            {
                return; // TODO: Currently don't support multiple parameters
            }

            typeInfos.Add(typeInfo);

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

            // Preprocess words: lowercase and trim non-alphanumeric characters at the end if applicable
            var processedWords = words.Select(word =>
            {
                // Convert the word to lowercase
                string lowerCasedWord = word.ToLowerInvariant();

                // If the word is more than one character long and ends with a non-alphanumeric character,
                // remove the last character
                if (lowerCasedWord.Length > 1 && !char.IsLetterOrDigit(lowerCasedWord[^1]))
                {
                    lowerCasedWord = lowerCasedWord.Substring(0, lowerCasedWord.Length - 1);
                }

                return lowerCasedWord;
            }).ToList();

            foreach (var typeInfo in typeInfos)
            {
                // Count how many words match (after preprocessing) with the current typeInfo's WordsOfNiceNameLowerCased
                int matchCount = processedWords.Count(word => typeInfo.WordsOfNiceNameLowerCased.Contains(word));

                if (matchCount > bestMatchCount)
                {
                    // Clear previous best matches as we found a better match
                    bestMatches.Clear();
                    // Add this typeInfo as the current best match
                    bestMatches.Add(typeInfo);
                    // Update the best match count
                    bestMatchCount = matchCount;
                }
                else if (matchCount == bestMatchCount && bestMatchCount > 0)
                {
                    // If this item matches as well as the current best, add it to the list of best matches
                    bestMatches.Add(typeInfo);
                }
            }
            // Return the list of best matches (can be empty if no matches found)
            return bestMatches;
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
