namespace tvn.cosine.ai.learning.framework
{ 
    public class NumericAttribute : IAttribute
    {
        double value;

        private NumericAttributeSpecification spec;

        public NumericAttribute(double rawValue, NumericAttributeSpecification spec)
        {
            this.value = rawValue;
            this.spec = spec;
        }

        public string ValueAsString()
        {
            return value.ToString();
        }

        public string Name()
        {
            return spec.GetAttributeName().Trim();
        }

        public double valueAsDouble()
        {
            return value;
        }
    }
}
