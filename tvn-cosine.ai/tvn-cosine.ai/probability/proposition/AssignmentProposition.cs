 namespace aima.core.probability.proposition;

 

import aima.core.probability.RandomVariable;

public class AssignmentProposition : AbstractTermProposition {
	private object value = null;
	//
	private string toString = null;

	public AssignmentProposition(RandomVariable forVariable, object value) {
		base(forVariable);
		setValue(value);
	}

	public object getValue() {
		return value;
	}
	
	public void setValue(object value) {
		if (null == value) {
			throw new ArgumentException(
					"The value for the Random Variable must be specified.");
		}
		this.value = value;
	}

	 
	public bool holds(IDictionary<RandomVariable, Object> possibleWorld) {
		return value .Equals(possibleWorld.get(getTermVariable()));
	}

	 
	public override string ToString() {
		if (null == toString) {
			StringBuilder sb = new StringBuilder();
			sb.Append(getTermVariable().getName());
			sb.Append(" = ");
			sb.Append(value);

			toString = sb.ToString();
		}
		return toString;
	}
}
