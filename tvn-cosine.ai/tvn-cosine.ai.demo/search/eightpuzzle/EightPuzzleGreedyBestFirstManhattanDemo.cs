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
    public class EightPuzzleGreedyBestFirstManhattanDemo : EightPuzzleDemoBase
    {
        static void Main(params string[] args)
        {
            eightPuzzleGreedyBestFirstManhattanDemo();
        }

        static void eightPuzzleGreedyBestFirstManhattanDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo Greedy Best First Search (ManhattanHeursitic)-->");
            try
            {
                IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
                ISearchForActions<EightPuzzleBoard, IAction>
                    search = new GreedyBestFirstSearch<EightPuzzleBoard, IAction>(
                        new GraphSearch<EightPuzzleBoard, IAction>(),
                        EightPuzzleFunctions.createManhattanHeuristicFunction());
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
