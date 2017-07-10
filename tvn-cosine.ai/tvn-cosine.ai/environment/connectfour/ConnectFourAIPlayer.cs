using System;
using System.Collections.Generic;
using tvn.cosine.ai.search.adversarial;

namespace tvn.cosine.ai.environment.connectfour
{
    /**
     * Implements an iterative deepening Minimax search with alpha-beta pruning and
     * a special action ordering optimized for the Connect Four game.
     * 
     * @author Ruediger Lunde
     */
    public class ConnectFourAIPlayer : IterativeDeepeningAlphaBetaSearch<ConnectFourState, int, string>
    {
        public ConnectFourAIPlayer(Game<ConnectFourState, int, string> game, int time)
            : base(game, 0.0, 1.0, time)
        { }

        protected override bool isSignificantlyBetter(double newUtility, double utility)
        {
            return newUtility - utility > (utilMax - utilMin) * 0.4;
        }

        protected override bool hasSafeWinner(double resultUtility)
        {
            return Math.Abs(resultUtility - (utilMin + utilMax) / 2) > 0.4 * utilMax - utilMin;
        }

        /**
         * Modifies the super implementation by making safe winner values even more
         * attractive if depth is small.
         */
        protected override double eval(ConnectFourState state, String player)
        {
            double value = base.eval(state, player);
            if (hasSafeWinner(value))
            {
                if (value > (utilMin + utilMax) / 2)
                    value -= state.getMoves() / 1000.0;
                else
                    value += state.getMoves() / 1000.0;
            }
            return value;
        }

        /**
         * Orders actions with respect to the number of potential win positions
         * which profit from the action.
         */
        public override IList<int> orderActions(ConnectFourState state, IList<int> actions, string player, int depth)
        {
            IList<int> result = actions;
            if (depth == 0)
            {
                List<ActionValuePair<int>> actionEstimates = new List<ActionValuePair<int>>(actions.Count);
                foreach (int action in actions)
                    actionEstimates.Add(ActionValuePair<int>.createFor(action, state.analyzePotentialWinPositions(action)));
                actionEstimates.Sort();
                result = new List<int>();
                foreach (ActionValuePair<int> pair in actionEstimates)
                    result.Add(pair.getAction());
            }
            return result;
        }
    }

}
