using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceToProtoFlux.Objects.ProtoFluxTypeObjects;
using VoiceToProtoFlux.Objects.SpeechTranscriberObjects;




namespace VoiceToProtoFluxTests
{
    [TestClass]
    public class SpeechTranscriberGrammarTests
    {
        [TestMethod]
        public void Test_SpeechTranscriberGrammar_Populated_By_ProtoFluxTypeInfoCollection()
        {
            // Arrange: Create a collection and add some TypeInfo objects.
            ProtoFluxTypeInfoCollection collection = new ProtoFluxTypeInfoCollection();
            var typeInfoA = new ProtoFluxTypeInfo("FullNameA", "NiceNameA", "CategoryA", "NiceCategoryA", 0, new List<string> { "WordA0", "WordA1", "WordA2" });
            collection.AddTypeInfo(typeInfoA);
            var typeInfoB = new ProtoFluxTypeInfo("FullNameB", "NiceNameB", "CategoryB", "NiceCategoryB", 0, new List<string> { "WordB0", "WordB1", "WordB2" });
            collection.AddTypeInfo(typeInfoB);
            var typeInfoC = new ProtoFluxTypeInfo("FullNameC", "NiceNameC", "CategoryC", "NiceCategoryC", 0, new List<string> { "WordC0", "WordC1", "WordC2" });
            collection.AddTypeInfo(typeInfoC);

            HashSet<string> uniqueWordsSet = collection.GetUniqueWordsOfNiceNames();
            List<string> uniqueWords = new List<string>(uniqueWordsSet);

            SpeechTranscriberGrammar grammar = new SpeechTranscriberGrammar(uniqueWords);

            List<string> expectedWordsInGrammar = new List<string> { "WordA0", "WordA1", "WordA2", "WordB0", "WordB1", "WordB2", "WordC0", "WordC1", "WordC2" };

            // Act: Retrieve the unique set of WordsOfNiceName from the collection.
            List<string> actualWordsInGrammar = grammar.Phrases;

            // Assert: Verify the count and presence of expected unique words.
            Assert.AreEqual(expectedWordsInGrammar.Count, actualWordsInGrammar.Count, "Unique words count does not match.");
        }
    }
}
