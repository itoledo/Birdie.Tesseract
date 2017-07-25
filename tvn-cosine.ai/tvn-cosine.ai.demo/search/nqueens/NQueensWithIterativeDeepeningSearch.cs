using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.environment.nqueens;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.api;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.problem.api;
using tvn.cosine.ai.search.uninformed;

namespace tvn_cosine.ai.demo.search.nqueens
{
    public class NQueensWithIterativeDeepeningSearch : NQueensDemoBase
    {
        static void Main(params string[] args)
        {
            nQueensWithIterativeDeepeningSearch();
        }

        static void nQueensWithIterativeDeepeningSearch()
        {
            System.Console.WriteLine("\nNQueensDemo Iterative DS  -->");
            try
            {
                IProblem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createIncrementalFormulationProblem(boardSize);
                ISearchForActions<NQueensBoard, QueenAction> 
                    search = new IterativeDeepeningSearch<NQueensBoard, QueenAction>();
                SearchAgent<NQueensBoard, QueenAction> 
                    agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);

                System.Console.WriteLine();
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
