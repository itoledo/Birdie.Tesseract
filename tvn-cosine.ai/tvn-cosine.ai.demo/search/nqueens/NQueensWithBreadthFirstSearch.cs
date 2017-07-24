using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.environment.nqueens;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.search.uninformed;

namespace tvn_cosine.ai.demo.search.nqueens
{
    public class NQueensWithBreadthFirstSearch : NQueensDemoBase
    {
        static void Main(params string[] args)
        {
            nQueensWithBreadthFirstSearch();
        }

        static void nQueensWithBreadthFirstSearch()
        {
            try
            {
                System.Console.WriteLine("\nNQueensDemo BFS -->");
                Problem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createIncrementalFormulationProblem(boardSize);
                SearchForActions<NQueensBoard, QueenAction> 
                    search = new BreadthFirstSearch<NQueensBoard, QueenAction>(new TreeSearch<NQueensBoard, QueenAction>());
                SearchAgent<NQueensBoard, QueenAction> 
                    agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);
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
