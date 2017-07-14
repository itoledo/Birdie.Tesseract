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
            IProblem<EightPuzzleBoard, tvn.cosine.ai.agent.Action> problem = new BidirectionalEightPuzzleProblem(Util.random1);
            SimulatedAnnealingSearch<EightPuzzleBoard, tvn.cosine.ai.agent.Action> search = new SimulatedAnnealingSearch<EightPuzzleBoard, tvn.cosine.ai.agent.Action>
                    (EightPuzzleFunctions.createManhattanHeuristicFunction());
            SearchAgent<EightPuzzleBoard, tvn.cosine.ai.agent.Action> agent = new SearchAgent<EightPuzzleBoard, tvn.cosine.ai.agent.Action>(problem, search);
            Util.printActions(agent.GetActions());
            Console.WriteLine("Search Outcome=" + search.getOutcome());
            Console.WriteLine("Final State=\n" + search.getLastSearchState());
            Util.printInstrumentation(agent.GetInstrumentation()); 
        }
    }
}
