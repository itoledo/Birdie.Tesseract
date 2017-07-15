using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.logic.fol;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn_cosine.ai.test.learning.fol
{
    [TestClass]
    public class SubstVisitorTest
    {

        private FOLParser parser;
        private SubstVisitor sv;

        [TestInitialize]
        public void setUp()
        {
            parser = new FOLParser(DomainFactory.crusadesDomain());
            sv = new SubstVisitor();
        }

        [TestMethod]
        public void testSubstSingleVariableSucceedsWithPredicate()
        {
            Sentence beforeSubst = parser.parse("King(x)");
            Sentence expectedAfterSubst = parser.parse(" King(John) ");
            Sentence expectedAfterSubstCopy = expectedAfterSubst.copy();

            Assert.AreEqual(expectedAfterSubst, expectedAfterSubstCopy);
            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("x"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(beforeSubst, parser.parse("King(x)"));
        }

        [TestMethod]
        public void testSubstSingleVariableFailsWithPredicate()
        {
            Sentence beforeSubst = parser.parse("King(x)");
            Sentence expectedAfterSubst = parser.parse(" King(x) ");

            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("y"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(beforeSubst, parser.parse("King(x)"));
        }

        [TestMethod]
        public void testMultipleVariableSubstitutionWithPredicate()
        {
            Sentence beforeSubst = parser.parse("King(x,y)");
            Sentence expectedAfterSubst = parser.parse(" King(John ,England) ");

            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("x"), new Constant("John"));
            p.Add(new Variable("y"), new Constant("England"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(beforeSubst, parser.parse("King(x,y)"));
        }

        [TestMethod]
        public void testMultipleVariablePartiallySucceedsWithPredicate()
        {
            Sentence beforeSubst = parser.parse("King(x,y)");
            Sentence expectedAfterSubst = parser.parse(" King(John ,y) ");

            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("x"), new Constant("John"));
            p.Add(new Variable("z"), new Constant("England"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(beforeSubst, parser.parse("King(x,y)"));
        }

        [TestMethod]
        public void testSubstSingleVariableSucceedsWithTermEquality()
        {
            Sentence beforeSubst = parser.parse("BrotherOf(x) = EnemyOf(y)");
            Sentence expectedAfterSubst = parser
                    .parse("BrotherOf(John) = EnemyOf(Saladin)");

            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("x"), new Constant("John"));
            p.Add(new Variable("y"), new Constant("Saladin"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(beforeSubst,
                    parser.parse("BrotherOf(x) = EnemyOf(y)"));
        }

        [TestMethod]
        public void testSubstSingleVariableSucceedsWithTermEquality2()
        {
            Sentence beforeSubst = parser.parse("BrotherOf(John) = x)");
            Sentence expectedAfterSubst = parser.parse("BrotherOf(John) = Richard");

            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("x"), new Constant("Richard"));
            p.Add(new Variable("y"), new Constant("Saladin"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("BrotherOf(John) = x)"), beforeSubst);
        }

        [TestMethod]
        public void testSubstWithUniversalQuantifierAndSngleVariable()
        {
            Sentence beforeSubst = parser.parse("FORALL x King(x))");
            Sentence expectedAfterSubst = parser.parse("King(John)");

            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("x"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("FORALL x King(x))"), beforeSubst);
        }

        [TestMethod]
        public void testSubstWithUniversalQuantifierAndZeroVariablesMatched()
        {
            Sentence beforeSubst = parser.parse("FORALL x King(x))");
            Sentence expectedAfterSubst = parser.parse("FORALL x King(x)");

            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("y"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("FORALL x King(x))"), beforeSubst);
        }

        [TestMethod]
        public void testSubstWithUniversalQuantifierAndOneOfTwoVariablesMatched()
        {
            Sentence beforeSubst = parser.parse("FORALL x,y King(x,y))");
            Sentence expectedAfterSubst = parser.parse("FORALL x King(x,John)");

            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("y"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("FORALL x,y King(x,y))"), beforeSubst);
        }

        [TestMethod]
        public void testSubstWithExistentialQuantifierAndSngleVariable()
        {
            Sentence beforeSubst = parser.parse("EXISTS x King(x))");
            Sentence expectedAfterSubst = parser.parse("King(John)");

            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("x"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);

            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("EXISTS x King(x)"), beforeSubst);
        }

        [TestMethod]
        public void testSubstWithNOTSentenceAndSngleVariable()
        {
            Sentence beforeSubst = parser.parse("NOT King(x))");
            Sentence expectedAfterSubst = parser.parse("NOT King(John)");

            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("x"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("NOT King(x))"), beforeSubst);
        }

        [TestMethod]
        public void testConnectiveANDSentenceAndSngleVariable()
        {
            Sentence beforeSubst = parser
                    .parse("EXISTS x ( King(x) AND BrotherOf(x) = EnemyOf(y) )");
            Sentence expectedAfterSubst = parser
                    .parse("( King(John) AND BrotherOf(John) = EnemyOf(Saladin) )");

            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("x"), new Constant("John"));
            p.Add(new Variable("y"), new Constant("Saladin"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser
                    .parse("EXISTS x ( King(x) AND BrotherOf(x) = EnemyOf(y) )"),
                    beforeSubst);
        }

        [TestMethod]
        public void testParanthisedSingleVariable()
        {
            Sentence beforeSubst = parser.parse("((( King(x))))");
            Sentence expectedAfterSubst = parser.parse("King(John) ");

            IDictionary<Variable, Term> p = new Dictionary<Variable, Term>();
            p.Add(new Variable("x"), new Constant("John"));

            Sentence afterSubst = sv.subst(p, beforeSubst);
            Assert.AreEqual(expectedAfterSubst, afterSubst);
            Assert.AreEqual(parser.parse("((( King(x))))"), beforeSubst);
        }
    }
}
