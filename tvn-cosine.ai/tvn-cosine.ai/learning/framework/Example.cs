 namespace aima.core.learning.framework;

 

/**
 * @author Ravi Mohan
 * 
 */
public class Example {
	Dictionary<String, Attribute> attributes;

	private Attribute targetAttribute;

	public Example(Dictionary<String, Attribute> attributes,
			Attribute targetAttribute) {
		this.attributes = attributes;
		this.targetAttribute = targetAttribute;
	}

	public string getAttributeValueAsString(string attributeName) {
		return attributes.get(attributeName).valueAsString();
	}

	public double getAttributeValueAsDouble(string attributeName) {
		Attribute attribute = attributes.get(attributeName);
		if (attribute == null || !(attribute is NumericAttribute)) {
			throw new  Exception(
					"cannot return numerical value for non numeric attribute");
		}
		return ((NumericAttribute) attribute).valueAsDouble();
	}

	 
	public override string ToString() {
		return attributes.ToString();
	}

	public string targetValue() {
		return getAttributeValueAsString(targetAttribute.name());
	}

	 
	public override bool Equals(object o) {
		if (this == o) {
			return true;
		}
		if ((o == null) || (this .GetType() != o .GetType())) {
			return false;
		}
		Example other = (Example) o;
		return attributes .Equals(other.attributes);
	}

	 
	public override int GetHashCode() {
		return attributes .GetHashCode();
	}

	public Example numerize(
			Dictionary<String, Dictionary<String, int>> attrValueToNumber) {
		Dictionary<String, Attribute> numerizedExampleData = new Dictionary<String, Attribute>();
		for (string key : attributes.Keys) {
			Attribute attribute = attributes.get(key);
			if (attribute is StringAttribute) {
				int correspondingNumber = attrValueToNumber.get(key).get(
						attribute.valueAsString());
				NumericAttributeSpecification spec = new NumericAttributeSpecification(
						key);
				numerizedExampleData.Add(key, new NumericAttribute(
						correspondingNumber, spec));
			} else {// Numeric Attribute
				numerizedExampleData.Add(key, attribute);
			}
		}
		return new Example(numerizedExampleData,
				numerizedExampleData.get(targetAttribute.name()));
	}
}
