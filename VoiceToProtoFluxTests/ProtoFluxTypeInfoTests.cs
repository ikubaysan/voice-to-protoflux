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
        public void Create_ProtoFluxTypeInfo()
        {
            // Arrange
            var wordsOfNiceName = new List<string> { "Test", "Node" };
            var protoFluxTypeInfo = new ProtoFluxTypeInfo("FullName", "NiceName", "Category", "NiceCategory", 1, wordsOfNiceName);
        }
    }
}
