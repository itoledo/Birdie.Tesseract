using System;
using System.Collections.Generic; 
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.learning.reinforcement.agent;
using tvn.cosine.ai.learning.reinforcement.example;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.mdp.impl;

namespace TvnTestConsoleApp.demo.learning
{
    class PassiveADPAgentDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("=======================");
            Console.WriteLine("DEMO: Passive-ADP-Agent");
            Console.WriteLine("=======================");
            Console.WriteLine("Figure 21.3");
            Console.WriteLine("-----------");

            passiveADPAgentDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void passiveADPAgentDemo()
        {

            CellWorld<double> cw = CellWorldFactory.CreateCellWorldForFig17_1();
            CellWorldEnvironment cwe = new CellWorldEnvironment(
                    cw.getCellAt(1, 1),
                    cw.GetCells(),
                    MDPFactory.createTransitionProbabilityFunctionForFigure17_1(cw), new Random());

            IDictionary<Cell<double>, CellWorldAction> fixedPolicy = new Dictionary<Cell<double>, CellWorldAction>();
            fixedPolicy.Add(cw.getCellAt(1, 1), CellWorldAction.Up);
            fixedPolicy.Add(cw.getCellAt(1, 2), CellWorldAction.Up);
            fixedPolicy.Add(cw.getCellAt(1, 3), CellWorldAction.Right);
            fixedPolicy.Add(cw.getCellAt(2, 1), CellWorldAction.Left);
            fixedPolicy.Add(cw.getCellAt(2, 3), CellWorldAction.Right);
            fixedPolicy.Add(cw.getCellAt(3, 1), CellWorldAction.Left);
            fixedPolicy.Add(cw.getCellAt(3, 2), CellWorldAction.Up);
            fixedPolicy.Add(cw.getCellAt(3, 3), CellWorldAction.Right);
            fixedPolicy.Add(cw.getCellAt(4, 1), CellWorldAction.Left);

            PassiveADPAgent<Cell<double>, CellWorldAction> padpa = new PassiveADPAgent<Cell<double>, CellWorldAction>(
                    fixedPolicy, cw.GetCells(), cw.getCellAt(1, 1),
                    MDPFactory.createActionsFunctionForFigure17_1(cw),
                    new ModifiedPolicyEvaluation<Cell<double>, CellWorldAction>(10, 1.0));

            cwe.AddAgent(padpa);

            Util.output_utility_learning_rates(padpa, 20, 100, 100, 1);

            Console.WriteLine("=========================");
        }
         
    }
}
