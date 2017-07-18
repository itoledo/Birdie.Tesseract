namespace tvn.cosine.ai.probability.proposition
{
    public class SubsetProposition : AbstractDerivedProposition
    {


    private FiniteDomain subsetDomain = null;
    private RandomVariable varSubsetOf = null;
    //
    private string toString = null;

    public SubsetProposition(string name, FiniteDomain subsetDomain,
            RandomVariable ofVar)
    {
        base(name);

        if (null == subsetDomain)
        {
            throw new IllegalArgumentException("Sum Domain must be specified.");
        }
        this.subsetDomain = subsetDomain;
        this.varSubsetOf = ofVar;
        addScope(this.varSubsetOf);
    }

    //
    // START-Proposition
    public bool holds(IMap<RandomVariable, object> possibleWorld)
    {
        return subsetDomain.getPossibleValues().contains(
                possibleWorld.Get(varSubsetOf));
    }

    // END-Proposition
    //

     
    public override string ToString()
    {
        if (null == toString)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(getDerivedName());
            sb.Append(" = ");
            sb.Append(subsetDomain.ToString());
            toString = sb.ToString();
        }
        return toString;
    }
}

}
