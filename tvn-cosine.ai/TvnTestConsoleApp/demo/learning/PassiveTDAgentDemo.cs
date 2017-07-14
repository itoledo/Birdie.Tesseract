using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.learning.reinforcement.agent;
using tvn.cosine.ai.learning.reinforcement.example;
using tvn.cosine.ai.probability.example;

namespace TvnTestConsoleApp.demo.learning
{
    class PassiveTDAgentDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("======================");
            Console.WriteLine("DEMO: Passive-TD-Agent");
            Console.WriteLine("======================");
            Console.WriteLine("Figure 21.5");
            Console.WriteLine("-----------");

            passiveTDAgentDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void passiveTDAgentDemo()
        {
            CellWorld<double> cw = CellWorldFactory.CreateCellWorldForFig17_1();
            CellWorldEnvironment cwe = new CellWorldEnvironment(
                    cw.getCellAt(1, 1),
                    cw.GetCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    new Random());

            IDictionary<Cell<double>, CellWorldAction> fixedPolicy = new Dictionary<Cell<double>, CellWorldAction>();
            fixedPolicy[cw.getCellAt(1, 1)] = CellWorldAction.Up;
            fixedPolicy[cw.getCellAt(1, 2)] = CellWorldAction.Up;
            fixedPolicy[cw.getCellAt(1, 3)] = CellWorldAction.Right;
            fixedPolicy[cw.getCellAt(2, 1)] = CellWorldAction.Left;
            fixedPolicy[cw.getCellAt(2, 3)] = CellWorldAction.Right;
            fixedPolicy[cw.getCellAt(3, 1)] = CellWorldAction.Left;
            fixedPolicy[cw.getCellAt(3, 2)] = CellWorldAction.Up;
            fixedPolicy[cw.getCellAt(3, 3)] = CellWorldAction.Right;
            fixedPolicy[cw.getCellAt(4, 1)] = CellWorldAction.Left;

            PassiveTDAgent<Cell<Double>, CellWorldAction> ptda = new PassiveTDAgent<Cell<double>, CellWorldAction>(fixedPolicy, 0.2, 1.0);

            cwe.addAgent(ptda);

            Util.output_utility_learning_rates(ptda, 20, 500, 100, 1);

            Console.WriteLine("=========================");
        }

    }
}
