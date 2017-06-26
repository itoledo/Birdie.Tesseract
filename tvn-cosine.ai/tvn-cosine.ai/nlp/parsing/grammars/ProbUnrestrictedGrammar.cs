 namespace aima.core.nlp.parsing.grammars;

 
 
 

/**
 * Represents the most general grammatical formalism,
 * the Unrestricted (or Recrusively Enumerable) Grammar.
 * All other grammars can derive from this grammar, imposing extra
 * restrictions.
 * @author Jonathon
 *
 */
public class ProbUnrestrictedGrammar : ProbabilisticGrammar {

	// types of grammars
	public static readonly int UNRESTRICTED = 0;
	public static readonly int CONTEXT_SENSITIVE = 1;
	public static readonly int	CONTEXT_FREE = 2;
	public static readonly int REGULAR = 3;
	public static readonly int CNFGRAMMAR = 4;
	public static readonly int PROB_CONTEXT_FREE = 5;
	
	public List<Rule> rules;
	public List<string> vars;
	public List<string> terminals;
	public int type; 
	
	// default constructor. has no rules
	public ProbUnrestrictedGrammar() {
		type = 0;
		rules = new List<Rule>();
		vars =  new List<string>();
		terminals = new List<string>();
	}
	
	/**
	 * Add a number of rules at once, testing each in turn
	 * for validity, and then testing the batch for probability validity.
	 * @param ruleList
	 * @return true if rules are valid and incorporated into the grammar. false, otherwise
	 */
	public bool addRules( List<Rule> ruleList ) {
		for( int i=0; i < ruleList.Count; ++i ) {
			if( !validRule(ruleList.get(i)) ) {
				return false;
			}
		}
		if( !validateRuleProbabilities(ruleList)) {
			return false;
		}
		this.rules = ruleList;
		updateVarsAndTerminals();
		return true;
	}
	
	/**
	 * Add a single rule the grammar, testing it for structural 
	 * and probability validity.
	 * @param rule
	 * @return true if rule is incorporated. false, otherwise
	 */
	// TODO: More sophisticated probability distribution management
	public bool addRule( Rule rule ) {
		if( validRule(rule)) {
			rules.Add(rule);
			updateVarsAndTerminals( rule );
			return true;
		}
		else {
			return false;
		}
	}
	
	/**
	 * For a set of rules, test whether each batch of rules with the same 
	 * LHS have their probabilities sum to exactly 1.0
	 * @param ruleList
	 * @return true if the probabilities are valid. false, otherwise
	 */
	public bool validateRuleProbabilities( List<Rule> ruleList ) {
		float probTotal = 0;
		for( int i=0; i < vars.Count; ++i ) {
			for( int j=0; j < ruleList.Count; j++ ) {
				// reset probTotal at start
				if( j == 0 ) {
					probTotal = (float) 0.0;
				}
				if( ruleList.get(i).lhs.get(0) .Equals(vars.get(i))) {
					probTotal += ruleList.get(i).PROB;
				}
				// check probTotal hasn't exceed max
				if( probTotal > 1.0 ) {
					return false;
				}
				// check we have correct probability total
				if( j == ruleList.Count -1 && probTotal != (float) 1.0 ) {
					return false;
				}
			}
		}
		return true;
	}
	
	/**
	 * Test validity of the LHS and RHS of grammar rule.
	 * In unrestricted grammar, the only invalid rule type is
	 * a rule with a null LHS.
	 * @param r ( a rule )
	 * @return true, if rule has valid form. false, otherwise
	 */
	public bool validRule( Rule r ) {
		if( r.lhs != null && r.lhs.Count > 0 ) {
			return true;
		}
		else {
			return false;
		}
	}
	
	/** 
	 * Whenever a new rule is added to the grammar, we want to 
	 * update the list of variables and terminals with any new grammar symbols
	 */
	public void updateVarsAndTerminals() {
		if( rules == null ) {
			vars =  new List<string>();
			terminals = new List<string>();
			return;
		}
		for( int i=0; i < rules.Count; ++i ) {
			Rule r = rules.get(i);
			updateVarsAndTerminals(r);	// update the variables and terminals for this rule
		}
	}
	
	/**
	 * Update variable and terminal lists with a single rule's symbols,
	 * if there a new symbols
	 * @param r
	 */
	public void updateVarsAndTerminals( Rule r ) {
		// check lhs for new terminals or variables
		for( int j=0; j < r.lhs.Count; j++ ) {
			if( isVariable(r.lhs.get(j)) && !vars.Contains(r.lhs.get(j))) {
				vars.Add(r.lhs.get(j));
			}
			else if( isTerminal(r.lhs.get(j)) && !terminals.Contains(r.lhs.get(j))) {
				terminals.Add(r.lhs.get(j));
			}
		}
		// for rhs we must check that this isn't a null-rule
		if ( r.rhs != null ) {
			// check rhs for new terminals or variables
			for( int j=0; j < r.rhs.Count; j++ ) {
				if( isVariable(r.rhs.get(j)) && !vars.Contains(r.rhs.get(j))) {
					vars.Add(r.rhs.get(j));
				}
				else if( isTerminal(r.rhs.get(j)) && !terminals.Contains(r.rhs.get(j))) {
					terminals.Add(r.rhs.get(j));
				}
			}
		}
		// maintain sorted lists
		Collections.sort(vars);
		Collections.sort(terminals);
	}
	
	
	/**
	 * Check if we have a variable, as they are uppercase strings.
	 * @param s
	 * @return
	 */
	public static bool isVariable(string s) {
		for (int i=0; i < s.Length(); ++i)
		{
			if (!Character.isUpperCase(s.charAt(i))) {
				return false;
			}
		}
		return true;
	}
	
	/** 
	 * Check if we have a terminal, as they are lowercase strings
	 * @param s
	 * @return true, if string must be a terminal. false, otherwise
	 */
	public static bool isTerminal(string s) {
		for (int i=0; i < s.Length(); ++i ) {
			
			if( !Character.isLowerCase(s.charAt(i))) {
				return false;
			}
		}
		return true;
	}
	
	
	 
	public override string ToString() {
		StringBuilder output = new StringBuilder();

		output.Append("Variables:  ");

		this.vars.forEach(var -> output.Append(var).Append(", "));

		output.Append('\n');
		output.Append("Terminals:  ");

		this.terminals.forEach(terminal -> output.Append(terminal).Append(", "));

		output.Append('\n');

		this.rules.forEach(rule -> output.Append(rule.ToString()).Append('\n'));

		return output.ToString();
	}
}
