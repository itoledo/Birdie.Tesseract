using System.Collections.Generic;

namespace tvn.cosine.ai.search.csp.heuristics
{
    /**
     * Implements the minimum-remaining-values heuristic.
     */
    public class MrvHeuristic<VAR, VAL> : VariableSelection<VAR, VAL>
        where VAR : Variable
    {
        /** Returns variables from <code>vars</code> which are the best with respect to MRV. */
        public List<VAR> apply(CSP<VAR, VAL> csp, List<VAR> vars)
        {
            List<VAR> result = new List<VAR>();
            int mrv = int.MaxValue;
            foreach (VAR var in vars)
            {
                int rv = csp.getDomain(var).size();
                if (rv <= mrv)
                {
                    if (rv < mrv)
                    {
                        result.Clear();
                        mrv = rv;
                    }
                    result.Add(var);
                }
            }
            return result;
        }
    }
}
