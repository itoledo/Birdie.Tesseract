using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.learning.framework
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class StringAttributeSpecification : AttributeSpecification
    {
        string attributeName;

        IQueue<string> attributePossibleValues;


        public StringAttributeSpecification(string attributeName,
                IQueue<string> attributePossibleValues)
        {
            this.attributeName = attributeName;
            this.attributePossibleValues = attributePossibleValues;
        }

        public StringAttributeSpecification(string attributeName,
                                            string[] attributePossibleValues)
            : this(attributeName, Factory.CreateQueue<string>(attributePossibleValues))
        { }

        public bool isValid(string value)
        {
            return (attributePossibleValues.Contains(value));
        }

        /**
         * @return Returns the attributeName.
         */
        public string getAttributeName()
        {
            return attributeName;
        }

        public IQueue<string> possibleAttributeValues()
        {
            return attributePossibleValues;
        }

        public Attribute createAttribute(string rawValue)
        {
            return new StringAttribute(rawValue, this);
        }
    }
}
