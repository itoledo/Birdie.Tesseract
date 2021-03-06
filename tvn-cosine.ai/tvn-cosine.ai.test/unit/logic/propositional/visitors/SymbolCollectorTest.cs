﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.collections.api;
using tvn.cosine.ai.logic.propositional.parsing;
using tvn.cosine.ai.logic.propositional.parsing.ast;
using tvn.cosine.ai.logic.propositional.visitors;

namespace tvn_cosine.ai.test.unit.logic.propositional.visitors
{
    [TestClass] public class SymbolCollectorTest
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
            Assert.AreEqual(3, s.Size());
            Sentence b11 = parser.parse("B11");
            Sentence p21 = parser.parse("P21");
            Sentence p12 = parser.parse("P12");
            Assert.IsTrue(s.Contains(b11 as PropositionSymbol));
            Assert.IsTrue(s.Contains(p21 as PropositionSymbol));
            Assert.IsTrue(s.Contains(p12 as PropositionSymbol));
        }
    }
}
