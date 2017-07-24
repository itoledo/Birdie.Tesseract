namespace tvn.cosine.ai.learning.framework
{ 
    public interface IAttributeSpecification
    { 
        bool IsValid(string s); 
        string GetAttributeName(); 
        IAttribute CreateAttribute(string rawValue);
    }
}
