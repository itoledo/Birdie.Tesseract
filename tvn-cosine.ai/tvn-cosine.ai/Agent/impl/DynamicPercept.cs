using System; 

namespace tvn.cosine.ai.agent.impl
{ 
    public class DynamicPercept : ObjectWithDynamicAttributes<string, object>, Percept
    {
        public DynamicPercept()
        { }

        public override string DescribeType()
        {
            return typeof(Percept).Name;
        }
         
        /// <summary>
        /// Constructs a DynamicPercept with one attribute
        /// </summary>
        /// <param name="key1">the attribute key</param>
        /// <param name="value1">the attribute value</param>
        public DynamicPercept(string key1, object value1)
        {
            SetAttribute(key1, value1);
        }
         
        /// <summary>
        /// Constructs a DynamicPercept with two attributes
        /// </summary>
        /// <param name="key1">the first attribute key</param>
        /// <param name="value1">the first attribute value</param>
        /// <param name="key2">the second attribute key</param>
        /// <param name="value2">the second attribute value</param>
        public DynamicPercept(string key1, object value1, string key2, object value2)
        {
            SetAttribute(key1, value1);
            SetAttribute(key2, value2);
        }
         
        /// <summary>
        /// Constructs a DynamicPercept with an array of attributes
        /// </summary>
        /// <param name="keys">the array of attribute keys</param>
        /// <param name="values">the array of attribute values</param>
        public DynamicPercept(string[] keys, object[] values)
        {
            if (!(keys.Length == values.Length))
            {
                throw new ArgumentOutOfRangeException("keys and values does not match.");
            }

            for (int i = 0; i < keys.Length; ++i)
            {
                SetAttribute(keys[i], values[i]);
            }
        }
    }
}
