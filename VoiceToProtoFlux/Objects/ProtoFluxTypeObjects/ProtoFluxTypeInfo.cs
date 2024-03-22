using System;
using System.Collections.Generic;

namespace VoiceToProtoFlux.Objects.ProtoFluxTypeObjects
{
    public class ProtoFluxTypeInfo
    {
        public string FullName { get; set; }
        public string NiceName { get; set; }
        public string FullCategory { get; set; }
        public string NiceCategory { get; set; }
        public int ParameterCount { get; set; }
        public List<string> WordsOfNiceName { get; set; }
        public List<string> Phrases { get; set; }

        public ProtoFluxTypeInfo(string fullName, string niceName, string fullCategory, string niceCategory, int parameterCount, List<string> wordsOfNiceName)
        {
            FullName = fullName;
            NiceName = niceName;
            FullCategory = fullCategory;
            NiceCategory = niceCategory;
            ParameterCount = parameterCount;
            WordsOfNiceName = wordsOfNiceName;

            // We want the phrases to be single compound-words so they'll get picked up by the transcriber.
            Phrases = new List<string> { string.Join("", WordsOfNiceName) };
            // Generate additional phrases using synonyms
            GenerateAdditionalPhrases();
        }

        private void GenerateAdditionalPhrases()
        {
            var baseWords = WordsOfNiceName;

            // Iterate over each word to find synonyms and generate phrases
            foreach (var word in baseWords)
            {
                if (SynonymManager.HasSynonyms(word))
                {
                    foreach (var synonym in SynonymManager.GetSynonyms(word))
                    {
                        var newPhrase = new List<string>(baseWords);
                        newPhrase[newPhrase.IndexOf(word)] = synonym; // Replace word with its synonym
                        Phrases.Add(string.Join("", newPhrase));
                    }
                }
            }
            return;
        }
    }
}
