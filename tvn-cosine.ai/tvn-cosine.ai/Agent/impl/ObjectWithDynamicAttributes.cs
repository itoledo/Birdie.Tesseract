using System;
using System.Collections.Generic;
using System.Text;

namespace tvn.cosine.ai.agent.impl
{
    public abstract class ObjectWithDynamicAttributes<KEY, VALUE>
    {
        private readonly IDictionary<KEY, VALUE> attributes = new Dictionary<KEY, VALUE>();

        /// <summary>
        /// By default, returns the simple name of the underlying class as given in the source code.
        /// </summary>
        /// <returns>the simple name of the underlying class</returns>
        public virtual string DescribeType()
        {
            return GetType().Name;
        }

        /// <summary>
        /// Returns a string representation of the object's current attributes
        /// </summary>
        /// <returns>a string representation of the object's current attributes</returns>
        public virtual string DescribeAttributes()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            bool first = true;
            foreach (KEY key in attributes.Keys)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    stringBuilder.Append(", ");
                }

                stringBuilder.Append("\"");
                stringBuilder.Append(key);
                stringBuilder.Append("\": ");
                stringBuilder.Append("\"");
                stringBuilder.Append(attributes[key]);
                stringBuilder.Append("\": ");
            }
            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Returns a copy of the object's key set
        /// </summary>
        /// <returns>a copy of the object's key set</returns>
        public virtual ISet<KEY> GetKeySet()
        {
            return new HashSet<KEY>(attributes.Keys);
        }

        /// <summary>
        /// Associates the specified value with the specified attribute key. If the
        /// ObjectWithDynamicAttributes previously contained a mapping for the
        /// attribute key, the old value is replaced.
        /// </summary>
        /// <param name="key">the attribute key</param>
        /// <param name="value">the attribute value</param>
        public virtual void SetAttribute(KEY key, VALUE value)
        {
            attributes[key] = value;
        }

        /// <summary>
        /// Returns the value of the specified attribute key, or null if the attribute was not found.
        /// </summary>
        /// <param name="key">the attribute key</param>
        /// <returns>the value of the specified attribute name, or null if not found.</returns>
        public virtual VALUE GetAttribute(KEY key)
        {
            if (attributes.ContainsKey(key))
            {
                return attributes[key];
            }

            return default(VALUE);
        }

        /// <summary>
        /// Removes the attribute with the specified key from this ObjectWithDynamicAttributes.
        /// </summary>
        /// <param name="key">the attribute key</param>
        public virtual void RemoveAttribute(KEY key)
        {
            attributes.Remove(key);
        }

        public override bool Equals(object o)
        {
            if (o != null
             && GetType() == o.GetType())
            {
                ObjectWithDynamicAttributes<KEY, VALUE> objAttributes = o as ObjectWithDynamicAttributes<KEY, VALUE>;
                if (objAttributes.attributes.Count != attributes.Count) return false;

                foreach (var attribute in objAttributes.attributes)
                {
                    if (!(attributes.ContainsKey(attribute.Key)
                       && !attributes[attribute.Key].Equals(attribute.Value)))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return attributes.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{{\"Name\": \"{0}\", \"Attributes\": \"{1}\"}}", DescribeType(), DescribeAttributes());
        }
    }
}
