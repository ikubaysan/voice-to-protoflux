using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceToProtoFlux.Objects;

namespace VoiceToProtoFluxTests
{
    [TestClass]
    public class SynonymManagerTests
    {
        [TestMethod]
        public void GetSynonyms_ReturnsCorrectSynonyms()
        {
            // Act & Assert
            var synonymsForUlong = SynonymManager.GetSynonyms("ulong");
            Assert.IsTrue(synonymsForUlong.Contains("UnsignedLong"), "UnsignedLong synonym for ulong is missing");

            var synonymsForUint = SynonymManager.GetSynonyms("uint");
            Assert.IsTrue(synonymsForUint.Contains("UnsignedInt") && synonymsForUint.Contains("UnsignedInteger"), "Synonyms for uint are missing or incorrect");
        }
    }

}
