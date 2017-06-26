 namespace aima.core.learning.framework;

/**
 * @author Ravi Mohan
 * 
 */
public interface AttributeSpecification {

	bool isValid(string string);

	String getAttributeName();

	Attribute createAttribute(string rawValue);
}
