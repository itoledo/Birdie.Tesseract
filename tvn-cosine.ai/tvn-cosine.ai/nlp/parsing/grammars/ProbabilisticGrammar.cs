 namespace aima.core.nlp.parsing.grammars;

 

public interface ProbabilisticGrammar {
	
	public bool addRules( List<Rule> ruleList );
	
	public bool addRule( Rule r );
	
	public bool validRule( Rule r );
	
	public bool validateRuleProbabilities( List<Rule> ruleList );
	
	public void updateVarsAndTerminals( Rule r );
	
	public void updateVarsAndTerminals();
}
