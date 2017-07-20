using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.learning.framework
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class DataSetSpecification
    {
        IQueue<AttributeSpecification> attributeSpecifications;

        private string targetAttribute;

        public DataSetSpecification()
        {
            this.attributeSpecifications = Factory.CreateQueue<AttributeSpecification>();
        }

        public virtual bool isValid(IQueue<string> uncheckedAttributes)
        {
            if (attributeSpecifications.Size() != uncheckedAttributes.Size())
            {
                throw new RuntimeException("size mismatch specsize = "
                        + attributeSpecifications.Size() + " attrbutes size = "
                        + uncheckedAttributes.Size());
            }

            for (int i = 0; i < attributeSpecifications.Size(); ++i)
            {
                if (!(attributeSpecifications.Get(i).isValid(uncheckedAttributes.Get(i))))
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

        public virtual IQueue<string> getPossibleAttributeValues(string attributeName)
        {
            foreach (AttributeSpecification _as in attributeSpecifications)
            {
                if (_as.getAttributeName().Equals(attributeName))
                {
                    return ((StringAttributeSpecification)_as).possibleAttributeValues();
                }
            }
            throw new RuntimeException("No such attribute" + attributeName);
        }

        public virtual IQueue<string> getAttributeNames()
        {
            IQueue<string> names = Factory.CreateQueue<string>();
            foreach (AttributeSpecification _as in attributeSpecifications)
            {
                names.Add(_as.getAttributeName());
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
            throw new RuntimeException("no attribute spec for  " + name);
        }

        public virtual void defineNumericAttribute(string name)
        {
            attributeSpecifications.Add(new NumericAttributeSpecification(name));
        }

        public virtual IQueue<string> getNamesOfStringAttributes()
        {
            IQueue<string> names = Factory.CreateQueue<string>();
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
