using System;
using System.Collections.Generic;

namespace tvn.cosine.ai.learning.framework
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class DataSetSpecification
    {
        List<AttributeSpecification> attributeSpecifications;

        private string targetAttribute;

        public DataSetSpecification()
        {
            this.attributeSpecifications = new List<AttributeSpecification>();
        }

        public bool isValid(List<string> uncheckedAttributes)
        {
            if (attributeSpecifications.Count != uncheckedAttributes.Count)
            {
                throw new Exception("size mismatch specsize = "
                        + attributeSpecifications.Count + " attrbutes size = "
                        + uncheckedAttributes.Count);
            } 
            int min = Math.Min(attributeSpecifications.Count, uncheckedAttributes.Count);
            for (int i = 0; i < min; ++i)
            {
                if (!(attributeSpecifications[i].isValid(uncheckedAttributes[i])))
                {
                    return false;
                }
            } 

            return true;
        }

        /**
         * @return Returns the targetAttribute.
         */
        public string getTarget()
        {
            return targetAttribute;
        }

        public List<string> getPossibleAttributeValues(string attributeName)
        {
            foreach (AttributeSpecification ass in attributeSpecifications)
            {
                if (ass.getAttributeName().Equals(attributeName))
                {
                    return ((StringAttributeSpecification)ass)
                            .possibleAttributeValues();
                }
            }
            throw new Exception("No such attribute" + attributeName);
        }

        public List<string> getAttributeNames()
        {
            List<string> names = new List<string>();
            foreach (AttributeSpecification ass in attributeSpecifications)
            {
                names.Add(ass.getAttributeName());
            }
            return names;
        }

        public void defineStringAttribute(string name, string[] attributeValues)
        {
            attributeSpecifications.Add(new StringAttributeSpecification(name, attributeValues));
            setTarget(name);// target defaults to last column added
        }

        /**
         * @param target
         *            The targetAttribute to set.
         */
        public void setTarget(string target)
        {
            this.targetAttribute = target;
        }

        public AttributeSpecification getAttributeSpecFor(string name)
        {
            foreach (AttributeSpecification spec in attributeSpecifications)
            {
                if (spec.getAttributeName().Equals(name))
                {
                    return spec;
                }
            }
            throw new Exception("no attribute spec for  " + name);
        }

        public void defineNumericAttribute(string name)
        {
            attributeSpecifications.Add(new NumericAttributeSpecification(name));
        }

        public List<string> getNamesOfStringAttributes()
        {
            List<string> names = new List<string>();
            foreach (AttributeSpecification spec in attributeSpecifications)
            {
                if (spec is StringAttributeSpecification)
                {
                    names.Add(spec.getAttributeName());
                }
            }
            return names;
        }
    }
}
