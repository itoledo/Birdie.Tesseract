using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.logic.propositional.inference;
using tvn.cosine.ai.logic.propositional.kb;
using tvn.cosine.ai.logic.propositional.kb.data;
using tvn.cosine.ai.logic.propositional.parsing;
using tvn.cosine.ai.logic.propositional.parsing.ast;
using tvn.cosine.ai.logic.propositional.visitors;

namespace tvn_cosine.ai.test.logic.propositional.inference
{
    [TestClass]
    public class PLResolutionTest
    {
        private PLResolution resolution;
        private PLParser parser;

        public PLResolutionTest(bool discardTautologies)
        {
            this.resolution = new PLResolution(discardTautologies);
            parser = new PLParser();
        }

        [TestMethod]
        public void testPLResolveWithOneLiteralMatching()
        {
            Clause one = ConvertToConjunctionOfClauses
                    .convert(parser.parse("A | B")).getClauses().First();
            Clause two = ConvertToConjunctionOfClauses
                    .convert(parser.parse("~B | C")).getClauses().First();
            Clause expected = ConvertToConjunctionOfClauses
                    .convert(parser.parse("A | C")).getClauses().First();

            ISet<Clause> resolvents = resolution.plResolve(one, two);
            Assert.AreEqual(1, resolvents.Count);
            Assert.IsTrue(resolvents.Contains(expected));
        }

        [TestMethod]
        public void testPLResolveWithNoLiteralMatching()
        {
            Clause one = ConvertToConjunctionOfClauses
                    .convert(parser.parse("A | B")).getClauses().First();
            Clause two = ConvertToConjunctionOfClauses
                    .convert(parser.parse("C | D")).getClauses().First();

            ISet<Clause> resolvents = resolution.plResolve(one, two);
            Assert.AreEqual(0, resolvents.Count);
        }

        [TestMethod]
        public void testPLResolveWithOneLiteralSentencesMatching()
        {
            Clause one = ConvertToConjunctionOfClauses.convert(parser.parse("A"))
                    .getClauses().First();
            Clause two = ConvertToConjunctionOfClauses.convert(parser.parse("~A"))
                    .getClauses().First();

            ISet<Clause> resolvents = resolution.plResolve(one, two);
            Assert.AreEqual(1, resolvents.Count);
            Assert.IsTrue(resolvents.First().isEmpty());
            Assert.IsTrue(resolvents.First().isFalse());
        }

        [TestMethod]
        public void testPLResolveWithTwoLiteralsMatching()
        {
            Clause one = ConvertToConjunctionOfClauses
                    .convert(parser.parse("~P21 | B11")).getClauses().First();
            Clause two = ConvertToConjunctionOfClauses
                    .convert(parser.parse("~B11 | P21 | P12")).getClauses()
                    .First();
            ISet<Clause> expected = ConvertToConjunctionOfClauses.convert(
                    parser.parse("(P12 | P21 | ~P21) & (B11 | P12 | ~B11)"))
                    .getClauses();

            ISet<Clause> resolvents = resolution.plResolve(one, two);

            int numberExpectedResolvents = 2;
            if (resolution.isDiscardTautologies())
            {
                numberExpectedResolvents = 0; // due to being tautologies
            }
            Assert.AreEqual(numberExpectedResolvents, resolvents.Count);
            Assert.AreEqual(numberExpectedResolvents, expected.Intersect(resolvents).Count());
        }

        [TestMethod]
        public void testPLResolve1()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("(B11 => ~P11) & B11");
            Sentence alpha = parser.parse("P11");

            bool b = resolution.plResolution(kb, alpha);
            Assert.AreEqual(false, b);
        }

        [TestMethod]
        public void testPLResolve2()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("A & B");
            Sentence alpha = parser.parse("B");

            bool b = resolution.plResolution(kb, alpha);
            Assert.AreEqual(true, b);
        }

        [TestMethod]
        public void testPLResolve3()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("(B11 => ~P11) & B11");
            Sentence alpha = parser.parse("~P11");

            bool b = resolution.plResolution(kb, alpha);
            Assert.AreEqual(true, b);
        }

        [TestMethod]
        public void testPLResolve4()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("A | B");
            Sentence alpha = parser.parse("B");

            bool b = resolution.plResolution(kb, alpha);
            Assert.AreEqual(false, b);
        }

        [TestMethod]
        public void testPLResolve5()
        {
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("(B11 => ~P11) & B11");
            Sentence alpha = parser.parse("~B11");

            bool b = resolution.plResolution(kb, alpha);
            Assert.AreEqual(false, b);
        }

        [TestMethod]
        public void testPLResolve6()
        {
            KnowledgeBase kb = new KnowledgeBase();
            // e.g. from AIMA3e pg. 254
            kb.tell("(B11 <=> P12 | P21) & ~B11");
            Sentence alpha = parser.parse("~P21");

            bool b = resolution.plResolution(kb, alpha);
            Assert.AreEqual(true, b);
        }

        [TestMethod]
        public void testMultipleClauseResolution()
        {
            // test (and fix) suggested by Huy Dinh. Thanks Huy!
            KnowledgeBase kb = new KnowledgeBase();
            kb.tell("(B11 <=> P12 | P21) & ~B11");
            Sentence alpha = parser.parse("B");

            bool b = resolution.plResolution(kb, alpha);
            Assert.AreEqual(false, b); // false as KB says nothing about B
        }

        [TestMethod]
        public void testPLResolutionWithChadCarfBugReportData()
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

            Sentence alpha = parser.parse("P00");
            bool b = resolution.plResolution(kb, alpha);
            Assert.AreEqual(true, b);
        }

        [TestMethod]
        public void testPLResolutionSucceedsWithChadCarffsBugReport2()
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

            Sentence alpha = parser.parse("P00");
            bool b = resolution.plResolution(kb, alpha);
            Assert.AreEqual(true, b);
        }
    }
}
