using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.environment.eightpuzzle;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.uninformed;

namespace tvn_cosine.ai.demo.search.eightpuzzle
{
    public class EightPuzzleDLSDemo : EightPuzzleDemoBase
    {
        static void Main(params string[] args)
        {
            eightPuzzleDLSDemo();
        }

        static void eightPuzzleDLSDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo recursive DLS (9) -->");
            try
            {
                Problem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
                SearchForActions<EightPuzzleBoard, IAction> search = new DepthLimitedSearch<EightPuzzleBoard, IAction>(9);
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
