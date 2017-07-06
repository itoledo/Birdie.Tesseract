using System;

namespace tvn.cosine.ai.learning.framework
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class NumericAttributeSpecification : AttributeSpecification
    {
        // a simple attribute representing a number represented as a double .
        private string name;

        public NumericAttributeSpecification(String name)
        {
            this.name = name;
        }

        public bool isValid(string s)
        {
            double o;
            return double.TryParse(s, out o);
        }

        public string getAttributeName()
        {
            return name;
        }

        public Attribute createAttribute(string rawValue)
        {
            return new NumericAttribute(double.Parse(rawValue), this);
        }
    }
}
