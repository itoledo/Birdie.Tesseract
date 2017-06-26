 namespace aima.core.probability.mdp.impl;

 

 
import aima.core.probability.mdp.ActionsFunction;
import aima.core.probability.mdp.MarkovDecisionProcess;
import aima.core.probability.mdp.RewardFunction;
import aima.core.probability.mdp.TransitionProbabilityFunction;

/**
 * Default implementation of the MarkovDecisionProcess<S, A> interface.
 * 
 * @param <S>
 *            the state type.
 * @param <A>
 *            the action type.
 * 
 * @author Ciaran O'Reilly
 * @author Ravi Mohan
 */
public class MDP<S, A : Action> : MarkovDecisionProcess<S, A> {
	private ISet<S> states = null;
	private S initialState = null;
	private ActionsFunction<S, A> actionsFunction = null;
	private TransitionProbabilityFunction<S, A> transitionProbabilityFunction = null;
	private RewardFunction<S> rewardFunction = null;

	public MDP(ISet<S> states, S initialState,
			ActionsFunction<S, A> actionsFunction,
			TransitionProbabilityFunction<S, A> transitionProbabilityFunction,
			RewardFunction<S> rewardFunction) {
		this.states = states;
		this.initialState = initialState;
		this.actionsFunction = actionsFunction;
		this.transitionProbabilityFunction = transitionProbabilityFunction;
		this.rewardFunction = rewardFunction;
	}

	//
	// START-MarkovDecisionProcess
	 
	public ISet<S> states() {
		return states;
	}

	 
	public S getInitialState() {
		return initialState;
	}

	 
	public ISet<A> actions(S s) {
		return actionsFunction.actions(s);
	}

	 
	public double transitionProbability(S sDelta, S s, A a) {
		return transitionProbabilityFunction.probability(sDelta, s, a);
	}

	 
	public double reward(S s) {
		return rewardFunction.reward(s);
	}

	// END-MarkovDecisionProcess
	//
}
