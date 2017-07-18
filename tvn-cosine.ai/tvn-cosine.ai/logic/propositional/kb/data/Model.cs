namespace tvn.cosine.ai.logic.propositional.kb.data
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): pages 240, 245.<br>
     * <br>
     * Models are mathematical abstractions, each of which simply fixes the truth or
     * falsehood of every relevant sentence. In propositional logic, a model simply
     * fixes the <b>truth value</b> - <em>true</em> or <em>false</em> - for
     * every proposition symbol.<br>
     * <br>
     * Models as implemented here can represent partial assignments 
     * to the set of proposition symbols in a Knowledge Base (i.e. a partial model).
     * 
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class Model : PLVisitor<Boolean, Boolean> {


    private HashMap<PropositionSymbol, Boolean> assignments = Factory.CreateMap<PropositionSymbol, Boolean>();

    /**
	 * Default Constructor.
	 */
    public Model()
    {
    }

    public Model(IMap<PropositionSymbol, Boolean> values)
    {
        assignments.putAll(values);
    }

    public Boolean getValue(PropositionSymbol symbol)
    {
        return assignments.Get(symbol);
    }

    public bool isTrue(PropositionSymbol symbol)
    {
        return Boolean.TRUE.Equals(assignments.Get(symbol));
    }

    public bool isFalse(PropositionSymbol symbol)
    {
        return Boolean.FALSE.Equals(assignments.Get(symbol));
    }

    public Model union(PropositionSymbol symbol, bool b)
    {
        Model m = new Model();
        m.assignments.putAll(this.assignments);
        m.assignments.Put(symbol, b);
        return m;
    }

    public Model unionInPlace(PropositionSymbol symbol, bool b)
    {
        assignments.Put(symbol, b);
        return this;
    }

    public bool remove(PropositionSymbol p)
    {
        return assignments.Remove(p);
    }

    public bool isTrue(Sentence s)
    {
        return Boolean.TRUE.Equals(s.accept(this, null));
    }

    public bool isFalse(Sentence s)
    {
        return Boolean.FALSE.Equals(s.accept(this, null));
    }

    public bool isUnknown(Sentence s)
    {
        return null == s.accept(this, null);
    }

    public Model flip(PropositionSymbol s)
    {
        if (isTrue(s))
        {
            return union(s, false);
        }
        if (isFalse(s))
        {
            return union(s, true);
        }
        return this;
    }

    public ISet<PropositionSymbol> getAssignedSymbols()
    {
        return Factory.CreateReadOnlySet<>(assignments.GetKeys());
    }

    /**
	 * Determine if the model satisfies a set of clauses.
	 * 
	 * @param clauses
	 *            a set of propositional clauses.
	 * @return if the model satisfies the clauses, false otherwise.
	 */
    public bool satisfies(Set<Clause> clauses)
    {
        for (Clause c : clauses)
        {
            // All must to be true
            if (!Boolean.TRUE.Equals(determineValue(c)))
            {
                return false;
            }
        }
        return true;
    }

    /**
	 * Determine based on the current assignments within the model, whether a
	 * clause is known to be true, false, or unknown.
	 * 
	 * @param c
	 *            a propositional clause.
	 * @return true, if the clause is known to be true under the model's
	 *         assignments. false, if the clause is known to be false under the
	 *         model's assignments. null, if it is unknown whether the clause is
	 *         true or false under the model's current assignments.
	 */
    public Boolean determineValue(Clause c)
    {
        Boolean result = null; // i.e. unknown

        if (c.isTautology())
        { // Test independent of the model's assignments.
            result = Boolean.TRUE;
        }
        else if (c.isFalse())
        { // Test independent of the model's
          // assignments.
            result = Boolean.FALSE;
        }
        else
        {
            bool unassignedSymbols = false;
            Boolean value = null;
            for (PropositionSymbol positive : c.getPositiveSymbols())
            {
                value = assignments.Get(positive);
                if (value != null)
                {
                    if (Boolean.TRUE.Equals(value))
                    {
                        result = Boolean.TRUE;
                        break;
                    }
                }
                else
                {
                    unassignedSymbols = true;
                }
            }
            // If truth not determined, continue checking negative symbols
            if (result == null)
            {
                for (PropositionSymbol negative : c.getNegativeSymbols())
                {
                    value = assignments.Get(negative);
                    if (value != null)
                    {
                        if (Boolean.FALSE.Equals(value))
                        {
                            result = Boolean.TRUE;
                            break;
                        }
                    }
                    else
                    {
                        unassignedSymbols = true;
                    }
                }

                if (result == null)
                {
                    // If truth not determined and there are no
                    // unassigned symbols then we can determine falsehood
                    // (i.e. all of its literals are assigned false under the
                    // model)
                    if (!unassignedSymbols)
                    {
                        result = Boolean.FALSE;
                    }
                }
            }
        }

        return result;
    }

    public void print()
    {
        for (Map.Entry<PropositionSymbol, Boolean> e : assignments.entrySet())
        {
            System.out.print(e.getKey() + " = " + e.getValue() + " ");
        }
        System.Console.WriteLine();
    }

     
    public override string ToString()
    {
        return assignments.ToString();
    }

    //
    // START-PLVisitor
     
    public Boolean visitPropositionSymbol(PropositionSymbol s, Boolean arg)
    {
        if (s.isAlwaysTrue())
        {
            return Boolean.TRUE;
        }
        if (s.isAlwaysFalse())
        {
            return Boolean.FALSE;
        }
        return getValue(s);
    }

     
    public Boolean visitUnarySentence(ComplexSentence fs, Boolean arg)
    {
        object negatedValue = fs.getSimplerSentence(0).accept(this, null);
        if (negatedValue != null)
        {
            return new Boolean(!((Boolean)negatedValue).booleanValue());
        }
        else
        {
            return null;
        }
    }

     
    public Boolean visitBinarySentence(ComplexSentence bs, Boolean arg)
    {
        Boolean firstValue = (Boolean)bs.getSimplerSentence(0).accept(this,
                null);
        Boolean secondValue = (Boolean)bs.getSimplerSentence(1).accept(this,
                null);
        if ((firstValue == null) || (secondValue == null))
        {
            // strictly not true for or/and
            // -FIX later
            return null;
        }
        else
        {
            Connective connective = bs.getConnective();
            if (connective.Equals(Connective.AND))
            {
                return firstValue && secondValue;
            }
            else if (connective.Equals(Connective.OR))
            {
                return firstValue || secondValue;
            }
            else if (connective.Equals(Connective.IMPLICATION))
            {
                return !(firstValue && !secondValue);
            }
            else if (connective.Equals(Connective.BICONDITIONAL))
            {
                return firstValue.Equals(secondValue);
            }
            return null;
        }
    }
    // END-PLVisitor
    //
}
}
