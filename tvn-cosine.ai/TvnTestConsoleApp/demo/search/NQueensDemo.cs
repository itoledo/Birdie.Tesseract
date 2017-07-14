using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.environment.nqueens;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.agent;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.qsearch;
using tvn.cosine.ai.search.local;
using tvn.cosine.ai.search.uninformed; 

namespace TvnTestConsoleApp.demo.search
{
    class NQueensDemo
    {
        private const int boardSize = 8;

        public static void main(string[] args)
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
            Console.WriteLine("\nNQueensDemo recursive DLS -->");

            IProblem<NQueensBoard, QueenAction> problem =
                    NQueensFunctions.createIncrementalFormulationProblem(boardSize);
            SearchForActions<NQueensBoard, QueenAction> search = new DepthLimitedSearch<NQueensBoard, QueenAction>(boardSize);
            SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation());

        }

        private static void nQueensWithBreadthFirstSearch()
        {
            Console.WriteLine("\nNQueensDemo BFS -->");
            IProblem<NQueensBoard, QueenAction> problem =
                    NQueensFunctions.createIncrementalFormulationProblem(boardSize);
            SearchForActions<NQueensBoard, QueenAction> search
                = new BreadthFirstSearch<NQueensBoard, QueenAction>(new TreeSearch<NQueensBoard, QueenAction>());
            SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation());

        }

        private static void nQueensWithDepthFirstSearch()
        {
            Console.WriteLine("\nNQueensDemo DFS -->");

            IProblem<NQueensBoard, QueenAction> problem =
                    NQueensFunctions.createIncrementalFormulationProblem(boardSize);
            SearchForActions<NQueensBoard, QueenAction> search
            = new DepthFirstSearch<NQueensBoard, QueenAction>(new GraphSearch<NQueensBoard, QueenAction>());
            SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation());

        }

        private static void nQueensWithIterativeDeepeningSearch()
        {
            Console.WriteLine("\nNQueensDemo Iterative DS  -->");

            IProblem<NQueensBoard, QueenAction> problem =
                    NQueensFunctions.createIncrementalFormulationProblem(boardSize);
            SearchForActions<NQueensBoard, QueenAction> search = new IterativeDeepeningSearch<NQueensBoard, QueenAction>();
            SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);

            Console.WriteLine();
            Util.printActions(agent.GetActions());
            Util.printInstrumentation(agent.GetInstrumentation());

        }

        private static void nQueensSimulatedAnnealingSearch()
        {
            Console.WriteLine("\nNQueensDemo Simulated Annealing  -->");

            IProblem<NQueensBoard, QueenAction> problem =
                    NQueensFunctions.createCompleteStateFormulationProblem(boardSize, NQueensBoard.Config.QUEENS_IN_FIRST_ROW);

            SimulatedAnnealingSearch<NQueensBoard, QueenAction> search =
                    new SimulatedAnnealingSearch<NQueensBoard, QueenAction>(NQueensFunctions.createAttackingPairsHeuristicFunction(),
                    new Scheduler(20, 0.045, 100));
            SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);

            Console.WriteLine();
            Util.printActions(agent.GetActions());
            Console.WriteLine("Search Outcome=" + search.getOutcome());
            Console.WriteLine("Final State=\n" + search.getLastSearchState());
            Util.printInstrumentation(agent.GetInstrumentation());

        }

        private static void nQueensHillClimbingSearch()
        {
            Console.WriteLine("\nNQueensDemo HillClimbing  -->");

            IProblem<NQueensBoard, QueenAction> problem =
                    NQueensFunctions.createCompleteStateFormulationProblem(boardSize, NQueensBoard.Config.QUEENS_IN_FIRST_ROW);
            HillClimbingSearch<NQueensBoard, QueenAction> search = new HillClimbingSearch<NQueensBoard, QueenAction>
                    (NQueensFunctions.createAttackingPairsHeuristicFunction());
            SearchAgent<NQueensBoard, QueenAction> agent = new SearchAgent<NQueensBoard, QueenAction>(problem, search);

            Console.WriteLine();
            Util.printActions(agent.GetActions());
            Console.WriteLine("Search Outcome=" + search.getOutcome());
            Console.WriteLine("Final State=\n" + search.getLastSearchState());
            Util.printInstrumentation(agent.GetInstrumentation());

        }

        private static void nQueensGeneticAlgorithmSearch()
        {
            Console.WriteLine("\nNQueensDemo GeneticAlgorithm  -->");

            FitnessFunction<int> fitnessFunction = NQueensGenAlgoUtil.getFitnessFunction();
            GoalTest<Individual<int>> goalTest = NQueensGenAlgoUtil.getGoalTest();
            // Generate an initial population
            ISet<Individual<int>> population = new HashSet<Individual<int>>();
            for (int i = 0; i < 50; i++)
            {
                population.Add(NQueensGenAlgoUtil.generateRandomIndividual(boardSize));
            }

            GeneticAlgorithm<int> ga = new GeneticAlgorithm<int>(boardSize,
                    NQueensGenAlgoUtil.getFiniteAlphabetForBoardOfSize(boardSize), 0.15);

            // Run for a set amount of time
            Individual<int> bestIndividual = ga.geneticAlgorithm(population, fitnessFunction, goalTest, 1000L);

            Console.WriteLine("Max Time (1 second) Best Individual=\n"  + NQueensGenAlgoUtil.getBoardForIndividual(bestIndividual));
            Console.WriteLine("Board Size      = " + boardSize);
            Console.WriteLine("# Board Layouts = " + Math.Pow(boardSize, boardSize));
            Console.WriteLine("Fitness         = " + fitnessFunction(bestIndividual));
            Console.WriteLine("Is Goal         = " + goalTest(bestIndividual));
            Console.WriteLine("Population Size = " + ga.getPopulationSize());
            Console.WriteLine("Iterations      = " + ga.getIterations());
            Console.WriteLine("Took            = " + ga.getTimeInMilliseconds() + "ms.");

            // Run till goal is achieved
            bestIndividual = ga.geneticAlgorithm(population, fitnessFunction, goalTest, 0L);

            Console.WriteLine("");
            Console.WriteLine("Goal Test Best Individual=\n" + NQueensGenAlgoUtil.getBoardForIndividual(bestIndividual));
            Console.WriteLine("Board Size      = " + boardSize);
            Console.WriteLine("# Board Layouts = " + Math.Pow(boardSize, boardSize));
            Console.WriteLine("Fitness         = " + fitnessFunction(bestIndividual));
            Console.WriteLine("Is Goal         = " + goalTest(bestIndividual));
            Console.WriteLine("Population Size = " + ga.getPopulationSize());
            Console.WriteLine("Itertions       = " + ga.getIterations());
            Console.WriteLine("Took            = " + ga.getTimeInMilliseconds() + "ms.");


        }
    }
}
