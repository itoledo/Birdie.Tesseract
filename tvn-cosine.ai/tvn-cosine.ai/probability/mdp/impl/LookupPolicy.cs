using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;

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
        private IMap<S, A> policy = CollectionFactory.CreateInsertionOrderedMap<S, A>();

        public LookupPolicy(IMap<S, A> aPolicy)
        {
            policy.PutAll(aPolicy);
        }

        public A action(S s)
        {
            return policy.Get(s);
        }
    }
}
