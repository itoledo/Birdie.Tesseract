using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.environment.eightpuzzle;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.search.informed;
using tvn.cosine.ai.search.local;
using tvn.cosine.ai.search.uninformed;
using tvn.cosine.ai.util;

namespace tvn_cosine.ai.demo.search
{
    public class EightPuzzleDemo
    {
        private static EightPuzzleBoard boardWithThreeMoveSolution =
                new EightPuzzleBoard(new int[] { 1, 2, 5, 3, 4, 0, 6, 7, 8 });

        private static EightPuzzleBoard random1 =
                new EightPuzzleBoard(new int[] { 1, 4, 2, 7, 5, 8, 3, 0, 6 });

        //	private static EightPuzzleBoard extreme =
        //			new EightPuzzleBoard(new int[] { 0, 8, 7, 6, 5, 4, 3, 2, 1 });

        public static void Main(params string[] args)
        {
            //	eightPuzzleDLSDemo();
            //	eightPuzzleIDLSDemo();
            //	eightPuzzleGreedyBestFirstDemo();
            eightPuzzleGreedyBestFirstManhattanDemo();
            //	eightPuzzleAStarDemo();
            //	eightPuzzleAStarManhattanDemo();
            //	eightPuzzleSimulatedAnnealingDemo();
        }

        private static void eightPuzzleDLSDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo recursive DLS (9) -->");
            try
            {
                Problem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
                SearchForActions<EightPuzzleBoard, IAction> search = new DepthLimitedSearch<EightPuzzleBoard, IAction>(9);
                SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
                printActions(agent.getActions());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static void eightPuzzleIDLSDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo Iterative DLS -->");
            try
            {
                Problem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
                SearchForActions<EightPuzzleBoard, IAction> search = new IterativeDeepeningSearch<EightPuzzleBoard, IAction>();
                SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
                printActions(agent.getActions());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static void eightPuzzleGreedyBestFirstDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo Greedy Best First Search (MisplacedTileHeursitic)-->");
            try
            {
                Problem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
                SearchForActions<EightPuzzleBoard, IAction> search = new GreedyBestFirstSearch<EightPuzzleBoard, IAction>
                        (new GraphSearch<EightPuzzleBoard, IAction>(), EightPuzzleFunctions.createMisplacedTileHeuristicFunction());
                SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
                printActions(agent.getActions());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static void eightPuzzleGreedyBestFirstManhattanDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo Greedy Best First Search (ManhattanHeursitic)-->");
            try
            {
                Problem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
                SearchForActions<EightPuzzleBoard, IAction> search = new GreedyBestFirstSearch<EightPuzzleBoard, IAction>
                        (new GraphSearch<EightPuzzleBoard, IAction>(), EightPuzzleFunctions.createManhattanHeuristicFunction());
                SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
                printActions(agent.getActions());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static void eightPuzzleAStarDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo AStar Search (MisplacedTileHeursitic)-->");
            try
            {
                Problem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
                SearchForActions<EightPuzzleBoard, IAction> search = new AStarSearch<EightPuzzleBoard, IAction>
                        (new GraphSearch<EightPuzzleBoard, IAction>(), EightPuzzleFunctions.createMisplacedTileHeuristicFunction());
                SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
                printActions(agent.getActions());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static void eightPuzzleSimulatedAnnealingDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo Simulated Annealing  Search -->");
            try
            {
                Problem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
                SimulatedAnnealingSearch<EightPuzzleBoard, IAction> search = new SimulatedAnnealingSearch<EightPuzzleBoard, IAction>
                        (EightPuzzleFunctions.createManhattanHeuristicFunction());
                SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
                printActions(agent.getActions());
                System.Console.WriteLine("Search Outcome=" + search.getOutcome());
                System.Console.WriteLine("Final State=\n" + search.getLastSearchState());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void eightPuzzleAStarManhattanDemo()
        {
            System.Console.WriteLine("\nEightPuzzleDemo AStar Search (ManhattanHeursitic)-->");
            try
            {
                Problem<EightPuzzleBoard, IAction> problem = new BidirectionalEightPuzzleProblem(random1);
                SearchForActions<EightPuzzleBoard, IAction> search = new AStarSearch<EightPuzzleBoard, IAction>
                        (new GraphSearch<EightPuzzleBoard, IAction>(), EightPuzzleFunctions.createManhattanHeuristicFunction());
                SearchAgent<EightPuzzleBoard, IAction> agent = new SearchAgent<EightPuzzleBoard, IAction>(problem, search);
                printActions(agent.getActions());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static void printInstrumentation(Properties properties)
        {
            foreach (object o in properties.GetKeys())
            {
                string key = (string)o;
                string property = (string)properties.getProperty(key);
                System.Console.WriteLine(key + " : " + property);
            }

        }

        private static void printActions(IQueue<IAction> actions)
        {
            foreach (IAction action in actions)
                System.Console.WriteLine(action);
        }
    }
}
