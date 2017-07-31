using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.exceptions;
using tvn.cosine.ai.logic.propositional.inference;
using tvn.cosine.ai.logic.propositional.kb;
using tvn.cosine.ai.logic.propositional.parsing;
using tvn.cosine.ai.logic.propositional.parsing.ast;

namespace tvn_cosine.ai.test.unit.logic.propositional.inference
{
    [TestClass]
    public class PLFCEntailsTest
    {
        private PLParser parser;
        private PLFCEntails plfce;

        [TestInitialize]
        public void setUp()
        {
            parser = new PLParser();
            plfce = new PLFCEntails();
        }

        [TestMethod]
        public void testAIMAExample()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("P => Q");
            kb.tell("L & M => P");
            kb.tell("B & L => M");
            kb.tell("A & P => L");
            kb.tell("A & B => L");
            kb.tell("A");
            kb.tell("B");
            PropositionSymbol q = (PropositionSymbol)parser.parse("Q");

            Assert.AreEqual(true, plfce.plfcEntails(kb, q));
        }


        [TestMethod]
        [ExpectedException(typeof(IllegalArgumentException))]
        public void testKBWithNonDefiniteClauses()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("P => Q");
            kb.tell("L & M => P");
            kb.tell("B & L => M");
            kb.tell("~A & P => L"); // Not a definite clause
            kb.tell("A & B => L");
            kb.tell("A");
            kb.tell("B");
            PropositionSymbol q = (PropositionSymbol)parser.parse("Q");

            Assert.AreEqual(true, plfce.plfcEntails(kb, q));
        }
    }
}
