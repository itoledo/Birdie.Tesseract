using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.learning.framework
{ 
    public class StringAttributeSpecification : IAttributeSpecification
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

        public bool IsValid(string value)
        {
            return (attributePossibleValues.Contains(value));
        }

        /// <summary>
        /// Returns the attributeName.
        /// </summary>
        /// <returns></returns>
        public string GetAttributeName()
        {
            return attributeName;
        }

        public IQueue<string> possibleAttributeValues()
        {
            return attributePossibleValues;
        }

        public IAttribute CreateAttribute(string rawValue)
        {
            return new StringAttribute(rawValue, this);
        }
    }
}
