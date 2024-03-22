using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceToProtoFluxTests
{
    [TestClass]
    public class SynonymManagerTests
    {
        [TestMethod]
        public void GetSynonyms_ReturnsCorrectSynonyms()
        {
            // Arrange
            var synonymManager = new VoiceToProtoFlux.Objects.SynonymManager();

            // Act & Assert
            var synonymsForUlong = synonymManager.GetSynonyms("ulong");
            Assert.IsTrue(synonymsForUlong.Contains("UnsignedLong"), "UnsignedLong synonym for ulong is missing");

            var synonymsForUint = synonymManager.GetSynonyms("uint");
            Assert.IsTrue(synonymsForUint.Contains("UnsignedInt") && synonymsForUint.Contains("UnsignedInteger"), "Synonyms for uint are missing or incorrect");
        }
    }

}
