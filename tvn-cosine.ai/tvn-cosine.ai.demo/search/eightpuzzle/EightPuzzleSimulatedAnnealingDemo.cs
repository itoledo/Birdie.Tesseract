using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.environment.eightpuzzle;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.problem.api;
using tvn.cosine.ai.search.local;

namespace tvn_cosine.ai.demo.search.eightpuzzle
{
    public class EightPuzzleSimulatedAnnealingDemo : EightPuzzleDemoBase
    {
        static void Main(params string[] args)
        {
            eightPuzzleSimulatedAnnealingDemo();
        }

        static void eightPuzzleSimulatedAnnealingDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo Simulated Annealing  Search -->");
            try
            {
                IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
                SimulatedAnnealingSearch<EightPuzzleBoard, IAction>
                    search = new SimulatedAnnealingSearch<EightPuzzleBoard, IAction>(
                        EightPuzzleFunctions.createManhattanHeuristicFunction());
                SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
                printActions(agent.getActions());
                System.Console.WriteLine("Search Outcome=" + search.getOutcome());
                System.Console.WriteLine("Final State=\n" + search.getLastSearchState());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
