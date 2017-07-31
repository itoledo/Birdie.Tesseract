using tvn.cosine.ai.agent.api;
using tvn.cosine.exceptions;
using tvn.cosine.ai.environment.eightpuzzle;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.api;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.problem.api;
using tvn.cosine.ai.search.uninformed;

namespace tvn_cosine.ai.demo.search.eightpuzzle
{
    public  class EightPuzzleIDLSDemo : EightPuzzleDemoBase
    {
        static void Main(params string[] args)
        {
            eightPuzzleIDLSDemo();
        }

        static void eightPuzzleIDLSDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo Iterative DLS -->");
            try
            {
                IProblem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
                ISearchForActions<EightPuzzleBoard, IAction> search = new IterativeDeepeningSearch<EightPuzzleBoard, IAction>();
                SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
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
