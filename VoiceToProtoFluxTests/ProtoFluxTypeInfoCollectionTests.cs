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
        public void GetUniqueWordsOfNiceNames_ReturnsUniqueWords()
        {
            // Arrange: Create a collection and add two ProtoFluxTypeInfo objects with overlapping WordsOfNiceName.
            var collection = new ProtoFluxTypeInfoCollection();
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName1", "NiceName1", "Category", "NiceCategory", 0, new List<string> { "Word1", "Word2" }));
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName2", "NiceName2", "Category", "NiceCategory", 0, new List<string> { "Word2", "Word3" }));

            // Act: Retrieve the unique set of WordsOfNiceName from the collection.
            var uniqueWords = collection.GetUniqueWordsOfNiceNames();

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


        [TestMethod]
        public void FindBestMatchingTypeInfoByWords_ReturnsAllBestMatches()
        {
            // Arrange: Create a collection with ProtoFluxTypeInfo objects having different WordsOfNiceName
            var collection = new ProtoFluxTypeInfoCollection();
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName1", "NiceName1", "Category1", "NiceCategory1", 0, new List<string> { "Word1", "Word2" }));
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName2", "NiceName2", "Category2", "NiceCategory2", 0, new List<string> { "Word2", "Word3" }));
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName3", "NiceName3", "Category3", "NiceCategory3", 0, new List<string> { "Word1", "Word3" }));

            // Act: Retrieve best matching type infos for a given list of words
            var bestMatches = collection.FindBestMatchingTypeInfoByWords(new List<string> { "Word1", "Word4" });

            // Assert: Ensure the collection returns all type infos with the highest match count
            Assert.AreEqual(2, bestMatches.Count, "Should return two best matching type infos.");
            Assert.IsTrue(bestMatches.Any(ti => ti.FullName == "FullName1"), "FullName1 should be one of the best matches.");
            Assert.IsTrue(bestMatches.Any(ti => ti.FullName == "FullName3"), "FullName3 should be one of the best matches.");
        }

        [TestMethod]
        public void FindBestMatchingTypeInfoByWords_ReturnsEmptyListWhenNoMatches()
        {
            // Arrange: Create a collection with a few ProtoFluxTypeInfo objects
            var collection = new ProtoFluxTypeInfoCollection();
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName1", "NiceName1", "Category1", "NiceCategory1", 0, new List<string> { "Word1" }));
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName2", "NiceName2", "Category2", "NiceCategory2", 0, new List<string> { "Word2" }));

            // Act: Try to find type infos with a word that doesn't exist in any of the type infos
            var bestMatches = collection.FindBestMatchingTypeInfoByWords(new List<string> { "NonExistentWord" });

            // Assert: Ensure the method returns an empty list when no matches are found
            Assert.AreEqual(0, bestMatches.Count, "Should return an empty list when no matches are found.");
        }

        [TestMethod]
        public void FindBestMatchingTypeInfoByWords_ReturnsSingleBestMatchWhenOneExists()
        {
            // Arrange
            var collection = new ProtoFluxTypeInfoCollection();
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName1", "NiceName1", "Category1", "NiceCategory1", 0, new List<string> { "UniqueWord1", "Word2" }));
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName2", "NiceName2", "Category2", "NiceCategory2", 0, new List<string> { "Word2", "Word3" }));

            // Act
            var bestMatch = collection.FindBestMatchingTypeInfoByWords(new List<string> { "UniqueWord1" });

            // Assert
            Assert.AreEqual(1, bestMatch.Count, "Should return a single best match when one exists.");
            Assert.AreEqual("FullName1", bestMatch[0].FullName, "The FullName1 type info should be the best and only match.");
        }



    }
}
