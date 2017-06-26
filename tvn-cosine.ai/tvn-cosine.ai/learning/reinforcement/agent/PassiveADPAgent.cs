 namespace aima.core.learning.reinforcement.agent;

 
 
 
 

 
import aima.core.learning.reinforcement.PerceptStateReward;
import aima.core.probability.mdp.ActionsFunction;
import aima.core.probability.mdp.PolicyEvaluation;
import aima.core.probability.mdp.RewardFunction;
import aima.core.probability.mdp.TransitionProbabilityFunction;
import aima.core.probability.mdp.impl.MDP;
import aima.core.util.FrequencyCounter;
import aima.core.util.datastructure.Pair;

/**
 * Artificial Intelligence A Modern Approach (3rd Edition): page 834.<br>
 * <br>
 * 
 * <pre>
 * function PASSIVE-ADP-AGENT(percept) returns an action
 *   inputs: percept, a percept indicating the current state s' and reward signal r'
 *   persistent: &pi;, a fixed policy
 *               mdp, an MDP with model P, rewards R, discount &gamma;
 *               U, a table of utilities, initially empty
 *               N<sub>sa</sub>, a table of frequencies for state-action pairs, initially zero
 *               N<sub>s'|sa</sub>, a table of outcome frequencies give state-action pairs, initially zero
 *               s, a, the previous state and action, initially null
 *               
 *   if s' is new then U[s'] <- r'; R[s'] <- r'
 *   if s is not null then
 *        increment N<sub>sa</sub>[s,a] and N<sub>s'|sa</sub>[s',s,a]
 *        for each t such that N<sub>s'|sa</sub>[t,s,a] is nonzero do
 *            P(t|s,a) <-  N<sub>s'|sa</sub>[t,s,a] / N<sub>sa</sub>[s,a]
 *   U <- POLICY-EVALUATION(&pi;, U, mdp)
 *   if s'.TERMINAL? then s,a <- null else s,a <- s',&pi;[s']
 *   return a
 * </pre>
 * 
 * Figure 21.2 A passive reinforcement learning agent based on adaptive dynamic
 * programming. The POLICY-EVALUATION function solves the fixed-policy Bellman
 * equations, as described on page 657.
 * 
 * @param <S>
 *            the state type.
 * @param <A>
 *            the action type.
 * 
 * @author Ciaran O'Reilly
 * @author Ravi Mohan
 * 
 */
public class PassiveADPAgent<S, A : Action> extends
		ReinforcementAgent<S, A> {
	// persistent: &pi;, a fixed policy
	private IDictionary<S, A> pi = new Dictionary<S, A>();
	// mdp, an MDP with model P, rewards R, discount &gamma;
	private MDP<S, A> mdp = null;
	private IDictionary<Pair<S, Pair<S, A>>, double> P = new Dictionary<Pair<S, Pair<S, A>>, double>();
	private IDictionary<S, double> R = new Dictionary<S, double>();
	private PolicyEvaluation<S, A> policyEvaluation = null;
	// U, a table of utilities, initially empty
	private IDictionary<S, double> U = new Dictionary<S, double>();
	// N<sub>sa</sub>, a table of frequencies for state-action pairs, initially
	// zero
	private FrequencyCounter<Pair<S, A>> Nsa = new FrequencyCounter<Pair<S, A>>();
	// N<sub>s'|sa</sub>, a table of outcome frequencies give state-action
	// pairs, initially zero
	private FrequencyCounter<Pair<S, Pair<S, A>>> NsDelta_sa = new FrequencyCounter<Pair<S, Pair<S, A>>>();
	// s, a, the previous state and action, initially null
	private S s = null;
	private A a = null;

	/**
	 * Constructor.
	 * 
	 * @param fixedPolicy
	 *            &pi; a fixed policy.
	 * @param states
	 *            the possible states in the world (i.e. fully observable).
	 * @param initialState
	 *            the initial state for the agent.
	 * @param actionsFunction
	 *            a function that lists the legal actions from a state.
	 * @param policyEvaluation
	 *            a function for evaluating a policy.
	 */
	public PassiveADPAgent(IDictionary<S, A> fixedPolicy, ISet<S> states,
			S initialState, ActionsFunction<S, A> actionsFunction,
			PolicyEvaluation<S, A> policyEvaluation) {
		this.pi.putAll(fixedPolicy);
		this.mdp = new MDP<S, A>(states, initialState, actionsFunction,
				new TransitionProbabilityFunction<S, A>() {
					public double probability(S sDelta, S s, A a) {
						Double p = P.get(new Pair<S, Pair<S, A>>(sDelta,
								new Pair<S, A>(s, a)));

						return null == p ? 0.0 : p.doubleValue();
					}
				}, new RewardFunction<S>() {
					public double reward(S s) {
						return R.get(s);
					}
				});
		this.policyEvaluation = policyEvaluation;
	}

	/**
	 * Passive reinforcement learning based on adaptive dynamic programming.
	 * 
	 * @param percept
	 *            a percept indicating the current state s' and reward signal
	 *            r'.
	 * @return an action
	 */
	 
	public A execute(PerceptStateReward<S> percept) {
		// if s' is new then U[s'] <- r'; R[s'] <- r'
		S sDelta = percept.state();
		double rDelta = percept.reward();
		if (!U.ContainsKey(sDelta)) {
			U.Add(sDelta, rDelta);
			R.Add(sDelta, rDelta);
		}
		// if s is not null then
		if (null != s) {
			// increment N<sub>sa</sub>[s,a] and N<sub>s'|sa</sub>[s',s,a]
			Pair<S, A> sa = new Pair<S, A>(s, a);
			Nsa.incrementFor(sa);
			NsDelta_sa.incrementFor(new Pair<S, Pair<S, A>>(sDelta, sa));
			// for each t such that N<sub>s'|sa</sub>[t,s,a] is nonzero do
			for (S t : mdp.states()) {
				Pair<S, Pair<S, A>> t_sa = new Pair<S, Pair<S, A>>(t, sa);
				if (0 != NsDelta_sa.getCount(t_sa)) {
					// P(t|s,a) <- N<sub>s'|sa</sub>[t,s,a] /
					// N<sub>sa</sub>[s,a]
					P.Add(t_sa, NsDelta_sa.getCount(t_sa).doubleValue()
							/ Nsa.getCount(sa).doubleValue());
				}
			}
		}
		// U <- POLICY-EVALUATION(&pi;, U, mdp)
		U = policyEvaluation.evaluate(pi, U, mdp);
		// if s'.TERMINAL? then s,a <- null else s,a <- s',&pi;[s']
		if (isTerminal(sDelta)) {
			s = null;
			a = null;

		} else {
			s = sDelta;
			a = pi.get(sDelta);
		}
		// return a
		return a;
	}

	 
	public IDictionary<S, double> getUtility() {
		return Collections.unmodifiableMap(U);
	}

	 
	public void reset() {
		P.Clear();
		R.Clear();
		U = new Dictionary<S, double>();
		Nsa.Clear();
		NsDelta_sa.Clear();
		s = null;
		a = null;
	}

	//
	// PRIVATE METHODS
	//
	private bool isTerminal(S s) {
		bool terminal = false;
		if (0 == mdp.actions(s).Count) {
			// No actions possible in state is considered terminal.
			terminal = true;
		}
		return terminal;
	}
}
