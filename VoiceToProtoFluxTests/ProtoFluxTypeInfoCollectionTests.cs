using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoiceToProtoFlux.Objects.ProtoFluxTypeObjects;
using System.Collections.Generic;

namespace VoiceToProtoFluxTests
{
    [TestClass]
    public class ProtoFluxTypeInfoCollectionTests
    {
        /// <summary>
        /// Tests that GetUniqueWordsOfNiceName correctly returns a unique set of words
        /// from all ProtoFluxTypeInfo objects added to the collection.
        /// </summary>
        [TestMethod]
        public void GetUniqueWordsOfNiceName_ReturnsUniqueWords()
        {
            // Arrange: Create a collection and add two ProtoFluxTypeInfo objects with overlapping WordsOfNiceName.
            var collection = new ProtoFluxTypeInfoCollection();
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName1", "NiceName1", "Category", "NiceCategory", 0, new List<string> { "Word1", "Word2" }));
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName2", "NiceName2", "Category", "NiceCategory", 0, new List<string> { "Word2", "Word3" }));

            // Act: Retrieve the unique set of WordsOfNiceName from the collection.
            var uniqueWords = collection.GetUniqueWordsOfNiceName();

            // Assert: Verify the count and presence of expected unique words.
            Assert.AreEqual(3, uniqueWords.Count, "Unique words count does not match.");
            Assert.IsTrue(uniqueWords.Contains("Word1"), "Word1 is missing.");
            Assert.IsTrue(uniqueWords.Contains("Word2"), "Word2 is missing.");
            Assert.IsTrue(uniqueWords.Contains("Word3"), "Word3 is missing.");
        }

        /// <summary>
        /// Tests that adding a ProtoFluxTypeInfo object with a phrase to the collection
        /// and then retrieving it by that phrase returns the correct object.
        /// </summary>
        [TestMethod]
        public void Test_AddTypeInfo_And_GetTypeInfoByPhrase()
        {
            // Arrange: Create a collection and add a ProtoFluxTypeInfo object with a specific phrase.
            var collection = new ProtoFluxTypeInfoCollection();
            var typeInfo = new ProtoFluxTypeInfo("FullName", "NiceName", "Category", "NiceCategory", 0, new List<string> { "Test" });
            typeInfo.Phrases.Add("TestPhrase");
            collection.AddTypeInfo(typeInfo);

            // Act: Retrieve the ProtoFluxTypeInfo object by the phrase.
            var retrievedTypeInfo = collection.GetTypeInfoByPhrase("TestPhrase");

            // Assert: Verify the retrieved object matches the one added.
            Assert.IsNotNull(retrievedTypeInfo, "TypeInfo should not be null for existing phrase.");
            Assert.AreEqual(typeInfo.FullName, retrievedTypeInfo.FullName, "Retrieved TypeInfo does not match the added TypeInfo.");
        }

        /// <summary>
        /// Tests that attempting to retrieve a ProtoFluxTypeInfo object by a nonexistent phrase returns null.
        /// </summary>
        [TestMethod]
        public void Test_GetTypeInfoByPhrase_ReturnsNullForNonexistentPhrase()
        {
            // Arrange: Create a collection and add a ProtoFluxTypeInfo object with a specific phrase.
            var collection = new ProtoFluxTypeInfoCollection();
            var typeInfo = new ProtoFluxTypeInfo("FullName", "NiceName", "Category", "NiceCategory", 0, new List<string> { "Test" });
            typeInfo.Phrases.Add("TestPhrase");
            collection.AddTypeInfo(typeInfo);

            // Act: Attempt to retrieve a ProtoFluxTypeInfo object by a nonexistent phrase.
            var retrievedTypeInfo = collection.GetTypeInfoByPhrase("NonexistentPhrase");

            // Assert: Verify that null is returned.
            Assert.IsNull(retrievedTypeInfo, "TypeInfo should be null for nonexistent phrase.");
        }
    }
}
