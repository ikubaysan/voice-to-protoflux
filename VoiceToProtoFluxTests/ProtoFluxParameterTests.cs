using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoiceToProtoFlux.Objects.ProtoFluxParameterObjects;
using System.Collections.Generic;

namespace VoiceToProtoFluxTests
{
    [TestClass]
    public class ProtoFluxParameterTests
    {
        [TestMethod]
        public void Constructor_SetsNameAndInitialPhrase()
        {
            // Arrange
            var paramName = "TestParameter";
            var parameter = new ProtoFluxParameter(paramName);

            // Assert
            Assert.AreEqual(paramName, parameter.Name, "Name property should be set by constructor.");
            Assert.IsTrue(parameter.Phrases.Contains(paramName), "Phrases should contain the parameter name.");
        }

        [TestMethod]
        public void GenerateAdditionalPhrases_AddsSynonymsToPhrases()
        {
            // Arrange
            var paramName = "uint";
            var parameter = new ProtoFluxParameter(paramName);


            // Assert
            Assert.IsTrue(parameter.Phrases.Contains("UnsignedInt"), "Phrases should include synonyms.");
            Assert.IsTrue(parameter.Phrases.Contains("UnsignedInteger"), "Phrases should include synonyms.");
        }

        [TestMethod]
        public void Phrases_DoesNotDuplicateInitialName()
        {
            // Arrange
            var paramName = "uint";
            var parameter = new ProtoFluxParameter(paramName);


            // Assert
            var nameOccurrences = parameter.Phrases.FindAll(phrase => phrase == paramName).Count;
            Assert.AreEqual(1, nameOccurrences, "Initial name should only appear once in Phrases.");
        }
    }
}
