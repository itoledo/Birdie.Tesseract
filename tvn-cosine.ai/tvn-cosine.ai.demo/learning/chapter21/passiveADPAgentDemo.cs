using tvn.cosine;
using tvn.cosine.api;
using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.learning.reinforcement.agent;
using tvn.cosine.ai.learning.reinforcement.example;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.mdp;

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

            CellWorld<double> cw = CellWorldFactory.CreateCellWorldForFig17_1();
            CellWorldEnvironment cwe = new CellWorldEnvironment(
                    cw.GetCellAt(1, 1),
                    cw.GetCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    CommonFactory.CreateRandom());

            IMap<Cell<double>, CellWorldAction> fixedPolicy = CollectionFactory.CreateInsertionOrderedMap<Cell<double>, CellWorldAction>();
            fixedPolicy.Put(cw.GetCellAt(1, 1), CellWorldAction.Up);
            fixedPolicy.Put(cw.GetCellAt(1, 2), CellWorldAction.Up);
            fixedPolicy.Put(cw.GetCellAt(1, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.GetCellAt(2, 1), CellWorldAction.Left);
            fixedPolicy.Put(cw.GetCellAt(2, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.GetCellAt(3, 1), CellWorldAction.Left);
            fixedPolicy.Put(cw.GetCellAt(3, 2), CellWorldAction.Up);
            fixedPolicy.Put(cw.GetCellAt(3, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.GetCellAt(4, 1), CellWorldAction.Left);

            PassiveADPAgent<Cell<double>, CellWorldAction> padpa = new PassiveADPAgent<Cell<double>, CellWorldAction>(
                    fixedPolicy, cw.GetCells(), cw.GetCellAt(1, 1),
                    MDPFactory.createActionsFunctionForFigure17_1(cw),
                    new ModifiedPolicyEvaluation<Cell<double>, CellWorldAction>(10, 1.0));

            cwe.AddAgent(padpa);

            output_utility_learning_rates(padpa, 20, 100, 100, 1);

            System.Console.WriteLine("=========================");
        }
    }
}
