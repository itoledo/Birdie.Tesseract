using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.nlp.parsing.grammars;

namespace tvn_cosine.ai.test.unit.nlp.parse
{
    /**
     * Test the class representing the Chomsky Normal Form grammar
     * @author Jonathon
     *
     */
    [TestClass]
    public class ProbCNFGrammarTest
    {

        ProbCNFGrammar gEmpty;
        Rule validR; Rule invalidR;

        [TestInitialize]
        public void setUp()
        {
            gEmpty = new ProbCNFGrammar();
            validR = new Rule(CollectionFactory.CreateQueue<string>(new[] { "A" }),
                    CollectionFactory.CreateQueue<string>(new[] { "Y", "X" }), (float)0.50);
            invalidR = new Rule(CollectionFactory.CreateQueue<string>(new[] { "A" }),
                      CollectionFactory.CreateQueue<string>(new[] { "Y", "X", "Z" }), (float)0.50); // too many RHS variables
        }

        [TestMethod]
        public void testAddValidRule()
        {
            Assert.IsTrue(gEmpty.addRule(validR));
        }

        [TestMethod]
        public void testAddInvalidRule()
        {
            Assert.IsFalse(gEmpty.addRule(invalidR));
        }

        [TestMethod]
        public void testValidRule()
        {
            Assert.IsTrue(gEmpty.validRule(validR));
            Assert.IsFalse(gEmpty.validRule(invalidR));
        } 
    } 
}
