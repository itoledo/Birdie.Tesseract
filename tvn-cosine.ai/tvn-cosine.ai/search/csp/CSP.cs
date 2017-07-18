namespace tvn.cosine.ai.search.csp
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Ed.): Section 6.1, Page 202.<br>
     * <br>
     * A constraint satisfaction problem or CSP consists of three components, X, D,
     * and C:
     * <ul>
     * <li>X is a set of variables, {X1, ... ,Xn}.</li>
     * <li>D is a set of domains, {D1, ... ,Dn}, one for each variable.</li>
     * <li>C is a set of constraints that specify allowable combinations of values.</li>
     * </ul>
     *
     * @param <VAR> Type which is used to represent variables
     * @param <VAL> Type which is used to represent the values in the domains
     *
     * @author Ruediger Lunde
     */
    public class CSP<VAR : Variable, VAL> : Cloneable
    {

    private IQueue<VAR> variables;
    private IQueue<Domain<VAL>> domains;
    private IQueue<Constraint<VAR, VAL>> constraints;

    /**
     * Lookup, which maps a variable to its index in the list of variables.
     */
    private IMap<Variable, int> varIndexHash;
    /**
     * Constraint network. Maps variables to those constraints in which they
     * participate.
     */
    private IMap<Variable, IQueue<Constraint<VAR, VAL>>> cnet;

    /**
     * Creates a new CSP.
     */
    public CSP()
    {
        variables = Factory.CreateQueue<>();
        domains = Factory.CreateQueue<>();
        constraints = Factory.CreateQueue<>();
        varIndexHash = Factory.CreateMap<>();
        cnet = Factory.CreateMap<>();
    }

    /**
     * Creates a new CSP.
     */
    public CSP(IQueue<VAR> vars)
    {
        this();
        vars.forEach(this::addVariable);
    }

    /**
     * Adds a new variable only if its name is new.
     */
    protected void addVariable(VAR var)
    {
        if (!varIndexHash.containsKey(var))
        {
            Domain<VAL> emptyDomain = new Domain<>(Collections.emptyList());
            variables.Add(var);
            domains.Add(emptyDomain);
            varIndexHash.Put(var, variables.size() - 1);
            cnet.Put(var, Factory.CreateQueue<>());
        }
        else
        {
            throw new IllegalArgumentException("Variable with same name already exists.");
        }
    }

    public IQueue<VAR> getVariables()
    {
        return Factory.CreateReadOnlyQueue<>(variables);
    }

    public int indexOf(Variable var)
    {
        return varIndexHash.Get(var);
    }

    public void setDomain(VAR var, Domain<VAL> domain)
    {
        domains.set(indexOf(var), domain);
    }

    public Domain<VAL> getDomain(Variable var)
    {
        return domains.Get(varIndexHash.Get(var));
    }

    /**
     * Replaces the domain of the specified variable by new domain, which
     * contains all values of the old domain except the specified value.
     */
    public bool removeValueFromDomain(VAR var, VAL value)
    {
        Domain<VAL> currDomain = getDomain(var);
        IQueue<VAL> values = Factory.CreateQueue<>(currDomain.size());
        for (VAL v : currDomain)
            if (!v.Equals(value))
                values.Add(v);
        if (values.size() < currDomain.size())
        {
            setDomain(var, new Domain<>(values));
            return true;
        }
        return false;
    }

    public void addConstraint(Constraint<VAR, VAL> constraint)
    {
        constraints.Add(constraint);
        for (VAR var : constraint.getScope())
            cnet.Get(var).Add(constraint);
    }

    public bool removeConstraint(Constraint<VAR, VAL> constraint)
    {
        bool result = constraints.Remove(constraint);
        if (result)
            for (VAR var : constraint.getScope())
                cnet.Get(var).Remove(constraint);
        return result;
    }

    public IQueue<Constraint<VAR, VAL>> getConstraints()
    {
        return constraints;
    }

    /**
     * Returns all constraints in which the specified variable participates.
     */
    public IQueue<Constraint<VAR, VAL>> getConstraints(Variable var)
    {
        return cnet.Get(var);
    }

    /**
     * Returns for binary constraints the other variable from the scope.
     *
     * @return a variable or null for non-binary constraints.
     */
    public VAR getNeighbor(VAR var, Constraint<VAR, VAL> constraint)
    {
        IQueue<VAR> scope = constraint.getScope();
        if (scope.size() == 2)
        {
            if (var.Equals(scope.Get(0)))
                return scope.Get(1);
            else if (var.Equals(scope.Get(1)))
                return scope.Get(0);
        }
        return null;
    }

    /**
     * Returns a copy which contains a copy of the domains list and is in all
     * other aspects a flat copy of this.
     */
    @SuppressWarnings("unchecked")
    public CSP<VAR, VAL> copyDomains()
    {
        CSP<VAR, VAL> result;
        try
        {
            result = (CSP<VAR, VAL>)clone();
            result.domains = Factory.CreateQueue<>(domains.size());
            result.domains.addAll(domains);
        }
        catch (CloneNotSupportedException e)
        {
            throw new UnsupportedOperationException("Could not copy domains.");
        }
        return result;
    }
}
}
