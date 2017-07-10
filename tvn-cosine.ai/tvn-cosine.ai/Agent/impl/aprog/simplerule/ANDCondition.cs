using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /**
     * Implementation of an AND condition.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class ANDCondition : Condition
    {

        private Condition left;

        private Condition right;

        public ANDCondition(Condition leftCon, Condition rightCon)
        {
           Debug.Assert(null != leftCon);
           Debug.Assert(null != rightCon);

            left = leftCon;
            right = rightCon;
        }
         
    public override bool evaluate(ObjectWithDynamicAttributes<object, object> p)
        {
            return (left.evaluate(p) && right.evaluate(p));
        }
         
    public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            return sb.Append("[").Append(left).Append(" && ").Append(right)
                    .Append("]").ToString();
        }
    }

}
