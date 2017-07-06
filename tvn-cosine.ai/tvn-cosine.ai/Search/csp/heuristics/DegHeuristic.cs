using System.Collections.Generic;

namespace tvn.cosine.ai.search.csp.heuristics
{
    /**
       * Implements the degree heuristic. Constraints with arbitrary scope size are supported.
       */
    public class DegHeuristic<VAR, VAL> : VariableSelection<VAR, VAL>
        where VAR : Variable
    {

        /** Returns variables from <code>vars</code> which are the best with respect to DEG. */
        public List<VAR> apply(CSP<VAR, VAL> csp, List<VAR> vars)
        {
            List<VAR> result = new List<VAR>();
            int maxDegree = -1;
            foreach (VAR var in vars)
            {
                int degree = csp.getConstraints(var).Count;
                if (degree >= maxDegree)
                {
                    if (degree > maxDegree)
                    {
                        result.Clear();
                        maxDegree = degree;
                    }
                    result.Add(var);
                }
            }
            return result;
        }
    }
}
