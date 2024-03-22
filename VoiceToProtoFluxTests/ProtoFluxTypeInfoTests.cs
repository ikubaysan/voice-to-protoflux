using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceToProtoFlux.Objects.ProtoFluxTypeObjects;




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

        [TestMethod]
        public void GenerateParameterBasedPhrases_CreatesExpectedPhrases()
        {
            // Arrange
            var wordsOfNiceName = new List<string> { "Test", "Node" };
            var protoFluxTypeInfo = new ProtoFluxTypeInfo("FullName", "NiceName", "Category", "NiceCategory", 1, wordsOfNiceName);

            // Act
            var phrases = protoFluxTypeInfo.Phrases;

            // Assert
            Assert.IsTrue(phrases.Contains("TestNodeInt"), "Phrase for Int parameter is missing");
            Assert.IsTrue(phrases.Contains("TestNodeUint"), "Phrase for Uint parameter is missing");

        }
    }
}
