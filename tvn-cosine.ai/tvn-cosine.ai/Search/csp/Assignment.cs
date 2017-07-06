using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace tvn.cosine.ai.search.csp
{
    /**
     * An assignment assigns values to some or all variables of a CSP.
     *
     * @author Ruediger Lunde
     */
    public class Assignment<VAR, VAL>  
        where VAR : Variable
    {
        /**
         * Maps variables to their assigned values.
         */
        private IDictionary<VAR, VAL> variableToValueMap = new Dictionary<VAR, VAL>();

        public List<VAR> getVariables()
        {
            return variableToValueMap.Keys.ToList();
        }

        public VAL getValue(VAR var)
        {
            return variableToValueMap[var];
        }

        public VAL add(VAR var, VAL value)
        {
            Debug.Assert(value != null);
            variableToValueMap.Add(var, value);
            return value;
        }

        public VAL remove(VAR var)
        {
            var val = variableToValueMap[var];
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
        public bool isConsistent(List<Constraint<VAR, VAL>> constraints)
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
        public bool isComplete(List<VAR> vars)
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

        public Assignment<VAR, VAL> Clone()
        {
            Assignment<VAR, VAL> result;

            result = (Assignment<VAR, VAL>)MemberwiseClone();
            result.variableToValueMap = new Dictionary<VAR, VAL>(variableToValueMap);

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
                result.Append(entry.Key)
                      .Append("=")
                      .Append(entry.Value);
                comma = true;
            }
            result.Append("}");

            return result.ToString();
        }
    }
}
