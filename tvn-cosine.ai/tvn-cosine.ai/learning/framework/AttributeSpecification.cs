namespace tvn.cosine.ai.learning.framework
{
    /**
     * @author Ravi Mohan
     * 
     */
    public interface AttributeSpecification
    { 
        bool isValid(string s); 
        string getAttributeName(); 
        Attribute createAttribute(string rawValue);
    }
}
