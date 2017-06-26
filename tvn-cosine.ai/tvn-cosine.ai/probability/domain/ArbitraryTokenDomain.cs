 namespace aima.core.probability.domain;

 
 
 

/**
 * Artificial Intelligence A Modern Approach (3rd Edition): page 486.
 * 
 * As in CSPs, domains can be sets of arbitrary tokens; we might choose the
 * domain of <i>Age</i> to be {<i>juvenile,teen,adult</i>} and the domain of
 * <i>Weather</i> might be {<i>sunny,rain,cloudy,snow</i>}.
 * 
 * @author Ciaran O'Reilly
 */
public class ArbitraryTokenDomain : AbstractFiniteDomain {

	private ISet<object> possibleValues = null;
	private bool ordered = false;

	public ArbitraryTokenDomain(object... pValues) {
		this(false, pValues);
	}

	public ArbitraryTokenDomain(bool ordered, Object... pValues) {
		this.ordered = ordered;
		// Keep consistent order
		possibleValues = new HashSet<object>();
		for (object v : pValues) {
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
		return ordered;
	}

	// END-Domain
	//

	//
	// START-FiniteDomain
	 
	public ISet<object> getPossibleValues() {
		return possibleValues;
	}

	// END-finiteDomain
	//

	 
	public override bool Equals(object o) {

		if (this == o) {
			return true;
		}
		if (!(o is ArbitraryTokenDomain)) {
			return false;
		}

		ArbitraryTokenDomain other = (ArbitraryTokenDomain) o;

		return this.possibleValues .Equals(other.possibleValues);
	}

	 
	public override int GetHashCode() {
		return possibleValues .GetHashCode();
	}
}
