 namespace aima.core.probability.example;

 
 
import java.util.HashSet;
 
 

import aima.core.environment.cellworld.Cell;
import aima.core.environment.cellworld.CellWorld;
import aima.core.environment.cellworld.CellWorldAction;
import aima.core.probability.mdp.ActionsFunction;
import aima.core.probability.mdp.MarkovDecisionProcess;
import aima.core.probability.mdp.RewardFunction;
import aima.core.probability.mdp.TransitionProbabilityFunction;
import aima.core.probability.mdp.impl.MDP;

/**
 * 
 * @author Ciaran O'Reilly
 * @author Ravi Mohan
 */
public class MDPFactory {

	/**
	 * Constructs an MDP that can be used to generate the utility values
	 * detailed in Fig 17.3.
	 * 
	 * @param cw
	 *            the cell world from figure 17.1.
	 * @return an MDP that can be used to generate the utility values detailed
	 *         in Fig 17.3.
	 */
	public static MarkovDecisionProcess<Cell<double>, CellWorldAction> createMDPForFigure17_3(
			final CellWorld<double> cw) {

		return new MDP<Cell<double>, CellWorldAction>(cw.getCells(),
				cw.getCellAt(1, 1), createActionsFunctionForFigure17_1(cw),
				createTransitionProbabilityFunctionForFigure17_1(cw),
				createRewardFunctionForFigure17_1());
	}

	/**
	 * Returns the allowed actions from a specified cell within the cell world
	 * described in Fig 17.1.
	 * 
	 * @param cw
	 *            the cell world from figure 17.1.
	 * @return the set of actions allowed at a particular cell. This set will be
	 *         empty if at a terminal state.
	 */
	public static ActionsFunction<Cell<double>, CellWorldAction> createActionsFunctionForFigure17_1(
			final CellWorld<double> cw) {
		final ISet<Cell<double>> terminals = new HashSet<Cell<double>>();
		terminals.Add(cw.getCellAt(4, 3));
		terminals.Add(cw.getCellAt(4, 2));

		ActionsFunction<Cell<double>, CellWorldAction> af = new ActionsFunction<Cell<double>, CellWorldAction>() {

			 
			public ISet<CellWorldAction> actions(Cell<double> s) {
				// All actions can be performed in each cell
				// (except terminal states)
				if (terminals.Contains(s)) {
					return Collections.emptySet();
				}
				return CellWorldAction.actions();
			}
		};
		return af;
	}

	/**
	 * Figure 17.1 (b) Illustration of the transition model of the environment:
	 * the 'intended' outcome occurs with probability 0.8, but with probability
	 * 0.2 the agent moves at right angles to the intended direction. A
	 * collision with a wall results in no movement.
	 * 
	 * @param cw
	 *            the cell world from figure 17.1.
	 * @return the transition probability function as described in figure 17.1.
	 */
	public static TransitionProbabilityFunction<Cell<double>, CellWorldAction> createTransitionProbabilityFunctionForFigure17_1(
			final CellWorld<double> cw) {
		TransitionProbabilityFunction<Cell<double>, CellWorldAction> tf = new TransitionProbabilityFunction<Cell<double>, CellWorldAction>() {
			private double[] distribution = new double[] { 0.8, 0.1, 0.1 };

			 
			public double probability(Cell<double> sDelta, Cell<double> s,
					CellWorldAction a) {
				double prob = 0;

				List<Cell<double>> outcomes = possibleOutcomes(s, a);
				for (int i = 0; i < outcomes.Count; ++i) {
					if (sDelta .Equals(outcomes.get(i))) {
						// Note: You have to sum the matches to
						// sDelta as the different actions
						// could have the same effect (i.e.
						// staying in place due to there being
						// no adjacent cells), which increases
						// the probability of the transition for
						// that state.
						prob += distribution[i];
					}
				}

				return prob;
			}
			
			private List<Cell<double>> possibleOutcomes(Cell<double> c,
					CellWorldAction a) {
				// There can be three possible outcomes for the planned action
				List<Cell<double>> outcomes = new List<Cell<double>>();

				outcomes.Add(cw.result(c, a));
				outcomes.Add(cw.result(c, a.getFirstRightAngledAction()));
				outcomes.Add(cw.result(c, a.getSecondRightAngledAction()));

				return outcomes;
			}
		};

		return tf;
	}

	/**
	 * 
	 * @return the reward function which takes the content of the cell as being
	 *         the reward value.
	 */
	public static RewardFunction<Cell<double>> createRewardFunctionForFigure17_1() {
		RewardFunction<Cell<double>> rf = new RewardFunction<Cell<double>>() {
			 
			public double reward(Cell<double> s) {
				return s.getContent();
			}
		};
		return rf;
	}
}
