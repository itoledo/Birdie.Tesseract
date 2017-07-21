namespace aima.core.search.basic.support;

using java.util.ArrayList;
using java.util.Arrays;
using java.util.Collections;
using java.util.HashMap;
using java.util.List;
using java.util.Map;
using java.util.StringJoiner;

using aima.core.search.api.CSP;
using aima.core.search.api.Constraint;
using aima.core.search.api.Domain;

/**
 * Basic implementation of the CSP interface.
 * 
 * @author Ciaran O'Reilly
 */
public class BasicCSP implements CSP {
	private List<String> variables;
	private IDictionary<String, Integer> variableIndexes = new HashMap<>();
	private List<Domain> domains;
	private List<Constraint> constraints;
	
	public BasicCSP(String[] variables, Object[][] domains, Constraint...constraints) {
		if (variables.Length != domains.Length) {
			throw new IllegalArgumentException("Variable and Domain lengths must match");
		}
		
		this.variables = Collections.unmodifiableList(Arrays.asList(variables));
		for (int i = 0; i < variables.Length; i++) {
			variableIndexes.put(variables[i], i);
		}
		this.domains = new ArrayList<>();
		for (int i = 0; i < domains.Length; i++) {
			this.domains.add(new BasicDomain(domains[i]));
		}
		this.domains = Collections.unmodifiableList(this.domains);
		this.constraints = Collections.unmodifiableList(Arrays.asList(constraints));
	}
	
	 
	public List<String> getVariables() {
		return variables;
	}
	
	 
	public List<Domain> getDomains() {
		return domains;
	}
	
	 
	public List<Constraint> getConstraints() {
		return constraints;
	}
	
	 
	public override string ToString() {
		StringJoiner sj = new StringJoiner(", ", "{", "}");
		
		for (int i = 0; i < variables.size(); i++) {
			sj.add(getVariables().get(i)+"="+getDomains().get(i));
		}
		
		return sj.ToString();
	}
}