using System; 
using tvn.cosine.ai.environment.nqueens;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.search.uninformed;

namespace TvnTestConsoleApp.demo.search.nqueens
{
    class nQueensWithDepthFirstSearchDemo
    {

        private static void nQueensWithDepthFirstSearch()
        {
            Console.WriteLine("\nNQueensDemo DFS -->");

            IProblem<NQueensBoard, QueenAction> problem =
                    NQueensFunctions.createIncrementalFormulationProblem(Util.boardSize);
            SearchForActions<NQueensBoard, QueenAction> search
             = new DepthFirstSearch<NQueensBoard, QueenAction>(new GraphSearch<NQueensBoard, QueenAction>());
            SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation());

        }
    }
}
