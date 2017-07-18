namespace tvn.cosine.ai.probability.proposition
{
    public abstract class AbstractProposition : Proposition
    {


    private LinkedHashSet<RandomVariable> scope = Factory.CreateSet<RandomVariable>();
    private LinkedHashSet<RandomVariable> unboundScope = Factory.CreateSet<RandomVariable>();

    public AbstractProposition()
    {

    }

    //
    // START-Proposition
    public ISet<RandomVariable> getScope()
    {
        return scope;
    }

    public ISet<RandomVariable> getUnboundScope()
    {
        return unboundScope;
    }

    public abstract bool holds(IMap<RandomVariable, object> possibleWorld);

    // END-Proposition
    //

    //
    // Protected Methods
    //
    protected void addScope(RandomVariable var)
    {
        scope.Add(var);
    }

    protected void addScope(Collection<RandomVariable> vars)
    {
        scope.addAll(vars);
    }

    protected void addUnboundScope(RandomVariable var)
    {
        unboundScope.Add(var);
    }

    protected void addUnboundScope(Collection<RandomVariable> vars)
    {
        unboundScope.addAll(vars);
    }
}

}
