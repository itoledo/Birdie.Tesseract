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
                Problem<EightPuzzleBoard, Action> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
                SearchForActions<EightPuzzleBoard, Action> search = new DepthLimitedSearch<EightPuzzleBoard, Action>(9);
                SearchAgent<EightPuzzleBoard, Action> agent = new SearchAgent<EightPuzzleBoard, Action>(problem, search);
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
                Problem<EightPuzzleBoard, Action> problem = new BidirectionalEightPuzzleProblem(random1);
                SearchForActions<EightPuzzleBoard, Action> search = new IterativeDeepeningSearch<EightPuzzleBoard, Action>();
                SearchAgent<EightPuzzleBoard, Action> agent = new SearchAgent<EightPuzzleBoard, Action>(problem, search);
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
                Problem<EightPuzzleBoard, Action> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
                SearchForActions<EightPuzzleBoard, Action> search = new GreedyBestFirstSearch<EightPuzzleBoard, Action>
                        (new GraphSearch<EightPuzzleBoard, Action>(), EightPuzzleFunctions.createMisplacedTileHeuristicFunction());
                SearchAgent<EightPuzzleBoard, Action> agent = new SearchAgent<EightPuzzleBoard, Action>(problem, search);
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
                Problem<EightPuzzleBoard, Action> problem = new BidirectionalEightPuzzleProblem(boardWithThreeMoveSolution);
                SearchForActions<EightPuzzleBoard, Action> search = new GreedyBestFirstSearch<EightPuzzleBoard, Action>
                        (new GraphSearch<EightPuzzleBoard, Action>(), EightPuzzleFunctions.createManhattanHeuristicFunction());
                SearchAgent<EightPuzzleBoard, Action> agent = new SearchAgent<EightPuzzleBoard, Action>(problem, search);
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
                Problem<EightPuzzleBoard, Action> problem = new BidirectionalEightPuzzleProblem(random1);
                SearchForActions<EightPuzzleBoard, Action> search = new AStarSearch<EightPuzzleBoard, Action>
                        (new GraphSearch<EightPuzzleBoard, Action>(), EightPuzzleFunctions.createMisplacedTileHeuristicFunction());
                SearchAgent<EightPuzzleBoard, Action> agent = new SearchAgent<EightPuzzleBoard, Action>(problem, search);
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
                Problem<EightPuzzleBoard, Action> problem = new BidirectionalEightPuzzleProblem(random1);
                SimulatedAnnealingSearch<EightPuzzleBoard, Action> search = new SimulatedAnnealingSearch<EightPuzzleBoard, Action>
                        (EightPuzzleFunctions.createManhattanHeuristicFunction());
                SearchAgent<EightPuzzleBoard, Action> agent = new SearchAgent<EightPuzzleBoard, Action>(problem, search);
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
                Problem<EightPuzzleBoard, Action> problem = new BidirectionalEightPuzzleProblem(random1);
                SearchForActions<EightPuzzleBoard, Action> search = new AStarSearch<EightPuzzleBoard, Action>
                        (new GraphSearch<EightPuzzleBoard, Action>(), EightPuzzleFunctions.createManhattanHeuristicFunction());
                SearchAgent<EightPuzzleBoard, Action> agent = new SearchAgent<EightPuzzleBoard, Action>(problem, search);
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

        private static void printActions(IQueue<Action> actions)
        {
            foreach (Action action in actions)
                System.Console.WriteLine(action);
        }
    }
}
