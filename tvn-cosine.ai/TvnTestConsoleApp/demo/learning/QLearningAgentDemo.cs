using System;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.learning.reinforcement.agent;
using tvn.cosine.ai.learning.reinforcement.example;
using tvn.cosine.ai.probability.example;

namespace TvnTestConsoleApp.demo.learning
{
    class QLearningAgentDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("======================");
            Console.WriteLine("DEMO: Q-Learning-Agent");
            Console.WriteLine("======================");

            qLearningAgentDemo();

            Console.WriteLine("=========================");
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }


        public static void qLearningAgentDemo()
        { 
            CellWorld<double> cw = CellWorldFactory.CreateCellWorldForFig17_1();
            CellWorldEnvironment cwe = new CellWorldEnvironment(
                    cw.getCellAt(1, 1),
                    cw.GetCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw),
                    new Random());

            QLearningAgent<Cell<double>, CellWorldAction> qla = new QLearningAgent<Cell<double>, CellWorldAction>(
                    MDPFactory.createActionsFunctionForFigure17_1(cw),
                    CellWorldAction.None, 0.2, 1.0, 5,
                    2.0);

            cwe.AddAgent(qla);

            Util.output_utility_learning_rates(qla, 20, 10000, 500, 20); 
        }
    }
}
