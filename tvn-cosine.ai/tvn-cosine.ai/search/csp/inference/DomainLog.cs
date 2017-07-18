namespace tvn.cosine.ai.search.csp.inference
{
    /**
     * Provides information which might be useful for a caller of a constraint
     * propagation algorithm. It maintains old domains for variables and provides
     * means to restore the initial state of the CSP (before domain reduction
     * started). Additionally, a flag indicates whether an empty domain has been
     * found during propagation.
     * 
     * @author Ruediger Lunde
     * 
     */
    public class DomainLog<VAR : Variable, VAL> : InferenceLog<VAR, VAL> {

    private IQueue<Pair<VAR, Domain<VAL>>> savedDomains;
    private HashSet<VAR> affectedVariables;
    private bool emptyDomainObserved;

    public DomainLog()
    {
        savedDomains = Factory.CreateQueue<>();
        affectedVariables = Factory.CreateSet<>();
    }

    public void clear()
    {
        savedDomains.Clear();
        affectedVariables.Clear();
    }

    /**
	 * Stores the specified domain for the specified variable if a domain has
	 * not yet been stored for the variable.
	 */
    public void storeDomainFor(VAR var, Domain<VAL> domain)
    {
        if (!affectedVariables.contains(var))
        {
            savedDomains.Add(new Pair<>(var, domain));
            affectedVariables.Add(var);
        }
    }

    public void setEmptyDomainFound(bool b)
    {
        emptyDomainObserved = b;
    }

    /**
	 * Can be called after all domain information has been collected to reduce
	 * storage consumption.
	 * 
	 * @return this object, after removing one hashtable.
	 */
    public DomainLog<VAR, VAL> compactify()
    {
        affectedVariables = null;
        return this;
    }

     
    public bool isEmpty()
    {
        return savedDomains.isEmpty();
    }

     
    public void undo(CSP<VAR, VAL> csp)
    {
        for (Pair<VAR, Domain<VAL>> pair : getSavedDomains())
            csp.setDomain(pair.getFirst(), pair.getSecond());
    }

     
    public bool inconsistencyFound()
    {
        return emptyDomainObserved;
    }

    private IQueue<Pair<VAR, Domain<VAL>>> getSavedDomains()
    {
        return savedDomains;
    }

    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        for (Pair<VAR, Domain<VAL>> pair : savedDomains)
            result.Append(pair.getFirst()).Append("=").Append(pair.getSecond()).Append(" ");
        if (emptyDomainObserved)
            result.Append("!");
        return result.ToString();
    }
}

}
