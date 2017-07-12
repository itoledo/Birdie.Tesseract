using System.Collections.Generic;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.probability.mdp.impl
{
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
    public class LookupPolicy<S, A> : Policy<S, A>
        where A : IAction
    {

        private IDictionary<S, A> policy = new Dictionary<S, A>();

        public LookupPolicy(IDictionary<S, A> aPolicy)
        {
            foreach (var v in aPolicy)
                policy.Add(v);
        }

        //
        // START-Policy 
        public A action(S s)
        {
            return policy[s];
        }

        // END-Policy
        //
    }
}
