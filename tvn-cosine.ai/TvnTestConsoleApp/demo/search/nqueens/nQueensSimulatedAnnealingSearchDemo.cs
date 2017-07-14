using System; 
using tvn.cosine.ai.environment.nqueens;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.local;

namespace TvnTestConsoleApp.demo.search.nqueens
{
    class nQueensSimulatedAnnealingSearchDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("\nNQueensDemo Simulated Annealing  -->");

            nQueensSimulatedAnnealingSearch();
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void nQueensSimulatedAnnealingSearch()
        {

            IProblem<NQueensBoard, QueenAction> problem =
                    NQueensFunctions.createCompleteStateFormulationProblem(Util.boardSize, NQueensBoard.Config.QUEENS_IN_FIRST_ROW);

            SimulatedAnnealingSearch<NQueensBoard, QueenAction> search =
                    new SimulatedAnnealingSearch<NQueensBoard, QueenAction>(NQueensFunctions.createAttackingPairsHeuristicFunction(),
                    new Scheduler(20, 0.045, 100));
            SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);

            Console.WriteLine();
            Util.printActions(agent.GetActions());
            Console.WriteLine("Search Outcome=" + search.getOutcome());
            Console.WriteLine("Final State=\n" + search.getLastSearchState());
            Util.printInstrumentation(agent.GetInstrumentation()); 
        }
    }
}
