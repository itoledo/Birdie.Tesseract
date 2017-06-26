 namespace aima.core.learning.framework;

 
 
 

/**
 * @author Ravi Mohan
 * 
 */
public class DataSetSpecification {
	List<AttributeSpecification> attributeSpecifications;

	private string targetAttribute;

	public DataSetSpecification() {
		this.attributeSpecifications = new List<AttributeSpecification>();
	}

	public bool isValid(List<string> uncheckedAttributes) {
		if (attributeSpecifications.Count != uncheckedAttributes.Count) {
			throw new  Exception("size mismatch specsize = "
					+ attributeSpecifications.Count + " attrbutes size = "
					+ uncheckedAttributes.Count);
		}
		Iterator<AttributeSpecification> attributeSpecIter = attributeSpecifications
				.iterator();
		Iterator<string> valueIter = uncheckedAttributes.iterator();
		while (valueIter.hasNext() && attributeSpecIter.hasNext()) {
			if (!(attributeSpecIter.next().isValid(valueIter.next()))) {
				return false;
			}
		}
		return true;
	}

	/**
	 * @return Returns the targetAttribute.
	 */
	public string getTarget() {
		return targetAttribute;
	}

	public List<string> getPossibleAttributeValues(string attributeName) {
		for (AttributeSpecification as : attributeSpecifications) {
			if (as.getAttributeName() .Equals(attributeName)) {
				return ((StringAttributeSpecification) as)
						.possibleAttributeValues();
			}
		}
		throw new  Exception("No such attribute" + attributeName);
	}

	public List<string> getAttributeNames() {
		List<string> names = new List<string>();
		for (AttributeSpecification as : attributeSpecifications) {
			names.Add(as.getAttributeName());
		}
		return names;
	}

	public void defineStringAttribute(string name, string[] attributeValues) {
		attributeSpecifications.Add(new StringAttributeSpecification(name,
				attributeValues));
		setTarget(name);// target defaults to last column added
	}

	/**
	 * @param target
	 *            The targetAttribute to set.
	 */
	public void setTarget(string target) {
		this.targetAttribute = target;
	}

	public AttributeSpecification getAttributeSpecFor(string name) {
		for (AttributeSpecification spec : attributeSpecifications) {
			if (spec.getAttributeName() .Equals(name)) {
				return spec;
			}
		}
		throw new  Exception("no attribute spec for  " + name);
	}

	public void defineNumericAttribute(string name) {
		attributeSpecifications.Add(new NumericAttributeSpecification(name));
	}

	public List<string> getNamesOfStringAttributes() {
		List<string> names = new List<string>();
		for (AttributeSpecification spec : attributeSpecifications) {
			if (spec is StringAttributeSpecification) {
				names.Add(spec.getAttributeName());
			}
		}
		return names;
	}
}
