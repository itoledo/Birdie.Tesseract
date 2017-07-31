using tvn.cosine.ai.agent.api;
using tvn.cosine.exceptions;
using tvn.cosine.ai.environment.eightpuzzle;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.api;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.problem.api;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.search.informed;

namespace tvn_cosine.ai.demo.search.eightpuzzle
{
    class EightPuzzleGreedyBestFirstDemo : EightPuzzleDemoBase
    {
        static void Main(params string[] args)
        {
            eightPuzzleGreedyBestFirstDemo();
        }

        static void eightPuzzleGreedyBestFirstDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo Greedy Best First Search (MisplacedTileHeursitic)-->");
            try
            {
                IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
                ISearchForActions<EightPuzzleBoard, IAction>
                    search = new GreedyBestFirstSearch<EightPuzzleBoard, IAction>(
                        new GraphSearch<EightPuzzleBoard, IAction>(), 
                        EightPuzzleFunctions.createMisplacedTileHeuristicFunction());
                SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
                printActions(agent.getActions());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
