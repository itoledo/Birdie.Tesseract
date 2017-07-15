using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.learning.reinforcement.agent;
using tvn.cosine.ai.learning.reinforcement.example;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.mdp.impl;

namespace tvn_cosine.ai.test.learning.reinforcement.agent
{
    [TestClass]
    public class PassiveADPAgentTest : ReinforcementLearningAgentTest
    {
        //
        private CellWorld<double> cw = null;
        private CellWorldEnvironment cwe = null;
        private PassiveADPAgent<Cell<double>, CellWorldAction> padpa = null;

        [TestInitialize]
        public void setUp()
        {
            cw = CellWorldFactory.CreateCellWorldForFig17_1();
            cwe = new CellWorldEnvironment(
                    cw.getCellAt(1, 1),
                    cw.GetCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    new System.Random());

            IDictionary<Cell<double>, CellWorldAction> fixedPolicy
                = new Dictionary<Cell<double>, CellWorldAction>();
            fixedPolicy[cw.getCellAt(1, 1)] = CellWorldAction.Up;
            fixedPolicy[cw.getCellAt(1, 2)] = CellWorldAction.Up;
            fixedPolicy[cw.getCellAt(1, 3)] = CellWorldAction.Right;
            fixedPolicy[cw.getCellAt(2, 1)] = CellWorldAction.Left;
            fixedPolicy[cw.getCellAt(2, 3)] = CellWorldAction.Right;
            fixedPolicy[cw.getCellAt(3, 1)] = CellWorldAction.Left;
            fixedPolicy[cw.getCellAt(3, 2)] = CellWorldAction.Up;
            fixedPolicy[cw.getCellAt(3, 3)] = CellWorldAction.Right;
            fixedPolicy[cw.getCellAt(4, 1)] = CellWorldAction.Left;

            padpa = new PassiveADPAgent<Cell<double>, CellWorldAction>(fixedPolicy,
                    cw.GetCells(), cw.getCellAt(1, 1), MDPFactory
                            .createActionsFunctionForFigure17_1(cw),
                    new ModifiedPolicyEvaluation<Cell<double>, CellWorldAction>(10,
                            1.0));

            cwe.addAgent(padpa);
        }

        [TestMethod]
        public void test_ADP_learning_fig21_1()
        {
            padpa.reset();
            cwe.executeTrials(2000);

            IDictionary<Cell<double>, double> U = padpa.getUtility();

            Assert.IsNotNull(U[cw.getCellAt(1, 1)]);

            // Note:
            // These are not reachable when starting at 1,1 using
            // the policy and default transition model
            // (i.e. 80% intended, 10% each right angle from intended).
            Assert.IsNull(U[cw.getCellAt(3, 1)]);
            Assert.IsNull(U[cw.getCellAt(4, 1)]);
            Assert.AreEqual(9, U.Count);

            // Note: Due to stochastic nature of environment,
            // will not test the individual utilities calculated
            // as this will take a fair amount of time.
            // Instead we will check if the RMS error in utility
            // for 1,1 is below a reasonable threshold.
            test_RMSeiu_for_1_1(padpa, 20, 100, 0.05);
        }

        // Note: Enable this test if you wish to generate tables for
        // creating figures, in a spreadsheet, of the learning
        // rate of the agent.
        [Ignore]
        [TestMethod] 
        public void test_ADP_learning_rate_fig21_3()
        {
            test_utility_learning_rates(padpa, 20, 100, 100, 1);
        }
    }
}
