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
    public class LookupPolicy<S, A : Action> : Policy<S, A> {

    private Map<S, A> policy = Factory.CreateMap<S, A>();

    public LookupPolicy(IMap<S, A> aPolicy)
    {
        policy.putAll(aPolicy);
    }

    //
    // START-Policy
     
    public A action(S s)
    {
        return policy.Get(s);
    }

    // END-Policy
    //
}
}
