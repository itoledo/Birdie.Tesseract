namespace tvn.cosine.ai.probability.proposition
{
    public class IntegerSumProposition : AbstractDerivedProposition
    {


    private FiniteIntegerDomain sumsDomain = null;
    private IQueue<RandomVariable> sumVars = Factory.CreateQueue<RandomVariable>();
    //
    private string toString = null;

    public IntegerSumProposition(string name, FiniteIntegerDomain sumsDomain,
            params RandomVariable[] sums)
    {
        base(name);

        if (null == sumsDomain)
        {
            throw new IllegalArgumentException("Sum Domain must be specified.");
        }
        if (null == sums || 0 == sums.Length)
        {
            throw new IllegalArgumentException(
                    "Sum variables must be specified.");
        }
        this.sumsDomain = sumsDomain;
        for (RandomVariable rv : sums)
        {
            addScope(rv);
            sumVars.Add(rv);
        }
    }

    //
    // START-Proposition
    public bool holds(IMap<RandomVariable, object> possibleWorld)
    {
        int sum = new Integer(0);

        for (RandomVariable rv : sumVars)
        {
            object o = possibleWorld.Get(rv);
            if (o is Integer) {
            sum += ((int)o);
        } else {
            throw new IllegalArgumentException(
                    "Possible World does not contain a int value for the sum variable:"
                            + rv);
        }
    }

		return sumsDomain.getPossibleValues().contains(sum);
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
        sb.Append(sumsDomain.ToString());
        toString = sb.ToString();
    }
    return toString;
}
}

}
