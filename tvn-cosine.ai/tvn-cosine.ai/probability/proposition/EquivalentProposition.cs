namespace tvn.cosine.ai.probability.proposition
{
    public class EquivalentProposition : AbstractDerivedProposition
    {
    //
    private string toString = null;

    public EquivalentProposition(string name, params RandomVariable[] equivs)
    {
        base(name);

        if (null == equivs || 1 >= equivs.Length)
        {
            throw new IllegalArgumentException(
                    "Equivalent variables must be specified.");
        }
        for (RandomVariable rv : equivs)
        {
            addScope(rv);
        }
    }

    //
    // START-Proposition
    public bool holds(IMap<RandomVariable, object> possibleWorld)
    {
        bool holds = true;

        Iterator<RandomVariable> i = getScope().iterator();
        RandomVariable rvC, rvL = i.next();
        while (i.hasNext())
        {
            rvC = i.next();
            if (!possibleWorld.Get(rvL).Equals(possibleWorld.Get(rvC)))
            {
                holds = false;
                break;
            }
            rvL = rvC;
        }

        return holds;
    }

    // END-Proposition
    //

     
    public override string ToString()
    {
        if (null == toString)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(getDerivedName());
            for (RandomVariable rv : getScope())
            {
                sb.Append(" = ");
                sb.Append(rv);
            }
            toString = sb.ToString();
        }
        return toString;
    }
}

}
