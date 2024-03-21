using System;
using System.Collections.Generic;

namespace VoiceToProtoFlux
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
            
            
            string defaultPhrase = string.Join("", WordsOfNiceName);
            Phrases = new List<string>();
            Phrases.Add(defaultPhrase);

        }
    }
}
