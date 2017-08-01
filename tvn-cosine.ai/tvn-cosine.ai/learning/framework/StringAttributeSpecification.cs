using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.learning.framework.api;
using tvn.cosine.text.api;
using tvn.cosine.text;

namespace tvn.cosine.ai.learning.framework
{
    public class StringAttributeSpecification : IAttributeSpecification
    {
        string attributeName;

        ICollection<string> attributePossibleValues;


        public StringAttributeSpecification(string attributeName,
                ICollection<string> attributePossibleValues)
        {
            this.attributeName = attributeName;
            this.attributePossibleValues = attributePossibleValues;
        }

        public StringAttributeSpecification(string attributeName,
                                            string[] attributePossibleValues)
            : this(attributeName, CollectionFactory.CreateQueue<string>(attributePossibleValues))
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

        public ICollection<string> possibleAttributeValues()
        {
            return attributePossibleValues;
        }

        public IAttribute CreateAttribute(string rawValue)
        {
            return new StringAttribute(rawValue, this);
        }

        public override string ToString()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();
            sb.Append('[');
            sb.Append(attributeName);
            sb.Append("=[");
            bool first = true;
            foreach(string value in attributePossibleValues)
            {
                if (!first)
                {
                    sb.Append(", ");
                }
                sb.Append(value);
                first = false;
            }

            sb.Append("]]");
            return sb.ToString();
        }
    }
}
