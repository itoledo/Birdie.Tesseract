using System.Collections.Generic;
using System.Linq;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.search.csp.heuristics
{
    /**
    * Implements the least constraining value heuristic.
    */
    public class LcvHeuristic<VAR, VAL> : ValueSelection<VAR, VAL>
            where VAR : Variable
    {

        /** Returns the values of Dom(var) in a special order. The least constraining value comes first. */
        public List<VAL> apply(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR var)
        {
            List<Pair<VAL, int>> pairs = new List<Pair<VAL, int>>();
            foreach (VAL value in csp.getDomain(var))
            {
                int num = countLostValues(csp, assignment, var, value);
                pairs.Add(new Pair<VAL, int>(value, num));
            }
            return pairs.OrderBy(x => x.Second).Select(x => x.First).ToList();
        }

        /**
         * Ignores constraints which are not binary.
         */
        private int countLostValues(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR var, VAL value)
        {
            int result = 0;
            Assignment<VAR, VAL> assign = new Assignment<VAR, VAL>();
            assign.add(var, value);
            foreach (Constraint<VAR, VAL> constraint in csp.getConstraints(var))
            {
                if (constraint.getScope().Count == 2)
                {
                    VAR neighbor = csp.getNeighbor(var, constraint);
                    if (!assignment.contains(neighbor))
                        foreach (VAL nValue in csp.getDomain(neighbor))
                        {
                            assign.add(neighbor, nValue);
                            if (!constraint.isSatisfiedWith(assign))
                            {
                                ++result;
                            }
                        }
                }
            }
            return result;
        }
    }
}
