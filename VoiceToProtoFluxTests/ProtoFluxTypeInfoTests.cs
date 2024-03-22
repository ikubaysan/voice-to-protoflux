using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace VoiceToProtoFluxTests
{
    [TestClass]
    public class ProtoFluxTypeInfoTests
    {
        [TestMethod]
        public void Phrases_WithSynonyms_GeneratesCorrectPhrases()
        {
            // Arrange
            var wordsOfNiceName = new List<string> { "ulong" };
            var protoFluxTypeInfo = new VoiceToProtoFlux.Objects.ProtoFluxTypeObjects.ProtoFluxTypeInfo(
                "Example.FullName", "ExampleNiceName", "ExampleCategory", "NiceCategory", 0, wordsOfNiceName);

            // Act
            var phrases = protoFluxTypeInfo.Phrases;

            // Assert
            Assert.IsTrue(phrases.Contains("ulong"), "Original phrase is missing");
            Assert.IsTrue(phrases.Contains("UnsignedLong"), "Synonym phrase is missing");
        }
    }

}
