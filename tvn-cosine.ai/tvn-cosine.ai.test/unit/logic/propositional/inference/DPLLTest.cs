using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.logic.propositional.inference;
using tvn.cosine.ai.logic.propositional.kb;
using tvn.cosine.ai.logic.propositional.kb.data;
using tvn.cosine.ai.logic.propositional.parsing;
using tvn.cosine.ai.logic.propositional.parsing.ast;
using tvn.cosine.ai.logic.propositional.visitors;

namespace tvn_cosine.ai.test.unit.logic.propositional.inference
{
    [TestClass]
    public class DPLLTest
    {
        private DPLL dpll;
        private PLParser parser;
         
        public DPLLTest()
        {
            this.dpll = new DPLLSatisfiable();
            this.parser = new PLParser();
        }

        [TestMethod]
        public void testDPLLReturnsTrueWhenAllClausesTrueInModel()
        {
            Model model = new Model();
            model = model.union(new PropositionSymbol("A"), true).union(
                    new PropositionSymbol("B"), true);
            Sentence sentence = parser.parse("A & B & (A | B)");
            ISet<Clause> clauses = ConvertToConjunctionOfClauses.convert(sentence)
                    .getClauses();
            ICollection<PropositionSymbol> symbols = CollectionFactory.CreateQueue<PropositionSymbol>(
                       SymbolCollector.getSymbolsFrom(sentence));

            bool satisfiable = dpll.dpll(clauses, symbols, model);
            Assert.AreEqual(true, satisfiable);
        }

        [TestMethod]
        public void testDPLLReturnsFalseWhenOneClauseFalseInModel()
        {
            Model model = new Model();
            model = model.union(new PropositionSymbol("A"), true).union(
                    new PropositionSymbol("B"), false);
            Sentence sentence = parser.parse("(A | B) & (A => B)");
            ISet<Clause> clauses = ConvertToConjunctionOfClauses.convert(sentence)
                    .getClauses();
            ICollection<PropositionSymbol> symbols = CollectionFactory.CreateQueue<PropositionSymbol>(
                       SymbolCollector.getSymbolsFrom(sentence));

            bool satisfiable = dpll.dpll(clauses, symbols, model);
            Assert.AreEqual(false, satisfiable);
        }

        [TestMethod]
        public void testDPLLSucceedsWithAandNotA()
        {
            Sentence sentence = parser.parse("A & ~A");
            bool satisfiable = dpll.dpllSatisfiable(sentence);
            Assert.AreEqual(false, satisfiable);
        }

        [TestMethod]
        public void testDPLLSucceedsWithChadCarffsBugReport()
        {
            KnowledgeBase kb = new KnowledgeBase();

            kb.tell("B12 <=> P11 | P13 | P22 | P02");
            kb.tell("B21 <=> P20 | P22 | P31 | P11");
            kb.tell("B01 <=> P00 | P02 | P11");
            kb.tell("B10 <=> P11 | P20 | P00");
            kb.tell("~B21");
            kb.tell("~B12");
            kb.tell("B10");
            kb.tell("B01");

            Assert.IsTrue(dpll.isEntailed(kb, parser.parse("P00")));
            Assert.IsFalse(dpll.isEntailed(kb, parser.parse("~P00")));
        }

        [TestMethod]
        public void testDPLLSucceedsWithStackOverflowBugReport1()
        {
            Sentence sentence = (Sentence)parser.parse("(A | ~A) & (A | B)");
            Assert.IsTrue(dpll.dpllSatisfiable(sentence));
        }

        [TestMethod]
        public void testDPLLSucceedsWithChadCarffsBugReport2()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("B10 <=> P11 | P20 | P00");
            kb.tell("B01 <=> P00 | P02 | P11");
            kb.tell("B21 <=> P20 | P22 | P31 | P11");
            kb.tell("B12 <=> P11 | P13 | P22 | P02");
            kb.tell("~B21");
            kb.tell("~B12");
            kb.tell("B10");
            kb.tell("B01");

            Assert.IsTrue(dpll.isEntailed(kb, parser.parse("P00")));
            Assert.IsFalse(dpll.isEntailed(kb, parser.parse("~P00")));
        }

        [TestMethod]
        public void testIssue66()
        {
            // http://code.google.com/p/aima-java/issues/detail?id=66
            Model model = new Model();
            model = model.union(new PropositionSymbol("A"), false)
                    .union(new PropositionSymbol("B"), false)
                    .union(new PropositionSymbol("C"), true);
            Sentence sentence = parser.parse("((A | B) | C)");
            ISet<Clause> clauses = ConvertToConjunctionOfClauses.convert(sentence)
                    .getClauses();
            ICollection<PropositionSymbol> symbols = CollectionFactory.CreateQueue<PropositionSymbol>(
                       SymbolCollector.getSymbolsFrom(sentence));

            bool satisfiable = dpll.dpll(clauses, symbols, model);
            Assert.AreEqual(true, satisfiable);
        }

        [TestMethod]
        public void testDoesNotKnow()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("A");

            Assert.IsFalse(dpll.isEntailed(kb, parser.parse("B")));
            Assert.IsFalse(dpll.isEntailed(kb, parser.parse("~B")));
        }
    }

}
