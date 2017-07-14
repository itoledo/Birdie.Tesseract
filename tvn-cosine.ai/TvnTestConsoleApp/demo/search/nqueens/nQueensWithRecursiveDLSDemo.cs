using System; 
using tvn.cosine.ai.environment.nqueens;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.uninformed;

namespace TvnTestConsoleApp.demo.search.nqueens
{
    class nQueensWithRecursiveDLSDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("\nNQueensDemo recursive DLS -->");

            nQueensWithRecursiveDLS();
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void nQueensWithRecursiveDLS()
        { 
            IProblem<NQueensBoard, QueenAction> problem = NQueensFunctions.createIncrementalFormulationProblem(Util.boardSize);
            SearchForActions<NQueensBoard, QueenAction> search = new DepthLimitedSearch<NQueensBoard, QueenAction>(Util.boardSize);
            SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation()); 
        }
    }
}
