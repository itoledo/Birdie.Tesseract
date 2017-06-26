 namespace aima.core.learning.framework;

/**
 * @author Ravi Mohan
 * 
 */
public class NumericAttributeSpecification : AttributeSpecification {

	// a simple attribute representing a number represented as a double .
	private string name;

	public NumericAttributeSpecification(string name) {
		this.name = name;
	}

	public bool isValid(string string) {
		try {
			Double.parseDouble(string);
			return true;
		} catch (Exception e) {
			return false;
		}
	}

	public string getAttributeName() {
		return name;
	}

	public Attribute createAttribute(string rawValue) {
		return new NumericAttribute(Double.parseDouble(rawValue), this);
	}
}
