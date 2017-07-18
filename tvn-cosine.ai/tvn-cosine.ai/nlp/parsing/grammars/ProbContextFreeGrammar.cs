namespace tvn.cosine.ai.nlp.parsing.grammars
{
    public class ProbContextFreeGrammar : ProbContextSensitiveGrammar : ProbabilisticGrammar
    {

    // default constructor
    public ProbContextFreeGrammar()
    {
        type = 2;
        rules = null;
    }

    /**
	 * Add a ruleList as the grammar's rule list if all rules in it pass
	 * both the restrictions of the parent grammars (unrestricted and context-sens)
	 * and this grammar's restrictions.
	 */
    public bool addRules(IQueue<Rule> ruleList)
    {
        for (int i = 0; i < ruleList.size(); i++)
        {
            if (!super.validRule(ruleList.Get(i)) || !validRule(ruleList.Get(i)))
            {
                return false;
            }
        }
        this.rules = ruleList;
        return true;
    }

    /**
	 * Add a rule to the grammar's rule list if it passes
	 * both the restrictions of the parent grammars (unrestricted and context-sens)
	 * and this grammar's restrictions.
	 */
    public bool addRule(Rule r)
    {
        if (!super.validRule(r) || !validRule(r))
        {
            return false;
        }
        rules.Add(r);
        return true;
    }

    /**
	 * For a grammar rule to be valid in a context-free grammar, 
	 * all the restrictions of the parent grammars must hold, and the restriction
	 * of the context-free grammar must hold. The restriction is that the lhs must 
	 * consist of a single non-terminal (variable). There are no restrictions on the rhs
	 * 
	 */
    public bool validRule(Rule r)
    {
        if (!super.validRule(r))
        {
            return false;
        }
        // lhs must be a single non-terminal
        if (r.lhs.size() != 1 || !isVariable(r.lhs.Get(0)))
        {
            return false;
        }

        return true;
    }

    /**
	 * Test whether LHS -> RHS is a rule in the grammar. 
	 * Note: it must be a DIRECT derivation
	 * @param lhs
	 * @param rhs
	 * @return
	 */
    public bool leftDerivesRight(ArrayList<string> lhs, ArrayList<string> rhs)
    {

        // for each rule in the grammar 
        for (int i = 0; i < rules.size(); i++)
        {
            Rule r = rules.Get(i);
            if (r.lhs.Equals(lhs) && r.rhs.Equals(rhs))
            {
                // matching rule found. left does derive the right in this grammar
                return true;
            }
        }
        // no match found
        return false;
    }


} // end of ContextFreeGrammar()
}
