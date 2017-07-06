using System.Collections.Generic;
using System.Linq;

namespace tvn.cosine.ai.learning.framework
{
    /**
     * @author Ravi Mohan
     * 
     */
    public class StringAttributeSpecification : AttributeSpecification
    {
        string attributeName;

        List<string> attributePossibleValues;


        public StringAttributeSpecification(string attributeName,
                List<string> attributePossibleValues)
        {
            this.attributeName = attributeName;
            this.attributePossibleValues = attributePossibleValues;
        }

        public StringAttributeSpecification(string attributeName, string[] attributePossibleValues)
            : this(attributeName, attributePossibleValues.ToList())
        {

        }

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

        public List<string> possibleAttributeValues()
        {
            return attributePossibleValues;
        }

        public Attribute createAttribute(string rawValue)
        {
            return new StringAttribute(rawValue, this);
        }
    } 
}
