using System.Collections.Generic; 

namespace tvn.cosine.ai.search.csp.heuristics
{
    public class MrvDegHeuristic<VAR, VAL> : VariableSelection<VAR, VAL>
        where VAR : Variable
    {
        public List<VAR> apply(CSP<VAR, VAL> csp, List<VAR> vars)
        {
            return new DegHeuristic<VAR, VAL>().apply(csp, new MrvHeuristic<VAR, VAL>().apply(csp, vars));
        }
    }
}
