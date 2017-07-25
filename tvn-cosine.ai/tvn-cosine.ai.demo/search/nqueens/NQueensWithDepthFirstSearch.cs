using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.environment.nqueens;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.api;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.problem.api;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.search.uninformed;

namespace tvn_cosine.ai.demo.search.nqueens
{
    public class NQueensWithDepthFirstSearch : NQueensDemoBase
    {
        static void Main(params string[] args)
        {
            nQueensWithDepthFirstSearch();
        }

        static void nQueensWithDepthFirstSearch()
        {
            System.Console.WriteLine("\nNQueensDemo DFS -->");
            try
            {
                IProblem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createIncrementalFormulationProblem(boardSize);
                ISearchForActions<NQueensBoard, QueenAction>
                    search = new DepthFirstSearch<NQueensBoard, QueenAction>(
                        new GraphSearch<NQueensBoard, QueenAction>());
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
