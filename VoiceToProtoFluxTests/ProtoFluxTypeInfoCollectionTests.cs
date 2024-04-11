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


        [TestMethod]
        public void FindBestMatchingTypeInfoByWords_HandlesMixedCasingAndNonAlphanumeric()
        {
            // Arrange
            var collection = new ProtoFluxTypeInfoCollection();
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName1", "NiceName1", "Category1", "NiceCategory1", 0, new List<string> { "word1" }));
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName2", "NiceName2", "Category2", "NiceCategory2", 0, new List<string> { "word2" }));

            // Act
            var bestMatch = collection.FindBestMatchingTypeInfoByWords(new List<string> { "Word1!" });

            // Assert
            Assert.AreEqual(1, bestMatch.Count, "Should return a single best match when one exists, handling mixed casing and non-alphanumeric characters.");
            Assert.AreEqual("FullName1", bestMatch[0].FullName, "The FullName1 type info should be the best and only match.");
        }

        [TestMethod]
        public void FindBestMatchingTypeInfoByWords_PreprocessingEnsuresCorrectMatching()
        {
            // Arrange
            var collection = new ProtoFluxTypeInfoCollection();
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName1", "NiceName1", "Category1", "NiceCategory1", 0, new List<string> { "preprocess" }));
            collection.AddTypeInfo(new ProtoFluxTypeInfo("FullName2", "NiceName2", "Category2", "NiceCategory2", 0, new List<string> { "preprocessed" }));

            // Words with and without the non-alphanumeric characters
            var wordsWithNonAlphaNumeric = new List<string> { "preprocess!" };
            var wordsWithoutNonAlphaNumeric = new List<string> { "preprocess" };

            // Act
            var bestMatchWith = collection.FindBestMatchingTypeInfoByWords(wordsWithNonAlphaNumeric);
            var bestMatchWithout = collection.FindBestMatchingTypeInfoByWords(wordsWithoutNonAlphaNumeric);

            // Assert
            Assert.AreEqual(1, bestMatchWith.Count, "Preprocessing should allow matching words with trailing non-alphanumeric characters.");
            Assert.AreEqual(1, bestMatchWithout.Count, "Should return the same result regardless of trailing non-alphanumeric characters.");
            Assert.AreEqual(bestMatchWith[0].FullName, bestMatchWithout[0].FullName, "Both preprocessing scenarios should match the same type info.");
        }




    }
}
