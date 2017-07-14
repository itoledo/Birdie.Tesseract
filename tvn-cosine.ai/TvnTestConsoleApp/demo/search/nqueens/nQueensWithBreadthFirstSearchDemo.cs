using System;
using tvn.cosine.ai.environment.nqueens;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.search.uninformed;

namespace TvnTestConsoleApp.demo.search.nqueens
{
    class nQueensWithBreadthFirstSearchDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("\nNQueensDemo BFS -->");

            nQueensWithBreadthFirstSearch();
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void nQueensWithBreadthFirstSearch()
        {
            IProblem<NQueensBoard, QueenAction> problem =
                    NQueensFunctions.createIncrementalFormulationProblem(Util.boardSize);
            SearchForActions<NQueensBoard, QueenAction> search
                = new BreadthFirstSearch<NQueensBoard, QueenAction>(new TreeSearch<NQueensBoard, QueenAction>());
            SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation());

        }
    }
}
