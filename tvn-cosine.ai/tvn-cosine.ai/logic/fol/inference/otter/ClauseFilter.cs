using System.Collections.Generic;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.otter
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface ClauseFilter
    {
        ISet<Clause> filter(ISet<Clause> clauses);
    }
}
