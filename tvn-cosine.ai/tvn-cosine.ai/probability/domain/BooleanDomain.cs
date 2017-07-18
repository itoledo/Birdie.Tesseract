namespace tvn.cosine.ai.probability.domain
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 486.
     * 
     * A Boolean random variable has the domain {true,false}.
     * 
     * @author Ciaran O'Reilly.
     */
    public class BooleanDomain : AbstractFiniteDomain
    {


    private static ISet<Boolean> _possibleValues = null;
    static {
		// Keep consistent order
		_possibleValues = Factory.CreateSet<Boolean>();
		_possibleValues.Add(Boolean.TRUE);
		_possibleValues.Add(Boolean.FALSE);
		// Ensure cannot be modified
		_possibleValues = Factory.CreateReadOnlySet<>(_possibleValues);
	}

public BooleanDomain()
{
    indexPossibleValues(_possibleValues);
}

//
// START-Domain
 
    public int size()
{
    return 2;
}

 
    public bool isOrdered()
{
    return false;
}

// END-Domain
//

//
// START-DiscreteDomain
 
    public ISet<Boolean> getPossibleValues()
{
    return _possibleValues;
}

// END-DiscreteDomain
//

 
    public override bool Equals(object o)
{
    return o is BooleanDomain;
}

 
    public override int GetHashCode()
{
    return _possibleValues.GetHashCode();
}
}

}
