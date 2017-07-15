using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.learning.reinforcement.agent;
using tvn.cosine.ai.learning.reinforcement.example;
using tvn.cosine.ai.probability.example;

namespace tvn_cosine.ai.test.learning.reinforcement.agent
{
    [TestClass]
    public class QLearningAgentTest : ReinforcementLearningAgentTest
    {
        //
        private CellWorld<double> cw = null;
        private CellWorldEnvironment cwe = null;
        private QLearningAgent<Cell<double>, CellWorldAction> qla = null;

        [TestInitialize]
        public void setUp()
        {
            cw = CellWorldFactory.CreateCellWorldForFig17_1();
            cwe = new CellWorldEnvironment(
                    cw.getCellAt(1, 1),
                    cw.GetCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    new Random());

            qla = new QLearningAgent<Cell<double>, CellWorldAction>(MDPFactory
                    .createActionsFunctionForFigure17_1(cw),
                    CellWorldAction.None, 0.2, 1.0, 5, 2.0);

            cwe.addAgent(qla);
        }

        [TestMethod]
        public void test_Q_learning()
        {

            qla.reset();
            cwe.executeTrials(100000);

            IDictionary<Cell<double>, double> U = qla.getUtility();

            Assert.IsNotNull(U[cw.getCellAt(1, 1)]);

            // Note:
            // As the Q-Learning Agent is not using a fixed
            // policy it should with a reasonable number
            // of iterations observe and calculate an
            // approximate utility for all of the states.
            Assert.AreEqual(11, U.Count);

            // Note: Due to stochastic nature of environment,
            // will not test the individual utilities calculated
            // as this will take a fair amount of time.
            // Instead we will check if the RMS error in utility
            // for 1,1 is below a reasonable threshold.
            test_RMSeiu_for_1_1(qla, 20, 10000, 0.2);
        }

        // Note: Enable this test if you wish to generate tables for
        // creating figures, in a spreadsheet, of the learning
        // rate of the agent.
        [Ignore]
        [TestMethod]

        public void test_Q_learning_rate()
        {
            test_utility_learning_rates(qla, 20, 10000, 500, 20);
        }
    }
}
