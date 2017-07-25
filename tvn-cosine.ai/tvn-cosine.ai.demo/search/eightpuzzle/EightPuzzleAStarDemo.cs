using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.common.exceptions;
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
    public class EightPuzzleAStarDemo : EightPuzzleDemoBase
    {
        static void Main(params string[] args)
        {
            eightPuzzleAStarDemo();
        }

        static void eightPuzzleAStarDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo AStar Search (MisplacedTileHeursitic)-->");
            try
            {
                IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
                ISearchForActions<EightPuzzleBoard, IAction>
                    search = new AStarSearch<EightPuzzleBoard, IAction>(
                        new GraphSearch<EightPuzzleBoard, IAction>(), EightPuzzleFunctions.createMisplacedTileHeuristicFunction());
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
