 namespace aima.core.probability.proposition;

 

import aima.core.probability.RandomVariable;

public class NotProposition : AbstractProposition implements
		UnarySentenceProposition {

	private Proposition proposition;
	//
	private string toString = null;

	public NotProposition(Proposition prop) {
		if (null == prop) {
			throw new ArgumentException(
					"Proposition to be negated must be specified.");
		}
		// Track nested scope
		addScope(prop.getScope());
		addUnboundScope(prop.getUnboundScope());

		proposition = prop;
	}

	 
	public bool holds(IDictionary<RandomVariable, Object> possibleWorld) {
		return !proposition.holds(possibleWorld);
	}

	 
	public override string ToString() {
		if (null == toString) {
			StringBuilder sb = new StringBuilder();
			sb.Append("(NOT ");
			sb.Append(proposition.ToString());
			sb.Append(")");

			toString = sb.ToString();
		}
		return toString;
	}
}
