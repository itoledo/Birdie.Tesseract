using System;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.environment.eightpuzzle;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.search.informed;

namespace TvnTestConsoleApp.demo.search.eightpuzzle
{
    public class EightPuzzleAStarManhattanDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("\nEightPuzzleDemo AStar Search (ManhattanHeursitic)-->");

            eightPuzzleAStarManhattanDemo();
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void eightPuzzleAStarManhattanDemo()
        {
            IProblem<EightPuzzleBoard, tvn.cosine.ai.agent.Action> problem = new BidirectionalEightPuzzleProblem(Util.random1);
            SearchForActions<EightPuzzleBoard, tvn.cosine.ai.agent.Action> search
                = new AStarSearch<EightPuzzleBoard, tvn.cosine.ai.agent.Action>(new GraphSearch<EightPuzzleBoard, tvn.cosine.ai.agent.Action>(),
                EightPuzzleFunctions.createManhattanHeuristicFunction());
            SearchAgent<EightPuzzleBoard, tvn.cosine.ai.agent.Action> agent = new SearchAgent<EightPuzzleBoard, tvn.cosine.ai.agent.Action>(problem, search);
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation());

        }
    }
}
