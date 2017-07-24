using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.environment.nqueens;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.local;

namespace tvn_cosine.ai.demo.search.nqueens
{
    public class NQueensSimulatedAnnealingSearch : NQueensDemoBase
    {
        static void Main(params string[] args)
        {
            nQueensSimulatedAnnealingSearch();
        }

        static void nQueensSimulatedAnnealingSearch()
        {
            System.Console.WriteLine("\nNQueensDemo Simulated Annealing  -->");
            try
            {
                Problem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createCompleteStateFormulationProblem(
                            boardSize, 
                            NQueensBoard.Config.QUEENS_IN_FIRST_ROW);

                SimulatedAnnealingSearch<NQueensBoard, QueenAction> 
                    search = new SimulatedAnnealingSearch<NQueensBoard, QueenAction>(
                        NQueensFunctions.createAttackingPairsHeuristicFunction(),
                        new Scheduler(20, 0.045, 100));
                SearchAgent<NQueensBoard, QueenAction> 
                    agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);

                System.Console.WriteLine();
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
