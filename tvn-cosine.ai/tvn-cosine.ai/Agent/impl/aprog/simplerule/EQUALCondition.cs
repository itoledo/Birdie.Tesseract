using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /**
     * Implementation of an EQUALity condition.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class EQUALCondition : Condition
    {
        private object key;
        private object value;

        public EQUALCondition(object key, object value)
        {
            Debug.Assert(null != key);
            Debug.Assert(null != value);

            this.key = key;
            this.value = value;
        }
         
    public override bool evaluate(ObjectWithDynamicAttributes<object, object> p)
        {
            return value.Equals(p.getAttribute(key));
        }
         
    public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append(key).Append("==").Append(value).ToString();
        }
    }
}
