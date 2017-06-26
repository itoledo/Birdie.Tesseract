 namespace aima.core.learning.framework;

/**
 * @author Ravi Mohan
 * 
 */
public class NumericAttribute : Attribute {
	double value;

	private NumericAttributeSpecification spec;

	public NumericAttribute(double rawValue, NumericAttributeSpecification spec) {
		this.value = rawValue;
		this.spec = spec;
	}

	public string valueAsString() {
		return double.toString(value);
	}

	public string name() {
		return spec.getAttributeName().trim();
	}

	public double valueAsDouble() {
		return value;
	}
}
