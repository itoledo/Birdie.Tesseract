using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using tvn.cosine.ai.logic.propositional.parsing;
using tvn.cosine.ai.logic.propositional.parsing.ast;
using tvn.cosine.ai.logic.propositional.visitors;

namespace tvn_cosine.ai.test.logic.propositional.visitors
{
    [TestClass]
    public class SymbolCollectorTest
    {
        private PLParser parser;

        [TestInitialize]
        public void setUp()
        {
            parser = new PLParser();
        }

        [TestMethod]
        public void testCollectSymbolsFromComplexSentence()
        {
            Sentence sentence = (Sentence)parser.parse("(~B11 | P12 | P21) & (B11 | ~P12) & (B11 | ~P21)");
            ISet<PropositionSymbol> s = SymbolCollector.getSymbolsFrom(sentence);
            Assert.AreEqual(3, s.Count);
            Sentence b11 = parser.parse("B11");
            Sentence p21 = parser.parse("P21");
            Sentence p12 = parser.parse("P12");
            Assert.IsTrue(s.Contains(b11));
            Assert.IsTrue(s.Contains(p21));
            Assert.IsTrue(s.Contains(p12));
        }
    }
}
