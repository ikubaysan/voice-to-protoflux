using System;
using System.Collections.Generic;

namespace VoiceToProtoFlux.Objects
{
    public static class SynonymManager
    {
        // Static field initialized directly.
        private static readonly Dictionary<string, List<string>> synonyms = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            // Key: the actual word
            // Value: list of words you can say which are synonymous
            {"ulong", new List<string> { "UnsignedLong" }},
            {"ushort", new List<string> { "UnsignedShort" }},
            {"u", new List<string> { "Unsigned" }},
            {"char", new List<string> { "Character" }},
            {"uint", new List<string> { "UnsignedInt", "UnsignedInteger" }},
            {"int", new List<string> { "Integer" }},
            {"ref", new List<string> { "Reference" }},
            {"add", new List<string> { "Addition" }},
            {"seconds", new List<string> { "Secs" }},
            {"minutes", new List<string> { "Mins" }},
            {"concatenate", new List<string> { "Concat" }},
            {"approximately", new List<string> { "Approx" }},
            {"dynamic", new List<string> { "Dy", "Dyn" }},
            {"bool", new List<string> { "Boolean" } },
            {"reference", new List<string> { "Ref" }},
            {"uri", new List<string> { "URL" }}
        };

        // Since we no longer have a constructor, we initialize the dictionary directly above.

        public static IEnumerable<string> GetSynonyms(string word)
        {
            string lowercaseWord = word.ToLower();
            if (synonyms.ContainsKey(lowercaseWord))
            {
                return synonyms[lowercaseWord];
            }
            return new List<string>();
        }

        public static bool HasSynonyms(string word)
        {
            return synonyms.ContainsKey(word.ToLower());
        }
    }
}
