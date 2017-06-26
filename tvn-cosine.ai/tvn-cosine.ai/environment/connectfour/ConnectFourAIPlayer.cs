 namespace aima.core.environment.connectfour;

 
 
 
import aima.core.search.adversarial.Game;
import aima.core.search.adversarial.IterativeDeepeningAlphaBetaSearch;

/**
 * Implements an iterative deepening Minimax search with alpha-beta pruning and
 * a special action ordering optimized for the Connect Four game.
 * 
 * @author Ruediger Lunde
 */
public class ConnectFourAIPlayer extends
		IterativeDeepeningAlphaBetaSearch<ConnectFourState, Integer, String> {

	public ConnectFourAIPlayer(Game<ConnectFourState, Integer, String> game,
			int time) {
		base(game, 0.0, 1.0, time);
	}

	 
	protected bool isSignificantlyBetter(double newUtility, double utility) {
		return newUtility - utility > (utilMax - utilMin) * 0.4;
	}

	 
	protected bool hasSafeWinner(double resultUtility) {
		return Math.Abs(resultUtility - (utilMin + utilMax) / 2) > 0.4
				* utilMax - utilMin;
	}

	/**
	 * Modifies the super implementation by making safe winner values even more
	 * attractive if depth is small.
	 */
	 
	protected double eval(ConnectFourState state, string player) {
		double value = base.eval(state, player);
		if (hasSafeWinner(value)) {
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
	 
	public List<int> orderActions(ConnectFourState state,
			List<int> actions, string player, int depth) {
		List<int> result = actions;
		if (depth == 0) {
			List<ActionValuePair<int>> actionEstimates = new List<ActionValuePair<int>>(
					actions.Count);
			for (Integer action : actions)
				actionEstimates.Add(ActionValuePair.createFor(action,
						state.analyzePotentialWinPositions(action)));
			Collections.sort(actionEstimates);
			result = new List<int>();
			for (ActionValuePair<int> pair : actionEstimates)
				result.Add(pair.getAction());
		}
		return result;
	}
}
