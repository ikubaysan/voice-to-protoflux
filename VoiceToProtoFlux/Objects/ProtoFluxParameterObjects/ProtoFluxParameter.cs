using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceToProtoFlux.Objects.ProtoFluxParameterObjects
{
    public class ProtoFluxParameter
    {
        public string Name { get; set; }
        public List<string> Phrases { get; set; }


        public ProtoFluxParameter(string name)
        {
            Name = name;
            Phrases = new List<string> { name };
            GenerateAdditionalPhrases();
        }

        private void GenerateAdditionalPhrases()
        {
            if (SynonymManager.HasSynonyms(Name))
            {
                Phrases.AddRange(SynonymManager.GetSynonyms(Name));
            }
        }
    }
}
