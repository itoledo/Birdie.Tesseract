using System;
using System.Text;

namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /// <summary>
    /// Implementation of an EQUALity condition.
    /// </summary>
    public class EQUALCondition : Condition
    {
        private string key;
        private object value;

        public EQUALCondition(string key, object value)
        {
            if (null == key
             || null == value)
            {
                throw new ArgumentNullException("key, value cannot be null.");
            }

            this.key = key;
            this.value = value;
        }

        public override bool Evaluate(ObjectWithDynamicAttributes<string, object> p)
        {
            return value.Equals(p.GetAttribute(key));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append(key)
                     .Append("==")
                     .Append(value)
                     .ToString();
        }
    }
}
