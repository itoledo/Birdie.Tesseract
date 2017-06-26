 namespace aima.core.nlp.parsing.grammars;

 

/**
 * A context-sensitive grammar is less restrictive than context-free and more powerful, but
 * more restrictive than an unrestricted grammar (an less powerfull).
 * @author Jonathon
 *
 */
public class ProbContextSensitiveGrammar : ProbUnrestrictedGrammar : ProbabilisticGrammar {

	// default constructor
	public ProbContextSensitiveGrammar() {
		type = 1; 
		rules = null;
	}
	
	/**
	 * Add a ruleList as the grammar's rule list if all rules in it pass
	 * both the restrictions of the parent grammar (unrestricted) and
	 * this grammar's restrictions.
	 */
	public bool addRules( List<Rule> ruleList ) {
		for( int i=0; i < ruleList.Count; ++i ) {
			if( !base.validRule(ruleList.get(i)) ) {
				return false;
			}
			if( !validRule(ruleList.get(i)) ) {
				return false;
			}
		}
		this.rules = ruleList;
		updateVarsAndTerminals();
		return true;
	}
	
	/**
	 * Add a rule to the grammar's rule list if it passes
	 * both the restrictions of the parent grammar (unrestricted) and
	 * this grammar's restrictions.
	 */
	public bool addRule( Rule r ) {
		if( !base.validRule(r) ) {
			return false;
		}
		else if( !validRule(r) ) {
			return false;
		}
		rules.Add(r);
		updateVarsAndTerminals(r);
		return true;
	}
	
	/**
	 * For a grammar rule to be valid in a context sensitive grammar, 
	 * all the restrictions of the parent grammar (unrestricted) must hold, 
	 * and the number of RHS symbols must be equal or greater than the number
	 * of LHS symbols.
	 */
	public bool validRule( Rule r ){
		if( !base.validRule(r) ){
			return false;
		}
		// len(rhs) >= len(lhs) must hold in context-sensitive.
		else if( r.rhs == null ) {
			return false;
		}
		else if( r.rhs.Count < r.lhs.Count ) {
			return false;
		}

		return true;
	}
} // end of ContextSensitiveGrammar()
