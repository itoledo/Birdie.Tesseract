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
        IList<AttributeSpecification> attributeSpecifications;

        private string targetAttribute;

        public DataSetSpecification()
        {
            this.attributeSpecifications = new List<AttributeSpecification>();
        }

        public virtual bool isValid(IList<string> uncheckedAttributes)
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
        public virtual string getTarget()
        {
            return targetAttribute;
        }

        public virtual IList<string> getPossibleAttributeValues(string attributeName)
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

        public virtual IList<string> getAttributeNames()
        {
            IList<string> names = new List<string>();
            foreach (AttributeSpecification ass in attributeSpecifications)
            {
                names.Add(ass.getAttributeName());
            }
            return names;
        }

        public virtual void defineStringAttribute(string name, string[] attributeValues)
        {
            attributeSpecifications.Add(new StringAttributeSpecification(name, attributeValues));
            setTarget(name);// target defaults to last column added
        }

        /**
         * @param target
         *            The targetAttribute to set.
         */
        public virtual void setTarget(string target)
        {
            this.targetAttribute = target;
        }

        public virtual AttributeSpecification getAttributeSpecFor(string name)
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

        public virtual void defineNumericAttribute(string name)
        {
            attributeSpecifications.Add(new NumericAttributeSpecification(name));
        }

        public virtual IList<string> getNamesOfStringAttributes()
        {
            IList<string> names = new List<string>();
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
