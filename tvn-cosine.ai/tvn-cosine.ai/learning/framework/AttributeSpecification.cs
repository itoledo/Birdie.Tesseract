namespace tvn.cosine.ai.learning.framework
{ 
    public interface AttributeSpecification
    { 
        bool isValid(string s); 
        string getAttributeName(); 
        Attribute createAttribute(string rawValue);
    }
}
