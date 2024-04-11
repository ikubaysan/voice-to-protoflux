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

        public List<string> GetParameterNames()
        {
            return Parameters.Keys.ToList();
        }

        public List<string> GetParameterNamesLowerCased()
        {
            return Parameters.Keys.Select(key => key.ToLowerInvariant()).ToList();
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

        public ProtoFluxParameter? FindBestMatchingParameterByWords(List<string> words)
        {
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


            List<string> parameterNamesLowercased = GetParameterNamesLowerCased();
            foreach (var processedWord in processedWords)
            {
                if (parameterNamesLowercased.Contains(processedWord))
                {
                    if (Parameters.TryGetValue(processedWord, out var parameter))
                    {
                        return parameter;
                    }
                }
            }

            return null;
        }

    }
}
