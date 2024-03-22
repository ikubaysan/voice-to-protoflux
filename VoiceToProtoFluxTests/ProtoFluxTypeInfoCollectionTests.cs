using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceToProtoFluxTests
{
    [TestClass]
    public class ProtoFluxTypeInfoCollectionTests
    {
        [TestMethod]
        public void GetUniqueWordsOfNiceName_ReturnsUniqueWords()
        {
            // Arrange
            var collection = new VoiceToProtoFlux.Objects.ProtoFluxTypeObjects.ProtoFluxTypeInfoCollection();
            collection.AddTypeInfo(new VoiceToProtoFlux.Objects.ProtoFluxTypeObjects.ProtoFluxTypeInfo("FullName1", "NiceName1", "Category", "NiceCategory", 0, new List<string> { "Word1", "Word2" }));
            collection.AddTypeInfo(new VoiceToProtoFlux.Objects.ProtoFluxTypeObjects.ProtoFluxTypeInfo("FullName2", "NiceName2", "Category", "NiceCategory", 0, new List<string> { "Word2", "Word3" }));

            // Act
            var uniqueWords = collection.GetUniqueWordsOfNiceName();

            // Assert
            Assert.AreEqual(3, uniqueWords.Count, "Unique words count does not match");
            Assert.IsTrue(uniqueWords.Contains("Word1"), "Word1 is missing");
            Assert.IsTrue(uniqueWords.Contains("Word2"), "Word2 is missing");
            Assert.IsTrue(uniqueWords.Contains("Word3"), "Word3 is missing");
        }
    }

}
