using System.Collections.Generic;

namespace tvn.cosine.ai.search.csp.heuristics
{
    public interface VariableSelection<VAR, VAL>
        where VAR : Variable
    {
        List<VAR> apply(CSP<VAR, VAL> csp, List<VAR> vars);
    }
}
