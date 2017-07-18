namespace tvn.cosine.ai.probability.bayes.impl
{
    /**
     * Default implementation of the ConditionalProbabilityTable interface.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class CPT : ConditionalProbabilityTable
    {

    private RandomVariable on = null;
    private LinkedHashSet<RandomVariable> parents = Factory.CreateSet<RandomVariable>();
    private ProbabilityTable table = null;
    private IQueue<Object> onDomain = Factory.CreateQueue<Object>();

    public CPT(RandomVariable on, double[] values,
            params RandomVariable[] conditionedOn)
    {
        this.on = on;
        if (null == conditionedOn)
        {
            conditionedOn = new RandomVariable[0];
        }
        RandomVariable[] tableVars = new RandomVariable[conditionedOn.Length + 1];
        for (int i = 0; i < conditionedOn.Length; i++)
        {
            tableVars[i] = conditionedOn[i];
            parents.Add(conditionedOn[i]);
        }
        tableVars[conditionedOn.Length] = on;
        table = new ProbabilityTable(values, tableVars);
        onDomain.addAll(((FiniteDomain)on.getDomain()).getPossibleValues());

        checkEachRowTotalsOne();
    }

    public double probabilityFor(final params object[]  values)
    {
        return table.getValue(values);
    }

    //
    // START-ConditionalProbabilityDistribution

     
    public RandomVariable getOn()
    {
        return on;
    }

     
    public ISet<RandomVariable> getParents()
    {
        return parents;
    }

     
    public ISet<RandomVariable> getFor()
    {
        return table.getFor();
    }

     
    public bool contains(RandomVariable rv)
    {
        return table.contains(rv);
    }

     
    public double getValue(params object[] eventValues)
    {
        return table.getValue(eventValues);
    }

     
    public double getValue(params AssignmentProposition[] eventValues)
    {
        return table.getValue(eventValues);
    }

     
    public object getSample(double probabilityChoice, params object[] parentValues)
    {
        return ProbUtil.sample(probabilityChoice, on,
                getConditioningCase(parentValues).getValues());
    }

     
    public object getSample(double probabilityChoice,
            params AssignmentProposition[] parentValues)
    {
        return ProbUtil.sample(probabilityChoice, on,
                getConditioningCase(parentValues).getValues());
    }

    // END-ConditionalProbabilityDistribution
    //

    //
    // START-ConditionalProbabilityTable
     
    public CategoricalDistribution getConditioningCase(params object[] parentValues)
    {
        if (parentValues.Length != parents.size())
        {
            throw new IllegalArgumentException(
                    "The number of parent value arguments ["
                            + parentValues.length
                            + "] is not equal to the number of parents ["
                            + parents.size() + "] for this CPT.");
        }
        AssignmentProposition[] aps = new AssignmentProposition[parentValues.Length];
        int idx = 0;
        for (RandomVariable parentRV : parents)
        {
            aps[idx] = new AssignmentProposition(parentRV, parentValues[idx]);
            idx++;
        }

        return getConditioningCase(aps);
    }

     
    public CategoricalDistribution getConditioningCase(
            params AssignmentProposition[] parentValues)
    {
        if (parentValues.Length != parents.size())
        {
            throw new IllegalArgumentException(
                    "The number of parent value arguments ["
                            + parentValues.length
                            + "] is not equal to the number of parents ["
                            + parents.size() + "] for this CPT.");
        }
        final ProbabilityTable cc = new ProbabilityTable(getOn());
        ProbabilityTable.Iterator pti = new ProbabilityTable.Iterator()
        {

            private int idx = 0;

     
            public void iterate(IMap<RandomVariable, object> possibleAssignment,
                    double probability)
    {
        cc.getValues()[idx] = probability;
        idx++;
    }
};
table.iterateOverTable(pti, parentValues);

		return cc;
	}

	public Factor getFactorFor(final params AssignmentProposition[]  evidence)
{
    ISet<RandomVariable> fofVars = Factory.CreateSet<RandomVariable>(
            table.getFor());
    for (AssignmentProposition ap : evidence)
    {
        fofVars.Remove(ap.getTermVariable());
    }
    final ProbabilityTable fof = new ProbabilityTable(fofVars);
    // Otherwise need to iterate through the table for the
    // non evidence variables.
    final object[]  termValues = new object[fofVars.size()];
    ProbabilityTable.Iterator di = new ProbabilityTable.Iterator()
    {

            public void iterate(IMap<RandomVariable, object> possibleWorld,
                    double probability)
{
    if (0 == termValues.Length)
    {
        fof.getValues()[0] += probability;
    }
    else
    {
        int i = 0;
        for (RandomVariable rv : fof.getFor())
        {
            termValues[i] = possibleWorld.Get(rv);
            i++;
        }
        fof.getValues()[fof.getIndex(termValues)] += probability;
    }
}
		};
		table.iterateOverTable(di, evidence);

		return fof;
	}

	// END-ConditionalProbabilityTable
	//

	//
	// PRIVATE METHODS
	//
	private void checkEachRowTotalsOne()
{
    ProbabilityTable.Iterator di = new ProbabilityTable.Iterator()
    {

            private int rowSize = onDomain.size();
private int iterateCnt = 0;
private double rowProb = 0;

public void iterate(IMap<RandomVariable, object> possibleWorld,
        double probability)
{
    iterateCnt++;
    rowProb += probability;
    if (iterateCnt % rowSize == 0)
    {
        if (System.Math.Abs(1 - rowProb) > ProbabilityModel.DEFAULT_ROUNDING_THRESHOLD)
        {
            throw new IllegalArgumentException("Row "
                    + (iterateCnt / rowSize)
                    + " of CPT does not sum to 1.0.");
        }
        rowProb = 0;
    }
}
		};

		table.iterateOverTable(di);
	}
}

}
