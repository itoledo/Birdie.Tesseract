using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.nlp.data.grammars;
using tvn.cosine.ai.nlp.parsing;
using tvn.cosine.ai.nlp.parsing.grammars;

namespace tvn_cosine.ai.test.unit.nlp.parse
{
    [TestClass]
    public class CYKParseTest
    {

        CYK parser;
        ICollection<string> words1;
     //   IQueue<string> words2;
        ProbCNFGrammar trivGrammar = ProbCNFGrammarExamples.buildTrivialGrammar();
        // Get Example Grammar 2

        [TestInitialize]
        public void setUp()
        {
            parser = new CYK();
            words1 = CollectionFactory.CreateQueue<string>(new[] { "the", "man", "liked", "a", "woman" });

        } // end setUp()

        [TestMethod]
        public void testParseReturn()
        {
            float[,,] probTable = null;
            probTable = parser.parse(words1, trivGrammar);
            Assert.IsNotNull(probTable);
        }

        [TestMethod]
        public void testParse()
        {
            float[,,] probTable;
            probTable = parser.parse(words1, trivGrammar);
            Assert.IsTrue(probTable[5,0,4] > 0); // probTable[5,0,4] = [S][Start=0][Length=5] 
        }
    } 
}
