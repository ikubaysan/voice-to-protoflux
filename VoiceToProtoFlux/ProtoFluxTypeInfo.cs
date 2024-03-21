using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceToProtoFlux
{
    public class ProtoFluxTypeInfo
    {
        public string FullName { get; set; }
        public string NiceName { get; set; }
        public string FullCategory { get; set; }
        public string NiceCategory { get; set; }

        // Constructor
        // TODO: also determine individual words.
        public ProtoFluxTypeInfo(string fullName, string niceName, string fullCategory, string niceCategory)
        {
            FullName = fullName;
            NiceName = niceName;
            FullCategory = fullCategory;
            NiceCategory = niceCategory;
        }
    }
}
