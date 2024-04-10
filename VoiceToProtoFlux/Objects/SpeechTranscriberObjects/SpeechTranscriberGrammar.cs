using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceToProtoFlux.Objects.SpeechTranscriberObjects
{
    public class SpeechTranscriberGrammar
    {
        public List<string> Phrases { get; set; }

        public SpeechTranscriberGrammar(List<string> phrases)
        {
            Phrases = phrases;
        }

        public void AddPhrase(string phrase)
        {
            // Add the phrase if it doesn't already exist
            if (!Phrases.Contains(phrase))
            {
                Phrases.Add(phrase);
            }
        }

    }
}
