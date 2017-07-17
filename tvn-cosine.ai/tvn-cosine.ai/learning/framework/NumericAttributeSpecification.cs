using System.Globalization;

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

        public NumericAttributeSpecification(string name)
        {
            this.name = name;
        }

        public bool isValid(string s)
        {
            double o;
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out o))
            {
                return true;
            }
            return false;
        }

        public string getAttributeName()
        {
            return name;
        }

        public Attribute createAttribute(string rawValue)
        {
            return new NumericAttribute(double.Parse(rawValue, NumberStyles.Any, CultureInfo.InvariantCulture), this);
        }
    }
}
