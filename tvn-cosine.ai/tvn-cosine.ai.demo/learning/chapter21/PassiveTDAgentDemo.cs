using tvn.cosine.ai.common;
using tvn.cosine.ai.common.api;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.learning.reinforcement.agent;
using tvn.cosine.ai.learning.reinforcement.example;
using tvn.cosine.ai.probability.example;

namespace tvn_cosine.ai.demo.learning.chapter21
{
    public class PassiveTDAgentDemo : LearningDemoBase
    {
        static void Main(params string[] args)
        {
            System.Console.WriteLine("======================");
            System.Console.WriteLine("DEMO: Passive-TD-Agent");
            System.Console.WriteLine("======================");
            System.Console.WriteLine("Figure 21.5");
            System.Console.WriteLine("-----------");
            passiveTDAgentDemo();
            System.Console.WriteLine("=========================");
        }

        static void passiveTDAgentDemo()
        {
            CellWorld<double> cw = CellWorldFactory.createCellWorldForFig17_1();
            CellWorldEnvironment cwe = new CellWorldEnvironment(
                    cw.getCellAt(1, 1),
                    cw.getCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    CommonFactory.CreateRandom());

            IMap<Cell<double>, CellWorldAction> fixedPolicy = CollectionFactory.CreateMap<Cell<double>, CellWorldAction>();
            fixedPolicy.Put(cw.getCellAt(1, 1), CellWorldAction.Up);
            fixedPolicy.Put(cw.getCellAt(1, 2), CellWorldAction.Up);
            fixedPolicy.Put(cw.getCellAt(1, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.getCellAt(2, 1), CellWorldAction.Left);
            fixedPolicy.Put(cw.getCellAt(2, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.getCellAt(3, 1), CellWorldAction.Left);
            fixedPolicy.Put(cw.getCellAt(3, 2), CellWorldAction.Up);
            fixedPolicy.Put(cw.getCellAt(3, 3), CellWorldAction.Right);
            fixedPolicy.Put(cw.getCellAt(4, 1), CellWorldAction.Left);

            PassiveTDAgent<Cell<double>, CellWorldAction> ptda
                = new PassiveTDAgent<Cell<double>, CellWorldAction>(fixedPolicy, 0.2, 1.0);

            cwe.AddAgent(ptda);

            output_utility_learning_rates(ptda, 20, 500, 100, 1); 
        }
    }
}
