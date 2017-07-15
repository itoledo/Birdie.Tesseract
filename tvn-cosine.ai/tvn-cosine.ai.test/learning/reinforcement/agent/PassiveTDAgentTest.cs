using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.learning.reinforcement.agent;
using tvn.cosine.ai.learning.reinforcement.example;
using tvn.cosine.ai.probability.example;

namespace tvn_cosine.ai.test.learning.reinforcement.agent
{
    [TestClass]
    public class PassiveTDAgentTest : ReinforcementLearningAgentTest
    {
        //
        private CellWorld<double> cw = null;
        private CellWorldEnvironment cwe = null;
        private PassiveTDAgent<Cell<double>, CellWorldAction> ptda = null;

        [TestInitialize]
        public void setUp()
        {
            cw = CellWorldFactory.CreateCellWorldForFig17_1();
            cwe = new CellWorldEnvironment(
                    cw.getCellAt(1, 1),
                    cw.GetCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    new Random());

            IDictionary<Cell<double>, CellWorldAction> fixedPolicy = new Dictionary<Cell<double>, CellWorldAction>();
            fixedPolicy[cw.getCellAt(1, 2)] = CellWorldAction.Up;
            fixedPolicy[cw.getCellAt(1, 1)] = CellWorldAction.Up;
            fixedPolicy[cw.getCellAt(1, 3)] = CellWorldAction.Right;
            fixedPolicy[cw.getCellAt(2, 1)] = CellWorldAction.Left;
            fixedPolicy[cw.getCellAt(2, 3)] = CellWorldAction.Right;
            fixedPolicy[cw.getCellAt(3, 1)] = CellWorldAction.Left;
            fixedPolicy[cw.getCellAt(3, 2)] = CellWorldAction.Up;
            fixedPolicy[cw.getCellAt(3, 3)] = CellWorldAction.Right;
            fixedPolicy[cw.getCellAt(4, 1)] = CellWorldAction.Left;

            ptda = new PassiveTDAgent<Cell<double>, CellWorldAction>(fixedPolicy,
                    0.2, 1.0);

            cwe.addAgent(ptda);
        }

        [TestMethod]
        public void test_TD_learning_fig21_1()
        {

            ptda.reset();
            cwe.executeTrials(10000);

            IDictionary<Cell<double>, double> U = ptda.getUtility();

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
            test_RMSeiu_for_1_1(ptda, 20, 1000, 0.07);
        }

        // Note: Enable this test if you wish to generate tables for
        // creating figures, in a spreadsheet, of the learning
        // rate of the agent.
        [Ignore]
        [TestMethod]
        public void test_TD_learning_rate_fig21_5()
        {
            test_utility_learning_rates(ptda, 20, 500, 100, 1);
        }
    }
}
