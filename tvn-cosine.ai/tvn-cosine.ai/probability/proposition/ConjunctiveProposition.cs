 namespace aima.core.probability.proposition;

 

import aima.core.probability.RandomVariable;

public class ConjunctiveProposition : AbstractProposition implements
		BinarySentenceProposition {

	private Proposition left = null;
	private Proposition right = null;
	//
	private string toString = null;

	public ConjunctiveProposition(Proposition left, Proposition right) {
		if (null == left) {
			throw new ArgumentException(
					"Left side of conjunction must be specified.");
		}
		if (null == right) {
			throw new ArgumentException(
					"Right side of conjunction must be specified.");
		}
		// Track nested scope
		addScope(left.getScope());
		addScope(right.getScope());
		addUnboundScope(left.getUnboundScope());
		addUnboundScope(right.getUnboundScope());

		this.left = left;
		this.right = right;
	}

	 
	public bool holds(IDictionary<RandomVariable, Object> possibleWorld) {
		return left.holds(possibleWorld) && right.holds(possibleWorld);
	}

	 
	public override string ToString() {
		if (null == toString) {
			StringBuilder sb = new StringBuilder();
			sb.Append("(");
			sb.Append(left.ToString());
			sb.Append(" AND ");
			sb.Append(right.ToString());
			sb.Append(")");

			toString = sb.ToString();
		}

		return toString;
	}
}
