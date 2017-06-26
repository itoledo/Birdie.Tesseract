 namespace aima.core.probability.domain;

 
 
 

public class FiniteIntegerDomain : AbstractFiniteDomain {

	private ISet<int> possibleValues = null;

	public FiniteIntegerDomain(Integer... pValues) {
		// Keep consistent order
		possibleValues = new HashSet<int>();
		for (Integer v : pValues) {
			possibleValues.Add(v);
		}
		// Ensure cannot be modified
		possibleValues = new HashSet<>(possibleValues);

		indexPossibleValues(possibleValues);
	}

	//
	// START-Domain
	 
	public int size() {
		return possibleValues.Count;
	}

	 
	public bool isOrdered() {
		return true;
	}

	// END-Domain
	//

	//
	// START-DiscreteDomain
	 
	public ISet<int> getPossibleValues() {
		return possibleValues;
	}

	// END-DiscreteDomain
	//

	 
	public override bool Equals(object o) {

		if (this == o) {
			return true;
		}
		if (!(o is FiniteIntegerDomain)) {
			return false;
		}

		FiniteIntegerDomain other = (FiniteIntegerDomain) o;

		return this.possibleValues .Equals(other.possibleValues);
	}

	 
	public override int GetHashCode() {
		return possibleValues .GetHashCode();
	}
}
