using System.Collections.Generic;
using System.Text;

namespace tvn_cosine.ai
{
    public abstract class DynamicAttributesBase<KEY, VALUE>
        : IDynamicAttributes<KEY, VALUE>
    {
        public DynamicAttributesBase()
        {
            Attributes = new Dictionary<KEY, VALUE>();
        }

        public IDictionary<KEY, VALUE> Attributes { get; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            bool first = true;
            foreach (var key in Attributes.Keys)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    stringBuilder.Append(", ");
                }

                stringBuilder.Append(key);
                stringBuilder.Append("==");
                stringBuilder.Append(Attributes[key]);
            }
            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }
    }

    public abstract class DynamicAttributesBase : DynamicAttributesBase<object, object>, IDynamicAttributes
    {
    }
}
