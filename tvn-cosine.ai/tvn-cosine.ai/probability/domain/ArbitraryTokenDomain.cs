namespace tvn.cosine.ai.probability.domain
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 486.
     * 
     * As in CSPs, domains can be sets of arbitrary tokens; we might choose the
     * domain of <i>Age</i> to be {<i>juvenile,teen,adult</i>} and the domain of
     * <i>Weather</i> might be {<i>sunny,rain,cloudy,snow</i>}.
     * 
     * @author Ciaran O'Reilly
     */
    public class ArbitraryTokenDomain : AbstractFiniteDomain
    {


    private ISet<Object> possibleValues = null;
    private bool ordered = false;

    public ArbitraryTokenDomain(params object[] pValues)
    {
        this(false, pValues);
    }

    public ArbitraryTokenDomain(bool ordered, params object[] pValues)
    {
        this.ordered = ordered;
        // Keep consistent order
        possibleValues = Factory.CreateSet<Object>();
        for (object v : pValues)
        {
            possibleValues.Add(v);
        }
        // Ensure cannot be modified
        possibleValues = Factory.CreateReadOnlySet<>(possibleValues);

        indexPossibleValues(possibleValues);
    }

    //
    // START-Domain

     
    public int size()
    {
        return possibleValues.size();
    }

     
    public bool isOrdered()
    {
        return ordered;
    }

    // END-Domain
    //

    //
    // START-FiniteDomain
     
    public ISet<Object> getPossibleValues()
    {
        return possibleValues;
    }

    // END-finiteDomain
    //

     
    public override bool Equals(object o)
    {

        if (this == o)
        {
            return true;
        }
        if (!(o is ArbitraryTokenDomain)) {
            return false;
        }

        ArbitraryTokenDomain other = (ArbitraryTokenDomain)o;

        return this.possibleValues.Equals(other.possibleValues);
    }

     
    public override int GetHashCode()
    {
        return possibleValues.GetHashCode();
    }
}

}
