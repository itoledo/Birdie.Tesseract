using Microsoft.VisualStudio.TestTools.UnitTesting;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.mdp;
using tvn.cosine.ai.probability.mdp.api; 
using tvn.cosine.ai.probability.mdp.search;

namespace tvn_cosine.ai.test.unit.probability.mdp
{
    [TestClass] public class PolicyIterationTest
    {
        private CellWorld<double> cw = null;
        private IMarkovDecisionProcess<Cell<double>, CellWorldAction> mdp = null;
        private PolicyIteration<Cell<double>, CellWorldAction> pi = null;

        [TestInitialize]
        public void setUp()
        {
            cw = CellWorldFactory.createCellWorldForFig17_1();
            mdp = MDPFactory.createMDPForFigure17_3(cw);
            pi = new PolicyIteration<Cell<double>, CellWorldAction>(
                    new ModifiedPolicyEvaluation<Cell<double>, CellWorldAction>(50, 1.0));
        }

        [TestMethod]
        public void testPolicyIterationForFig17_2()
        {

            // AIMA3e check with Figure 17.2 (a)
            IPolicy<Cell<double>, CellWorldAction> policy = pi.policyIteration(mdp);

            Assert.AreEqual(CellWorldAction.Up,
                    policy.action(cw.getCellAt(1, 1)));
            Assert.AreEqual(CellWorldAction.Up,
                    policy.action(cw.getCellAt(1, 2)));
            Assert.AreEqual(CellWorldAction.Right,
                    policy.action(cw.getCellAt(1, 3)));

            Assert.AreEqual(CellWorldAction.Left,
                    policy.action(cw.getCellAt(2, 1)));
            Assert.AreEqual(CellWorldAction.Right,
                    policy.action(cw.getCellAt(2, 3)));

            Assert.AreEqual(CellWorldAction.Left,
                    policy.action(cw.getCellAt(3, 1)));
            Assert.AreEqual(CellWorldAction.Up,
                    policy.action(cw.getCellAt(3, 2)));
            Assert.AreEqual(CellWorldAction.Right,
                    policy.action(cw.getCellAt(3, 3)));

            Assert.AreEqual(CellWorldAction.Left,
                    policy.action(cw.getCellAt(4, 1)));
            Assert.IsNull(policy.action(cw.getCellAt(4, 2)));
            Assert.IsNull(policy.action(cw.getCellAt(4, 3)));
        }
    }

}
