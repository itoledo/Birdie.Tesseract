using System;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.environment.eightpuzzle;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.local;

namespace TvnTestConsoleApp.demo.search.eightpuzzle
{
    class EightPuzzleSimulatedAnnealingDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("\nEightPuzzleDemo Simulated Annealing  Search -->");

            eightPuzzleSimulatedAnnealingDemo();
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void eightPuzzleSimulatedAnnealingDemo()
        { 
            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(Util.random1);
            SimulatedAnnealingSearch<EightPuzzleBoard, IAction> search = new SimulatedAnnealingSearch<EightPuzzleBoard, IAction>
                    (EightPuzzleFunctions.createManhattanHeuristicFunction());
            SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
            Util.printActions(agent.GetActions());
            Console.WriteLine("Search Outcome=" + search.getOutcome());
            Console.WriteLine("Final State=\n" + search.getLastSearchState());
            Util.printInstrumentation(agent.GetInstrumentation()); 
        }
    }
}
