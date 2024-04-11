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
        public List<string> WordsOfNiceNameLowerCased { get; set; }

        public ProtoFluxTypeInfo(string fullName, string niceName, string niceCategory, string nicePath, int parameterCount, List<string> wordsOfNiceName)
        {
            FullName = fullName;
            NiceName = niceName;
            NiceCategory = niceCategory;
            NicePath = nicePath;
            NiceCategory = niceCategory;
            ParameterCount = parameterCount;
            WordsOfNiceName = wordsOfNiceName;
            WordsOfNiceNameLowerCased = new List<string>();
            foreach (var word in wordsOfNiceName)
            {
                WordsOfNiceNameLowerCased.Add(word.ToLower());
            }

            // If Phrases contains "Object", and a parameter is required, then the type requires an object parameter
            RequiresObjectParameter = NiceName.Contains("Object") && parameterCount > 0;
          
        }
    }
}
