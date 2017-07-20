using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.environment.nqueens;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.search.local;
using tvn.cosine.ai.search.uninformed;
using tvn.cosine.ai.util;
using static tvn.cosine.ai.environment.nqueens.NQueensBoard;

namespace tvn_cosine.ai.demo.search
{
    public class NQueensDemo
    {

        private static readonly int boardSize = 8;

        public static void Main(params string[] args)
        {

            newNQueensDemo();
        }

        private static void newNQueensDemo()
        {

            nQueensWithDepthFirstSearch();
            nQueensWithBreadthFirstSearch();
            nQueensWithRecursiveDLS();
            nQueensWithIterativeDeepeningSearch();
            nQueensSimulatedAnnealingSearch();
            nQueensHillClimbingSearch();
            nQueensGeneticAlgorithmSearch();
        }

        private static void nQueensWithRecursiveDLS()
        {
            System.Console.WriteLine("\nNQueensDemo recursive DLS -->");
            try
            {
                Problem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createIncrementalFormulationProblem(boardSize);
                SearchForActions<NQueensBoard, QueenAction> search = new DepthLimitedSearch<NQueensBoard, QueenAction>(boardSize);
                SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);
                printActions(agent.getActions());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static void nQueensWithBreadthFirstSearch()
        {
            try
            {
                System.Console.WriteLine("\nNQueensDemo BFS -->");
                Problem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createIncrementalFormulationProblem(boardSize);
                SearchForActions<NQueensBoard, QueenAction> search = new BreadthFirstSearch<NQueensBoard, QueenAction>(new TreeSearch<NQueensBoard, QueenAction>());
                SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);
                printActions(agent.getActions());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void nQueensWithDepthFirstSearch()
        {
            System.Console.WriteLine("\nNQueensDemo DFS -->");
            try
            {
                Problem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createIncrementalFormulationProblem(boardSize);
                SearchForActions<NQueensBoard, QueenAction> search = new DepthFirstSearch<NQueensBoard, QueenAction>(new GraphSearch<NQueensBoard, QueenAction>());
                SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);
                printActions(agent.getActions());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void nQueensWithIterativeDeepeningSearch()
        {
            System.Console.WriteLine("\nNQueensDemo Iterative DS  -->");
            try
            {
                Problem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createIncrementalFormulationProblem(boardSize);
                SearchForActions<NQueensBoard, QueenAction> search = new IterativeDeepeningSearch<NQueensBoard, QueenAction>();
                SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);

                System.Console.WriteLine();
                printActions(agent.getActions());
                printInstrumentation(agent.getInstrumentation());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static void nQueensSimulatedAnnealingSearch()
        {
            System.Console.WriteLine("\nNQueensDemo Simulated Annealing  -->");
            try
            {
                Problem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createCompleteStateFormulationProblem(boardSize, Config.QUEENS_IN_FIRST_ROW);

                SimulatedAnnealingSearch<NQueensBoard, QueenAction> search =
                        new SimulatedAnnealingSearch<NQueensBoard, QueenAction>(NQueensFunctions.createAttackingPairsHeuristicFunction(),
                        new Scheduler(20, 0.045, 100));
                SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);

                System.Console.WriteLine();
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

        private static void nQueensHillClimbingSearch()
        {
            System.Console.WriteLine("\nNQueensDemo HillClimbing  -->");
            try
            {
                Problem<NQueensBoard, QueenAction> problem =
                        NQueensFunctions.createCompleteStateFormulationProblem(boardSize, Config.QUEENS_IN_FIRST_ROW);
                HillClimbingSearch<NQueensBoard, QueenAction> search = new HillClimbingSearch<NQueensBoard, QueenAction>
                        (NQueensFunctions.createAttackingPairsHeuristicFunction());
                SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);

                System.Console.WriteLine();
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

        private static void nQueensGeneticAlgorithmSearch()
        {
            System.Console.WriteLine("\nNQueensDemo GeneticAlgorithm  -->");
            try
            {
                FitnessFunction<int> fitnessFunction = NQueensGenAlgoUtil.getFitnessFunction();
                GoalTest<Individual<int>> goalTest = NQueensGenAlgoUtil.getGoalTest();
                // Generate an initial population
                ISet<Individual<int>> population = Factory.CreateSet<Individual<int>>();
                for (int i = 0; i < 50;++i)
                {
                    population.Add(NQueensGenAlgoUtil.generateRandomIndividual(boardSize));
                }

                GeneticAlgorithm<int> ga = new GeneticAlgorithm<int>(boardSize,
                        NQueensGenAlgoUtil.getFiniteAlphabetForBoardOfSize(boardSize), 0.15);

                // Run for a set amount of time
                Individual<int> bestIndividual = ga.geneticAlgorithm(population, fitnessFunction, goalTest, 1000L);

                System.Console.WriteLine("Max Time (1 second) Best Individual=\n"
                        + NQueensGenAlgoUtil.getBoardForIndividual(bestIndividual));
                System.Console.WriteLine("Board Size      = " + boardSize);
                //System.Console.WriteLine("# Board Layouts = " + (new BigDecimal(boardSize)).pow(boardSize)/*);*/
                System.Console.WriteLine("Fitness         = " + fitnessFunction.apply(bestIndividual));
                System.Console.WriteLine("Is Goal         = " + goalTest(bestIndividual));
                System.Console.WriteLine("Population Size = " + ga.getPopulationSize());
                System.Console.WriteLine("Iterations      = " + ga.getIterations());
                System.Console.WriteLine("Took            = " + ga.getTimeInMilliseconds() + "ms.");

                // Run till goal is achieved
                bestIndividual = ga.geneticAlgorithm(population, fitnessFunction, goalTest, 0L);

                System.Console.WriteLine("");
                System.Console
                    .WriteLine("Goal Test Best Individual=\n" + NQueensGenAlgoUtil.getBoardForIndividual(bestIndividual));
                System.Console.WriteLine("Board Size      = " + boardSize);
                //System.Console.WriteLine("# Board Layouts = " + (new BigDecimal(boardSize)).pow(boardSize)/*);*/
                System.Console.WriteLine("Fitness         = " + fitnessFunction.apply(bestIndividual));
                System.Console.WriteLine("Is Goal         = " + goalTest(bestIndividual));
                System.Console.WriteLine("Population Size = " + ga.getPopulationSize());
                System.Console.WriteLine("Itertions       = " + ga.getIterations());
                System.Console.WriteLine("Took            = " + ga.getTimeInMilliseconds() + "ms.");

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

        private static void printActions(IQueue<QueenAction> actions)
        {
            foreach (IAction action in actions)
            {
                System.Console.WriteLine(action.ToString());
            }
        }

    }
}
