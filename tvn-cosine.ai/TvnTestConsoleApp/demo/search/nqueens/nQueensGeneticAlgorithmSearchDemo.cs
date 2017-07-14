using System;
using System.Collections.Generic; 
using tvn.cosine.ai.environment.nqueens;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.local;

namespace TvnTestConsoleApp.demo.search.nqueens
{
    class nQueensGeneticAlgorithmSearchDemo
    {
        public static void Main(params string[] args)
        {
            Console.WriteLine("\nNQueensDemo GeneticAlgorithm  -->");

            nQueensGeneticAlgorithmSearch();
            Console.WriteLine("Complete, press <ENTER> to quit");
            Console.ReadLine();
        }

        private static void nQueensGeneticAlgorithmSearch()
        {

            FitnessFunction<int> fitnessFunction = NQueensGenAlgoUtil.getFitnessFunction();
            GoalTest<Individual<int>> goalTest = NQueensGenAlgoUtil.getGoalTest();
            // Generate an initial population
            ISet<Individual<int>> population = new HashSet<Individual<int>>();
            for (int i = 0; i < 50; i++)
            {
                population.Add(NQueensGenAlgoUtil.generateRandomIndividual(Util.boardSize));
            }

            GeneticAlgorithm<int> ga = new GeneticAlgorithm<int>(Util.boardSize,  NQueensGenAlgoUtil.getFiniteAlphabetForBoardOfSize(Util.boardSize), 0.15);

            // Run for a set amount of time
            Individual<int> bestIndividual = ga.geneticAlgorithm(population, fitnessFunction, goalTest, 1000L);

            Console.WriteLine("Max Time (1 second) Best Individual=\n" + NQueensGenAlgoUtil.getBoardForIndividual(bestIndividual));
            Console.WriteLine("Board Size      = " + Util.boardSize);
            Console.WriteLine("# Board Layouts = " + Math.Pow(Util.boardSize, Util.boardSize));
            Console.WriteLine("Fitness         = " + fitnessFunction(bestIndividual));
            Console.WriteLine("Is Goal         = " + goalTest(bestIndividual));
            Console.WriteLine("Population Size = " + ga.getPopulationSize());
            Console.WriteLine("Iterations      = " + ga.getIterations());
            Console.WriteLine("Took            = " + ga.getTimeInMilliseconds() + "ms.");

            // Run till goal is achieved
            bestIndividual = ga.geneticAlgorithm(population, fitnessFunction, goalTest, 0L);

            Console.WriteLine("");
            Console.WriteLine("Goal Test Best Individual=\n" + NQueensGenAlgoUtil.getBoardForIndividual(bestIndividual));
            Console.WriteLine("Board Size      = " + Util.boardSize);
            Console.WriteLine("# Board Layouts = " + Math.Pow(Util.boardSize, Util.boardSize));
            Console.WriteLine("Fitness         = " + fitnessFunction(bestIndividual));
            Console.WriteLine("Is Goal         = " + goalTest(bestIndividual));
            Console.WriteLine("Population Size = " + ga.getPopulationSize());
            Console.WriteLine("Itertions       = " + ga.getIterations());
            Console.WriteLine("Took            = " + ga.getTimeInMilliseconds() + "ms."); 
        }
    }
}
