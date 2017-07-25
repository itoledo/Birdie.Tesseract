using tvn.cosine.ai.common;
using tvn.cosine.ai.common.api;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.learning.reinforcement.agent;
using tvn.cosine.ai.learning.reinforcement.example;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.mdp.impl;

namespace tvn_cosine.ai.demo.learning.chapter21
{
    public class PassiveADPAgentDemo : LearningDemoBase
    {
        static void Main(params string[] args)
        {
            passiveADPAgentDemo();
        }

        static void passiveADPAgentDemo()
        {
            System.Console.WriteLine("=======================");
            System.Console.WriteLine("DEMO: Passive-ADP-Agent");
            System.Console.WriteLine("=======================");
            System.Console.WriteLine("Figure 21.3");
            System.Console.WriteLine("-----------");

            CellWorld<double> cw = CellWorldFactory.createCellWorldForFig17_1();
            CellWorldEnvironment cwe = new CellWorldEnvironment(
                    cw.getCellAt(1, 1),
                    cw.getCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    CommonFactory.CreateRandom());

            IMap<Cell<double>, CellWorldAction> fixedPolicy = CollectionFactory.CreateInsertionOrderedMap<Cell<double>, CellWorldAction>();
            fixedPolicy.Put(cw.getCellAt(1, 1), CellWorldAction.Up);
            fixedPolicy.Put(cw.getCellAt(1, 2), CellWorldAction.Up);
            fixedPolicy.Put(cw.getCellAt(1, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.getCellAt(2, 1), CellWorldAction.Left);
            fixedPolicy.Put(cw.getCellAt(2, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.getCellAt(3, 1), CellWorldAction.Left);
            fixedPolicy.Put(cw.getCellAt(3, 2), CellWorldAction.Up);
            fixedPolicy.Put(cw.getCellAt(3, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.getCellAt(4, 1), CellWorldAction.Left);

            PassiveADPAgent<Cell<double>, CellWorldAction> padpa = new PassiveADPAgent<Cell<double>, CellWorldAction>(
                    fixedPolicy, cw.getCells(), cw.getCellAt(1, 1),
                    MDPFactory.createActionsFunctionForFigure17_1(cw),
                    new ModifiedPolicyEvaluation<Cell<double>, CellWorldAction>(10, 1.0));

            cwe.AddAgent(padpa);

            output_utility_learning_rates(padpa, 20, 100, 100, 1);

            System.Console.WriteLine("=========================");
        }
    }
}
