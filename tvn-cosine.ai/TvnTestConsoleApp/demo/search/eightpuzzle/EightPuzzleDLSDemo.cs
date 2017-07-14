using System;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.environment.eightpuzzle;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.uninformed;

namespace TvnTestConsoleApp.demo.search.eightpuzzle
{
    class EightPuzzleDLSDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("\nEightPuzzleDemo recursive DLS (9) -->");

            eightPuzzleDLSDemo();
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }
         
        private static void eightPuzzleDLSDemo()
        {
            IProblem<EightPuzzleBoard, tvn.cosine.ai.agent.Action> problem = new BidirectionalEightPuzzleProblem(Util.boardWithThreeMoveSolution);
            SearchForActions<EightPuzzleBoard, tvn.cosine.ai.agent.Action> search = new DepthLimitedSearch<EightPuzzleBoard, tvn.cosine.ai.agent.Action>(9);
            SearchAgent<EightPuzzleBoard, tvn.cosine.ai.agent.Action> agent = new SearchAgent<EightPuzzleBoard, tvn.cosine.ai.agent.Action>(problem, search);
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation()); 
        }
    }
}
