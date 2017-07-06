using System.Collections.Generic;

namespace tvn.cosine.ai.search.csp.heuristics
{
    public interface ValueSelection<VAR, VAL>
        where VAR : Variable
    {
        List<VAL> apply(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR var);
    }
}
