using System.Text;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.search.csp
{
    /**
     * An assignment assigns values to some or all variables of a CSP.
     *
     * @author Ruediger Lunde
     */
    public class Assignment<VAR, VAL> : ICloneable<Assignment<VAR, VAL>>
        where VAR : Variable
    {
        /**
         * Maps variables to their assigned values.
         */
        private IMap<VAR, VAL> variableToValueMap = Factory.CreateMap<VAR, VAL>();

        public IQueue<VAR> getVariables()
        {
            return Factory.CreateQueue<VAR>(variableToValueMap.GetKeys());
        }

        public VAL getValue(VAR var)
        {
            return variableToValueMap.Get(var);
        }

        public VAL add(VAR var, VAL value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value cannot be null");
            }

            variableToValueMap.Put(var, value);
            return value;
        }

        public VAL remove(VAR var)
        {
            VAL val = variableToValueMap.Get(var);
            variableToValueMap.Remove(var);
            return val;
        }

        public bool contains(VAR var)
        {
            return variableToValueMap.ContainsKey(var);
        }

        /**
         * Returns true if this assignment does not violate any constraints of
         * <code>constraints</code>.
         */
        public bool isConsistent(IQueue<Constraint<VAR, VAL>> constraints)
        {
            foreach (Constraint<VAR, VAL> cons in constraints)
                if (!cons.isSatisfiedWith(this))
                    return false;
            return true;
        }

        /**
         * Returns true if this assignment assigns values to every variable of
         * <code>vars</code>.
         */
        public bool isComplete(IQueue<VAR> vars)
        {
            foreach (VAR var in vars)
                if (!contains(var))
                    return false;
            return true;
        }

        /**
         * Returns true if this assignment is consistent as well as complete with
         * respect to the given CSP.
         */
        public bool isSolution(CSP<VAR, VAL> csp)
        {
            return isConsistent(csp.getConstraints()) && isComplete(csp.getVariables());
        }

        public Assignment<VAR, VAL> clone()
        {
            Assignment<VAR, VAL> result;

            result = new Assignment<VAR, VAL>();
            result.variableToValueMap = Factory.CreateMap<VAR, VAL>(variableToValueMap);

            return result;
        }


        public override string ToString()
        {
            bool comma = false;
            StringBuilder result = new StringBuilder("{");
            foreach (var entry in variableToValueMap)
            {
                if (comma)
                    result.Append(", ");
                result.Append(entry.GetKey()).Append("=").Append(entry.GetValue());
                comma = true;
            }
            result.Append("}");
            return result.ToString();
        }
    }
}
