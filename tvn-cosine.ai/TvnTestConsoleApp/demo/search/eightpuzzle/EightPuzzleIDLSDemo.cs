using System; 
using tvn.cosine.ai.agent;
using tvn.cosine.ai.environment.eightpuzzle;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.uninformed;

namespace TvnTestConsoleApp.demo.search.eightpuzzle
{
    class EightPuzzleIDLSDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("\nEightPuzzleDemo Iterative DLS -->");

            eightPuzzleIDLSDemo();
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void eightPuzzleIDLSDemo()
        { 
            IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(Util.random1);
            SearchForActions<EightPuzzleBoard, IAction> search = new IterativeDeepeningSearch<EightPuzzleBoard, IAction>();
            SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation()); 
        }
    }
}
