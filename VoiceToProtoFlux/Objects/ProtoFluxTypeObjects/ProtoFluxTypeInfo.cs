using System;
using System.Collections.Generic;

namespace VoiceToProtoFlux.Objects.ProtoFluxTypeObjects
{
    public class ProtoFluxTypeInfo
    {
        public string FullName { get; set; }
        public string NiceName { get; set; }
        public string NicePath { get; set; }
        public string NiceCategory { get; set; }
        public int ParameterCount { get; set; }
        public bool RequiresObjectParameter { get; set; }
        public List<string> WordsOfNiceName { get; set; }
        public List<string> Phrases { get; set; }

        public ProtoFluxTypeInfo(string fullName, string niceName, string niceCategory, string nicePath, int parameterCount, List<string> wordsOfNiceName)
        {
            FullName = fullName;
            NiceName = niceName;
            NiceCategory = niceCategory;
            NicePath = nicePath;
            NiceCategory = niceCategory;
            ParameterCount = parameterCount;
            WordsOfNiceName = wordsOfNiceName;

            // We want the phrases to be single compound-words so they'll get picked up by the transcriber.
            Phrases = new List<string> { string.Join("", WordsOfNiceName) };

            // If Phrases contains "Object", then the type requires an object parameter
            RequiresObjectParameter = NiceName.Contains("Object");
            
            // Generate additional phrases using synonyms
            GenerateAdditionalPhrases();
            if (parameterCount > 0) GenerateParameterBasedPhrases();
        }

        private void GenerateParameterBasedPhrases()
        {
            var basePhrase = string.Join("", WordsOfNiceName);
            foreach (var parameter in ProtoFluxParameterCollection.Instance.Parameters.Values)
            {
                foreach (var phrase in parameter.Phrases)
                {
                    // If basePhrase ends with OfType, don't add another OfType
                    // Eg "ClearDynamicVariablesOfType" should not become "ClearDynamicVariablesOfTypeOfTypeInt",
                    // it should be "ClearDynamicVariablesOfTypeInt"
                    if (basePhrase.EndsWith("OfType") && phrase.StartsWith("OfType"))
                    {
                        Phrases.Add(basePhrase + phrase);
                    }
                    else
                    {
                        Phrases.Add(basePhrase + "OfType" + phrase);
                    }
                }
            }
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
