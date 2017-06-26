 namespace aima.core.probability.proposition;

 

import aima.core.probability.RandomVariable;

/**
 * Artificial Intelligence A Modern Approach (3rd Edition): Formula 13.4, page
 * 489.<br>
 * <br>
 * 
 * We can also derive the well-known formula for the probability of a
 * disjunction, sometimes called the <b>inclusion-exclusion principle:</b><br>
 * <br>
 * P(a OR b) = P(a) + P(b) - P(a AND b).<br>
 * 
 * @author Ciaran O'Reilly
 */
public class DisjunctiveProposition : AbstractProposition implements
		BinarySentenceProposition {

	private Proposition left = null;
	private Proposition right = null;
	//
	private string toString = null;

	public DisjunctiveProposition(Proposition left, Proposition right) {
		if (null == left) {
			throw new ArgumentException(
					"Left side of disjunction must be specified.");
		}
		if (null == right) {
			throw new ArgumentException(
					"Right side of disjunction must be specified.");
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
		return left.holds(possibleWorld) || right.holds(possibleWorld);
	}

	 
	public override string ToString() {
		if (null == toString) {
			StringBuilder sb = new StringBuilder();
			sb.Append("(");
			sb.Append(left.ToString());
			sb.Append(" OR ");
			sb.Append(right.ToString());
			sb.Append(")");

			toString = sb.ToString();
		}

		return toString;
	}
}
