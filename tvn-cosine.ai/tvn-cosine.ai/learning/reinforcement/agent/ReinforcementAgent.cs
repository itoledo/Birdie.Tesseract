 namespace aima.core.learning.reinforcement.agent;

 

 
 
import aima.core.agent.impl.AbstractAgent;
 
import aima.core.learning.reinforcement.PerceptStateReward;

/**
 * An abstract base class for creating reinforcement based agents.
 * 
 * @param <S>
 *            the state type.
 * @param <A>
 *            the action type.
 * 
 * @author Ciaran O'Reilly
 * @author Ravi Mohan
 */
public abstract class ReinforcementAgent<S, A : Action> extends
		AbstractAgent {

	/**
	 * Default Constructor.
	 */
	public ReinforcementAgent() {
	}

	/**
	 * Map the given percept to an Agent action.
	 * 
	 * @param percept
	 *            a percept indicating the current state s' and reward signal r'
	 * @return the action to take.
	 */
	public abstract A execute(PerceptStateReward<S> percept);
	
	/**
	 * Get a vector of the currently calculated utilities for states of type S
	 * in the world.
	 * 
	 * @return a Map of the currently learned utility values for the states in
	 *         the environment (Note: this map may not contain all of the states
	 *         in the environment, i.e. the agent has not seen them yet).
	 */
	public abstract IDictionary<S, double> getUtility();
	
	/**
	 * Reset the agent back to its initial state before it has learned anything
	 * about its environment.
	 */
	public abstract void reset();

	@SuppressWarnings("unchecked")
	 
	public Action execute(Percept p) {
		if (p is PerceptStateReward<?>) {
			Action a = execute((PerceptStateReward<S>) p);
			if (null == a) {
				a = NoOpAction.NO_OP;
				setAlive(false);
			}
			return a;
		}
		throw new ArgumentException(
				"Percept passed in must be a PerceptStateReward");
	}
}
