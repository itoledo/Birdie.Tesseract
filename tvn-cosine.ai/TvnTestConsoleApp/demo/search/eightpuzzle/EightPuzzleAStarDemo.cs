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
    class EightPuzzleAStarDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("\nEightPuzzleDemo AStar Search (MisplacedTileHeursitic)-->");

            eightPuzzleAStarDemo();
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void eightPuzzleAStarDemo()
        {

            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(Util.random1);
            SearchForActions<EightPuzzleBoard, IAction> search = new AStarSearch<EightPuzzleBoard, IAction>
                    (new GraphSearch<EightPuzzleBoard, IAction>(), EightPuzzleFunctions.createMisplacedTileHeuristicFunction());
            SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation());

        }
    }
}
