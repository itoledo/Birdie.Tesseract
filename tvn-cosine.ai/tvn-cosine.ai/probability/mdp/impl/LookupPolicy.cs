 namespace aima.core.probability.mdp.impl;

 
 

 
import aima.core.probability.mdp.Policy;

/**
 * Default implementation of the Policy interface using an underlying Map to
 * look up an action associated with a state.
 * 
 * @param <S>
 *            the state type.
 * @param <A>
 *            the action type.
 * 
 * @author Ciaran O'Reilly
 */
public class LookupPolicy<S, A : Action> : Policy<S, A> {
	private IDictionary<S, A> policy = new Dictionary<S, A>();

	public LookupPolicy(IDictionary<S, A> aPolicy) {
		policy.putAll(aPolicy);
	}

	//
	// START-Policy
	 
	public A action(S s) {
		return policy.get(s);
	}

	// END-Policy
	//
}
