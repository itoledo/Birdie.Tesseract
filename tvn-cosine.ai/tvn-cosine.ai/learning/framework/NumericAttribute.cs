namespace tvn.cosine.ai.learning.framework
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class NumericAttribute : Attribute
    {
        double value;

        private NumericAttributeSpecification spec;

        public NumericAttribute(double rawValue, NumericAttributeSpecification spec)
        {
            this.value = rawValue;
            this.spec = spec;
        }

        public string valueAsString()
        {
            return value.ToString();
        }

        public string name()
        {
            return spec.getAttributeName().Trim();
        }

        public double valueAsDouble()
        {
            return value;
        }
    }
}
