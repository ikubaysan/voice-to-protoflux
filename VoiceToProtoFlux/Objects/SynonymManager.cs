using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceToProtoFlux.Objects
{
    public class SynonymManager
    {
        private readonly Dictionary<string, List<string>> synonyms = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        public SynonymManager()
        {
            // Initialize synonyms. Ensure to use lowercase for keys for case-insensitive matching.
            synonyms.Add("ulong", new List<string> { "UnsignedLong" });
            synonyms.Add("ushort", new List<string> { "UnsignedShort" });
            synonyms.Add("u", new List<string> { "Unsigned" });
            synonyms.Add("char", new List<string> { "Character" });
            synonyms.Add("uint", new List<string> { "UnsignedInt", "UnsignedInteger" });
            synonyms.Add("int", new List<string> { "Integer" });
            synonyms.Add("ref", new List<string> { "Reference" });
            synonyms.Add("add", new List<string> { "Addition" });
            synonyms.Add("seconds", new List<string> { "Secs" });
            synonyms.Add("minutes", new List<string> { "Mins" });
            synonyms.Add("concatenate", new List<string> { "Concat" });
            synonyms.Add("approximately", new List<string> { "Approx" });
            synonyms.Add("dynamic", new List<string> { "Dy" });
        }

        public IEnumerable<string> GetSynonyms(string word)
        {
            string lowercaseWord = word.ToLower();
            if (synonyms.ContainsKey(lowercaseWord))
            {
                return synonyms[word];
            }
            return new List<string>();
        }

        public bool HasSynonyms(string word) => synonyms.ContainsKey(word);
    }

}
