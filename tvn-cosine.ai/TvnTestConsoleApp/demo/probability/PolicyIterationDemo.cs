using System;
using tvn.cosine.ai.environment.cellworld;
using tvn.cosine.ai.probability.example;
using tvn.cosine.ai.probability.mdp;
using tvn.cosine.ai.probability.mdp.impl;
using tvn.cosine.ai.probability.mdp.search;

namespace TvnTestConsoleApp.demo.probability
{
    class PolicyIterationDemo
    {
        public static void Main(params string[] args)
        {
            policyIterationDemo();

            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        public static void policyIterationDemo()
        {

            Console.WriteLine("DEMO: Policy Iteration");
            Console.WriteLine("======================");
            Console.WriteLine("Figure 17.3");
            Console.WriteLine("-----------");

            CellWorld<double> cw = CellWorldFactory.CreateCellWorldForFig17_1();
            MarkovDecisionProcess<Cell<double>, CellWorldAction> mdp = MDPFactory.createMDPForFigure17_3(cw);
            PolicyIteration<Cell<double>, CellWorldAction> pi 
                = new PolicyIteration<Cell<double>, CellWorldAction>(new ModifiedPolicyEvaluation<Cell<double>, CellWorldAction>(50, 1.0));

            Policy<Cell<double>, CellWorldAction> policy = pi.policyIteration(mdp);

            Console.WriteLine("(1,1) = " + policy.action(cw.getCellAt(1, 1)));
            Console.WriteLine("(1,2) = " + policy.action(cw.getCellAt(1, 2)));
            Console.WriteLine("(1,3) = " + policy.action(cw.getCellAt(1, 3)));

            Console.WriteLine("(2,1) = " + policy.action(cw.getCellAt(2, 1)));
            Console.WriteLine("(2,3) = " + policy.action(cw.getCellAt(2, 3)));

            Console.WriteLine("(3,1) = " + policy.action(cw.getCellAt(3, 1)));
            Console.WriteLine("(3,2) = " + policy.action(cw.getCellAt(3, 2)));
            Console.WriteLine("(3,3) = " + policy.action(cw.getCellAt(3, 3)));

            Console.WriteLine("(4,1) = " + policy.action(cw.getCellAt(4, 1)));
            Console.WriteLine("(4,2) = " + policy.action(cw.getCellAt(4, 2)));
            Console.WriteLine("(4,3) = " + policy.action(cw.getCellAt(4, 3)));

            Console.WriteLine("=========================");
        }
    }
}
