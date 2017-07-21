namespace aima.core.search.basic.support;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.StringJoiner;

import aima.core.search.api.CSP;
import aima.core.search.api.Constraint;
import aima.core.search.api.Domain;

/**
 * Basic implementation of the CSP interface.
 * 
 * @author Ciaran O'Reilly
 */
public class BasicCSP implements CSP {
	private List<String> variables;
	private Map<String, Integer> variableIndexes = new HashMap<>();
	private List<Domain> domains;
	private List<Constraint> constraints;
	
	public BasicCSP(String[] variables, Object[][] domains, Constraint...constraints) {
		if (variables.length != domains.length) {
			throw new IllegalArgumentException("Variable and Domain lengths must match");
		}
		
		this.variables = Collections.unmodifiableList(Arrays.asList(variables));
		for (int i = 0; i < variables.length; i++) {
			variableIndexes.put(variables[i], i);
		}
		this.domains = new ArrayList<>();
		for (int i = 0; i < domains.length; i++) {
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