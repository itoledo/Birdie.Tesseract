using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.datastructures;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.environment.nqueens
{
    /**
     * Provides useful functions for two versions of the n-queens problem. The
     * incremental formulation and the complete-state formulation share the same
     * RESULT function but use different ACTIONS functions.
     *
     * @author Ruediger Lunde
     * @author Ciaran O'Reilly
     */
    public class NQueensFunctions
    { 
        public static Problem<NQueensBoard, QueenAction> createIncrementalFormulationProblem(int boardSize)
        {
            return new GeneralProblem<NQueensBoard, QueenAction>(new NQueensBoard(boardSize),
                NQueensFunctions.getIFActions,
                NQueensFunctions.getResult,
                NQueensFunctions.testGoal);
        }

        public static Problem<NQueensBoard, QueenAction> createCompleteStateFormulationProblem
                (int boardSize, NQueensBoard.Config config)
        {
            return new GeneralProblem<NQueensBoard, QueenAction>(new NQueensBoard(boardSize, config),
                NQueensFunctions.getCSFActions,
                NQueensFunctions.getResult,
                NQueensFunctions.testGoal);
        }

        /**
         * Implements an ACTIONS function for the incremental formulation of the
         * n-queens problem.
         * <p>
         * Assumes that queens are placed column by column, starting with an empty
         * board, and provides queen placing actions for all non-attacked positions
         * of the first free column.
         */
        public static IQueue<QueenAction> getIFActions(NQueensBoard state)
        {
            IQueue<QueenAction> actions = Factory.CreateQueue<QueenAction>();

            int numQueens = state.getNumberOfQueensOnBoard();
            int boardSize = state.getSize();
            for (int i = 0; i < boardSize; i++)
            {
                XYLocation newLocation = new XYLocation(numQueens, i);
                if (!(state.isSquareUnderAttack(newLocation)))
                {
                    actions.Add(new QueenAction(QueenAction.PLACE_QUEEN, newLocation));
                }
            }
            return actions;
        }

        /**
         * Implements an ACTIONS function for the complete-state formulation of the
         * n-queens problem.
         * <p>
         * Assumes exactly one queen in each column and provides all possible queen
         * movements in vertical direction as actions.
         */
        public static IQueue<QueenAction> getCSFActions(NQueensBoard state)
        {
            IQueue<QueenAction> actions = Factory.CreateQueue<QueenAction>();
            for (int i = 0; i < state.getSize(); i++)
                for (int j = 0; j < state.getSize(); j++)
                {
                    XYLocation loc = new XYLocation(i, j);
                    if (!state.queenExistsAt(loc))
                        actions.Add(new QueenAction(QueenAction.MOVE_QUEEN, loc));
                }
            return actions;
        }

        /**
         * Implements a RESULT function for the n-queens problem.
         * Supports queen placing, queen removal, and queen movement actions.
         */
        public static NQueensBoard getResult(NQueensBoard state, QueenAction action)
        {
            NQueensBoard result = new NQueensBoard(state.getSize());
            result.setQueensAt(state.getQueenPositions());
            if (action.getName().Equals(QueenAction.PLACE_QUEEN))
                result.addQueenAt(action.getLocation());
            else if (action.getName().Equals(QueenAction.REMOVE_QUEEN))
                result.removeQueenFrom(action.getLocation());
            else if (action.getName().Equals(QueenAction.MOVE_QUEEN))
                result.moveQueenTo(action.getLocation());
            // if action is not understood or is a NoOp
            // the result will be the current state.
            return result;
        }

        /**
         * Implements a GOAL-TEST for the n-queens problem.
         */
        public static bool testGoal(NQueensBoard state)
        {
            return state.getNumberOfQueensOnBoard() == state.getSize() && state.getNumberOfAttackingPairs() == 0;
        }


        /**
         * Estimates the distance to goal by the number of attacking pairs of queens on
         * the board.
         */
        public static ToDoubleFunction<Node<NQueensBoard, QueenAction>> createAttackingPairsHeuristicFunction()
        {
            return (node) => node.getState().getNumberOfAttackingPairs();
        }
    }
}
