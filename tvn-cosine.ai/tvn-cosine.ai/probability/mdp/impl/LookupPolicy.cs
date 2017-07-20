using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;

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
        where A : Action
    {
        private IMap<S, A> policy = Factory.CreateMap<S, A>();

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
