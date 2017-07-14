using System; 
using tvn.cosine.ai.environment.nqueens;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.uninformed;

namespace TvnTestConsoleApp.demo.search.nqueens
{
    class nQueensWithIterativeDeepeningSearchDemo
    { 
        public static void Main(params string[] args)
        {
            Console.WriteLine("\nNQueensDemo Iterative DS  -->");

            nQueensWithIterativeDeepeningSearch();
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void nQueensWithIterativeDeepeningSearch()
        {

            IProblem<NQueensBoard, QueenAction> problem =
                    NQueensFunctions.createIncrementalFormulationProblem(Util.boardSize);
            SearchForActions<NQueensBoard, QueenAction> search = new IterativeDeepeningSearch<NQueensBoard, QueenAction>();
            SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);

            Console.WriteLine();
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation());

        }
    }
}
