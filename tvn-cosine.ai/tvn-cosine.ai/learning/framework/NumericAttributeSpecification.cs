using System.Globalization;

namespace tvn.cosine.ai.learning.framework
{ 
    public class NumericAttributeSpecification : IAttributeSpecification
    {
        // a simple attribute representing a number represented as a double .
        private string name;

        public NumericAttributeSpecification(string name)
        {
            this.name = name;
        }

        public bool IsValid(string s)
        {
            double o;
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out o))
            {
                return true;
            }
            return false;
        }

        public string GetAttributeName()
        {
            return name;
        }

        public IAttribute CreateAttribute(string rawValue)
        {
            return new NumericAttribute(double.Parse(rawValue, NumberStyles.Any, CultureInfo.InvariantCulture), this);
        }
    }
}
