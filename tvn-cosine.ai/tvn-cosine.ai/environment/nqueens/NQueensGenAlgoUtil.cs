using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.local;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.environment.nqueens
{
    /**
     * A class whose purpose is to provide static utility methods for solving the
     * n-queens problem with genetic algorithms. This includes fitness function,
     * goal test, random creation of individuals and convenience methods for
     * translating between between an NQueensBoard representation and the  int  list
     * representation used by the GeneticAlgorithm.
     * 
     * @author Ciaran O'Reilly
     * @author Ruediger Lunde
     * 
     */
    public class NQueensGenAlgoUtil
    {

        public static FitnessFunction<int> getFitnessFunction()
        {
            return new NQueensFitnessFunction();
        }

        public static GoalTest<Individual<int>> getGoalTest()
        {
            return NQueensFitnessFunction.NQueensGenAlgoGoalTest;
        }


        public static Individual<int> generateRandomIndividual(int boardSize)
        {
            IList<int> individualRepresentation = new List<int>();
            for (int i = 0; i < boardSize; i++)
            {
                individualRepresentation.Add(new Random().Next(boardSize));
            }
            return new Individual<int>(individualRepresentation);
        }

        public static ICollection<int> getFiniteAlphabetForBoardOfSize(int size)
        {
            ICollection<int> fab = new List<int>();

            for (int i = 0; i < size; i++)
            {
                fab.Add(i);
            }

            return fab;
        }

        public class NQueensFitnessFunction : FitnessFunction<int>
        {
            public static GoalTest<Individual<int>> NQueensGenAlgoGoalTest
            {
                get
                {
                    return (state) =>
                    {
                        return NQueensFunctions.testGoal(getBoardForIndividual(state));
                    };
                }
            }

            public double apply(Individual<int> individual)
            {
                double fitness = 0;

                NQueensBoard board = getBoardForIndividual(individual);
                int boardSize = board.getSize();

                // Calculate the number of non-attacking pairs of queens (refer to
                // AIMA
                // page 117).
                IList<XYLocation> qPositions = board.getQueenPositions();
                for (int fromX = 0; fromX < (boardSize - 1); fromX++)
                {
                    for (int toX = fromX + 1; toX < boardSize; toX++)
                    {
                        int fromY = qPositions[fromX].Y;
                        bool nonAttackingPair = true;
                        // Check right beside
                        int toY = fromY;
                        if (board.queenExistsAt(new XYLocation(toX, toY)))
                        {
                            nonAttackingPair = false;
                        }
                        // Check right and above
                        toY = fromY - (toX - fromX);
                        if (toY >= 0)
                        {
                            if (board.queenExistsAt(new XYLocation(toX, toY)))
                            {
                                nonAttackingPair = false;
                            }
                        }
                        // Check right and below
                        toY = fromY + (toX - fromX);
                        if (toY < boardSize)
                        {
                            if (board.queenExistsAt(new XYLocation(toX, toY)))
                            {
                                nonAttackingPair = false;
                            }
                        }

                        if (nonAttackingPair)
                        {
                            fitness += 1.0;
                        }
                    }
                }

                return fitness;
            }
        }

        public static NQueensBoard getBoardForIndividual(Individual<int> individual)
        {
            int boardSize = individual.length();
            NQueensBoard board = new NQueensBoard(boardSize);
            for (int i = 0; i < boardSize; i++)
            {
                int pos = individual.getRepresentation()[i];
                board.addQueenAt(new XYLocation(i, pos));
            }
            return board;
        }
    }
}
