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
    class EightPuzzleGreedyBestFirstManhattanDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("\nEightPuzzleDemo Greedy Best First Search (ManhattanHeursitic)-->");

            eightPuzzleGreedyBestFirstManhattanDemo();
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void eightPuzzleGreedyBestFirstManhattanDemo()
        {

            IProblem<EightPuzzleBoard, tvn.cosine.ai.agent.Action> problem = new BidirectionalEightPuzzleProblem(Util.boardWithThreeMoveSolution);
            SearchForActions<EightPuzzleBoard, tvn.cosine.ai.agent.Action> search = new GreedyBestFirstSearch<EightPuzzleBoard, tvn.cosine.ai.agent.Action>
                    (new GraphSearch<EightPuzzleBoard, tvn.cosine.ai.agent.Action>(), EightPuzzleFunctions.createManhattanHeuristicFunction());
            SearchAgent<EightPuzzleBoard, tvn.cosine.ai.agent.Action> agent = new SearchAgent<EightPuzzleBoard, tvn.cosine.ai.agent.Action>(problem, search);
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation());
        }
    }
}
