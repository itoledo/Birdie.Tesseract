using System;
using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.environment.eightpuzzle;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.search.informed;
using tvn.cosine.ai.search.local;
using tvn.cosine.ai.search.uninformed;

namespace TvnTestConsoleApp.demo.search
{
    public static class Util
    {
        private static EightPuzzleBoard boardWithThreeMoveSolution = new EightPuzzleBoard(new int[] { 1, 2, 5, 3, 4, 0, 6, 7, 8 });
        private static EightPuzzleBoard random1 = new EightPuzzleBoard(new int[] { 1, 4, 2, 7, 5, 8, 3, 0, 6 });

        //	private static EightPuzzleBoard extreme = new EightPuzzleBoard(new int[] { 0, 8, 7, 6, 5, 4, 3, 2, 1 });

        public static void main(params string[] args)
        {
            eightPuzzleDLSDemo();
            eightPuzzleIDLSDemo();
            eightPuzzleGreedyBestFirstDemo();
            eightPuzzleGreedyBestFirstManhattanDemo();
            eightPuzzleAStarDemo();
            eightPuzzleAStarManhattanDemo();
            eightPuzzleSimulatedAnnealingDemo();
        }

        private static void eightPuzzleDLSDemo()
        {
            Console.WriteLine("\nEightPuzzleDemo recursive DLS (9) -->");

            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
            SearchForActions<EightPuzzleBoard, IAction> search = new DepthLimitedSearch<EightPuzzleBoard, IAction>(9);
            SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
            printActions(agent.GetActions());
            printInstrumentation(agent.GetInstrumentation());

        }

        private static void eightPuzzleIDLSDemo()
        {
            Console.WriteLine("\nEightPuzzleDemo Iterative DLS -->");

            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
            SearchForActions<EightPuzzleBoard, IAction> search = new IterativeDeepeningSearch<EightPuzzleBoard, IAction>();
            SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
            printActions(agent.GetActions());
            printInstrumentation(agent.GetInstrumentation());

        }

        private static void eightPuzzleGreedyBestFirstDemo()
        {
            Console.WriteLine("\nEightPuzzleDemo Greedy Best First Search (MisplacedTileHeursitic)-->");

            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
            SearchForActions<EightPuzzleBoard, IAction> search = new GreedyBestFirstSearch<EightPuzzleBoard, IAction>
                    (new GraphSearch<EightPuzzleBoard, IAction>(), EightPuzzleFunctions.createMisplacedTileHeuristicFunction());
            SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
            printActions(agent.GetActions());
            printInstrumentation(agent.GetInstrumentation());

        }

        private static void eightPuzzleGreedyBestFirstManhattanDemo()
        {
            Console.WriteLine("\nEightPuzzleDemo Greedy Best First Search (ManhattanHeursitic)-->");

            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
            SearchForActions<EightPuzzleBoard, IAction> search = new GreedyBestFirstSearch<EightPuzzleBoard, IAction>
                    (new GraphSearch<EightPuzzleBoard, IAction>(), EightPuzzleFunctions.createManhattanHeuristicFunction());
            SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
            printActions(agent.GetActions());
            printInstrumentation(agent.GetInstrumentation());
        }

        private static void eightPuzzleAStarDemo()
        {
            Console.WriteLine("\nEightPuzzleDemo AStar Search (MisplacedTileHeursitic)-->");

            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
            SearchForActions<EightPuzzleBoard, IAction> search = new AStarSearch<EightPuzzleBoard, IAction>
                    (new GraphSearch<EightPuzzleBoard, IAction>(), EightPuzzleFunctions.createMisplacedTileHeuristicFunction());
            SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
            printActions(agent.GetActions());
            printInstrumentation(agent.GetInstrumentation());

        }

        private static void eightPuzzleSimulatedAnnealingDemo()
        {
            Console.WriteLine("\nEightPuzzleDemo Simulated Annealing  Search -->");

            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
            SimulatedAnnealingSearch<EightPuzzleBoard, IAction> search = new SimulatedAnnealingSearch<EightPuzzleBoard, IAction>
                    (EightPuzzleFunctions.createManhattanHeuristicFunction());
            SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
            printActions(agent.GetActions());
            Console.WriteLine("Search Outcome=" + search.getOutcome());
            Console.WriteLine("Final State=\n" + search.getLastSearchState());
            printInstrumentation(agent.GetInstrumentation());

        }

        private static void eightPuzzleAStarManhattanDemo()
        {
            Console.WriteLine("\nEightPuzzleDemo AStar Search (ManhattanHeursitic)-->");

            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
            SearchForActions<EightPuzzleBoard, IAction> search
                = new AStarSearch<EightPuzzleBoard, IAction>(new GraphSearch<EightPuzzleBoard, IAction>(),
                EightPuzzleFunctions.createManhattanHeuristicFunction());
            SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
            printActions(agent.GetActions());
            printInstrumentation(agent.GetInstrumentation());

        }

        private static void printInstrumentation(IDictionary<string, double> properties)
        {
            foreach (var o in properties)
            {
                Console.WriteLine(o.Key + " : " + o.Value);
            }
        }

        private static void printActions(IList<IAction> actions)
        {
            foreach (var a in actions)
                Console.WriteLine(a);
        }
    }
}
