using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.logic.fol;
using tvn.cosine.ai.logic.fol.domain;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn_cosine.ai.test.unit.logic.fol
{
    [TestClass]
    public class VariableCollectorTest
    {

        FOLParser parser;

        VariableCollector vc;

        [TestInitialize]
        public void setUp()
        {
            parser = new FOLParser(DomainFactory.crusadesDomain());
            vc = new VariableCollector();
        }

        [TestMethod]
        public void testSimplepredicate()
        {
            ISet<Variable> variables = vc.collectAllVariables(parser.parse("King(x)"));
            Assert.AreEqual(1, variables.Size());
            Assert.IsTrue(variables.Contains(new Variable("x")));
        }

        [TestMethod]
        public void testMultipleVariables()
        {
            ISet<Variable> variables = vc.collectAllVariables(parser.parse("BrotherOf(x) = EnemyOf(y)"));
            Assert.AreEqual(2, variables.Size());
            Assert.IsTrue(variables.Contains(new Variable("x")));
            Assert.IsTrue(variables.Contains(new Variable("y")));
        }

        [TestMethod]
        public void testQuantifiedVariables()
        {
            // Note: Should collect quantified variables
            // even if not mentioned in clause.
            ISet<Variable> variables = vc.collectAllVariables(parser.parse("FORALL x,y,z (BrotherOf(x) = EnemyOf(y))"));
            Assert.AreEqual(3, variables.Size());
            Assert.IsTrue(variables.Contains(new Variable("x")));
            Assert.IsTrue(variables.Contains(new Variable("y")));
            Assert.IsTrue(variables.Contains(new Variable("z")));
        }
    }

}
