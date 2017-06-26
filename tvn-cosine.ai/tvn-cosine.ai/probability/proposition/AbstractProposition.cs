 namespace aima.core.probability.proposition;

import java.util.Collection;
 
 
 

import aima.core.probability.RandomVariable;

public abstract class AbstractProposition : Proposition {

	private HashSet<RandomVariable> scope = new HashSet<RandomVariable>();
	private HashSet<RandomVariable> unboundScope = new HashSet<RandomVariable>();

	public AbstractProposition() {

	}

	//
	// START-Proposition
	public ISet<RandomVariable> getScope() {
		return scope;
	}

	public ISet<RandomVariable> getUnboundScope() {
		return unboundScope;
	}

	public abstract bool holds(IDictionary<RandomVariable, Object> possibleWorld);

	// END-Proposition
	//

	//
	// Protected Methods
	//
	protected void addScope(RandomVariable var) {
		scope.Add(var);
	}

	protected void addScope(ICollection<RandomVariable> vars) {
		scope.addAll(vars);
	}

	protected void addUnboundScope(RandomVariable var) {
		unboundScope.Add(var);
	}

	protected void addUnboundScope(ICollection<RandomVariable> vars) {
		unboundScope.addAll(vars);
	}
}
