using System;
using System.Collections.Generic;

namespace tvn.cosine.ai.learning.framework
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class Example
    {
        IDictionary<string, Attribute> attributes;

        private Attribute targetAttribute;

        public Example(IDictionary<string, Attribute> attributes, Attribute targetAttribute)
        {
            this.attributes = attributes;
            this.targetAttribute = targetAttribute;
        }

        public string getAttributeValueAsString(string attributeName)
        {
            return attributes[attributeName].valueAsString();
        }

        public double getAttributeValueAsDouble(string attributeName)
        {
            Attribute attribute = attributes[attributeName];
            if (attribute == null || !(attribute is NumericAttribute))
            {
                throw new Exception("cannot return numerical value for non numeric attribute");
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

        public override bool Equals(Object o)
        {
            if (this == o)
            {
                return true;
            }
            if ((o == null) || (GetType() != o.GetType()))
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

        public Example numerize(IDictionary<string, IDictionary<string, int>> attrValueToNumber)
        {
            IDictionary<string, Attribute> numerizedExampleData = new Dictionary<string, Attribute>();
            foreach (string key in attributes.Keys)
            {
                Attribute attribute = attributes[key];
                if (attribute is StringAttribute) {
                    int correspondingNumber = attrValueToNumber[key][attribute.valueAsString()];
                    NumericAttributeSpecification spec = new NumericAttributeSpecification(
                            key);
                    numerizedExampleData.Add(key, new NumericAttribute(
                            correspondingNumber, spec));
                } else {// Numeric Attribute
                    numerizedExampleData.Add(key, attribute);
                }
            }
            return new Example(numerizedExampleData,
                    numerizedExampleData[targetAttribute.name()]);
        }
    }

}
