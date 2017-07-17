using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.learning.framework
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class Example : IToString, IEquatable
    {
        IMap<string, Attribute> attributes;

        private Attribute targetAttribute;

        public Example(IMap<string, Attribute> attributes,
                       Attribute targetAttribute)
        {
            this.attributes = attributes;
            this.targetAttribute = targetAttribute;
        }

        public string getAttributeValueAsString(string attributeName)
        {
            return attributes.Get(attributeName).valueAsString();
        }

        public double getAttributeValueAsDouble(string attributeName)
        {
            Attribute attribute = attributes.Get(attributeName);
            if (attribute == null || !(attribute is NumericAttribute))
            {
                throw new RuntimeException("cannot return numerical value for non numeric attribute");
            }
            return ((NumericAttribute)attribute).valueAsDouble();
        }

        public override string ToString()
        {
            return attributes.ToString();
        }

        public string targetValue()
        {
            return getAttributeValueAsString(targetAttribute.name());
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if ((o == null) || (this.GetType() != o.GetType()))
            {
                return false;
            }
            Example other = (Example)o;
            return attributes.Equals(other.attributes);
        }

        public override int GetHashCode()
        {
            return attributes.GetHashCode();
        }

        public Example numerize(IMap<string, IMap<string, int>> attrValueToNumber)
        {
            IMap<string, Attribute> numerizedExampleData = Factory.CreateMap<string, Attribute>();
            foreach (string key in attributes.GetKeys())
            {
                Attribute attribute = attributes.Get(key);
                if (attribute is StringAttribute)
                {
                    int correspondingNumber = attrValueToNumber.Get(key).Get(attribute.valueAsString());
                    NumericAttributeSpecification spec = new NumericAttributeSpecification(key);
                    numerizedExampleData.Put(key, new NumericAttribute(correspondingNumber, spec));
                }
                else
                {// Numeric Attribute
                    numerizedExampleData.Put(key, attribute);
                }
            }
            return new Example(numerizedExampleData, numerizedExampleData.Get(targetAttribute.name()));
        }
    } 
}
