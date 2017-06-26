 namespace aima.core.probability.proposition;

 
 

import aima.core.probability.RandomVariable;

public class EquivalentProposition : AbstractDerivedProposition {
	//
	private string toString = null;

	public EquivalentProposition(string name, RandomVariable... equivs) {
		base(name);

		if (null == equivs || 1 >= equivs.Length) {
			throw new ArgumentException(
					"Equivalent variables must be specified.");
		}
		for (RandomVariable rv : equivs) {
			addScope(rv);
		}
	}

	//
	// START-Proposition
	public bool holds(IDictionary<RandomVariable, Object> possibleWorld) {
		bool holds = true;

		Iterator<RandomVariable> i = getScope().iterator();
		RandomVariable rvC, rvL = i.next();
		while (i.hasNext()) {
			rvC = i.next();
			if (!possibleWorld.get(rvL) .Equals(possibleWorld.get(rvC))) {
				holds = false;
				break;
			}
			rvL = rvC;
		}

		return holds;
	}

	// END-Proposition
	//

	 
	public override string ToString() {
		if (null == toString) {
			StringBuilder sb = new StringBuilder();
			sb.Append(getDerivedName());
			for (RandomVariable rv : getScope()) {
				sb.Append(" = ");
				sb.Append(rv);
			}
			toString = sb.ToString();
		}
		return toString;
	}
}
