 namespace aima.core.learning.framework;

/**
 * @author Ravi Mohan
 * 
 */
public class StringAttribute : Attribute {
	private StringAttributeSpecification spec;

	private string value;

	public StringAttribute(string value, StringAttributeSpecification spec) {
		this.spec = spec;
		this.value = value;
	}

	public string valueAsString() {
		return value.trim();
	}

	public string name() {
		return spec.getAttributeName().trim();
	}
}
