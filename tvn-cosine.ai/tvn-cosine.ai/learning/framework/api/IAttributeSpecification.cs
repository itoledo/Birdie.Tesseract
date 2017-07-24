namespace tvn.cosine.ai.learning.framework.api
{ 
    public interface IAttributeSpecification
    { 
        bool IsValid(string s); 
        string GetAttributeName(); 
        IAttribute CreateAttribute(string rawValue);
    }
}
