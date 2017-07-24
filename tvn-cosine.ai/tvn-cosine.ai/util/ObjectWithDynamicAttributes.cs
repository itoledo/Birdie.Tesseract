using System.Text;
using tvn.cosine.ai.common.api;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.util
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     * @author Mike Stampone
     */
    public abstract class ObjectWithDynamicAttributes : IEquatable, IHashable, IStringable
    {
        private IMap<object, object> attributes = CollectionFactory.CreateInsertionOrderedMap<object, object>();

        /**
         * By default, returns the simple name of the underlying class as given in the source code.
         * 
         * @return the simple name of the underlying class
         */
        public virtual string describeType()
        {
            return GetType().Name;
        }

        /**
         * Returns a string representation of the object's current attributes
         * 
         * @return a string representation of the object's current attributes
         */
        public virtual string describeAttributes()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");
            bool first = true;
            foreach (object key in attributes.GetKeys())
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(", ");
                }

                sb.Append(key);
                sb.Append("==");
                sb.Append(attributes.Get(key));
            }
            sb.Append("]");

            return sb.ToString();
        }

        /**
         * Returns an unmodifiable view of the object's key set
         * 
         * @return an unmodifiable view of the object's key set
         */
        public virtual ISet<object> getKeySet()
        {
            return CollectionFactory.CreateReadOnlySet<object>(attributes.GetKeys());
        }

        /**
         * Associates the specified value with the specified attribute key. If the
         * ObjectWithDynamicAttributes previously contained a mapping for the
         * attribute key, the old value is replaced.
         * 
         * @param key
         *            the attribute key
         * @param value
         *            the attribute value
         */
        public virtual void setAttribute(object key, object value)
        {
            attributes.Put(key, value);
        }

        /**
         * Returns the value of the specified attribute key, or null if the
         * attribute was not found.
         * 
         * @param key
         *            the attribute key
         * 
         * @return the value of the specified attribute name, or null if not found.
         */
        public virtual object getAttribute(object key)
        {
            return attributes.Get(key);
        }

        /**
         * Removes the attribute with the specified key from this
         * ObjectWithDynamicAttributes.
         * 
         * @param key
         *            the attribute key
         */
        public virtual void removeAttribute(object key)
        {
            attributes.Remove(key);
        }

        public override bool Equals(object o)
        {
            return o != null
                && GetType() == o.GetType()
                && attributes.Equals(((ObjectWithDynamicAttributes)o).attributes);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return describeType() + describeAttributes();
        }
    }
}
