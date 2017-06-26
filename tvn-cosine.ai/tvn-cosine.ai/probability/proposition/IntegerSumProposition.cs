 namespace aima.core.probability.proposition;

 
 
 

import aima.core.probability.RandomVariable;
import aima.core.probability.domain.FiniteIntegerDomain;

public class IntegerSumProposition : AbstractDerivedProposition {

	private FiniteIntegerDomain sumsDomain = null;
	private List<RandomVariable> sumVars = new List<RandomVariable>();
	//
	private string toString = null;

	public IntegerSumProposition(string name, FiniteIntegerDomain sumsDomain,
			RandomVariable... sums) {
		base(name);

		if (null == sumsDomain) {
			throw new ArgumentException("Sum Domain must be specified.");
		}
		if (null == sums || 0 == sums.Length) {
			throw new ArgumentException(
					"Sum variables must be specified.");
		}
		this.sumsDomain = sumsDomain;
		for (RandomVariable rv : sums) {
			addScope(rv);
			sumVars.Add(rv);
		}
	}

	//
	// START-Proposition
	public bool holds(IDictionary<RandomVariable, Object> possibleWorld) {
		Integer sum = new Integer(0);

		for (RandomVariable rv : sumVars) {
			Object o = possibleWorld.get(rv);
			if (o is Integer) {
				sum += ((Integer) o);
			} else {
				throw new ArgumentException(
						"Possible World does not contain a int value for the sum variable:"
								+ rv);
			}
		}

		return sumsDomain.getPossibleValues().Contains(sum);
	}

	// END-Proposition
	//

	 
	public override string ToString() {
		if (null == toString) {
			StringBuilder sb = new StringBuilder();
			sb.Append(getDerivedName());
			sb.Append(" = ");
			sb.Append(sumsDomain.ToString());
			toString = sb.ToString();
		}
		return toString;
	}
}
