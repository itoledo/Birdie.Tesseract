using System.Collections.Generic;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.otter.defaultimpl
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class DefaultClauseFilter : ClauseFilter
    {
        public DefaultClauseFilter()
        { }

        //
        // START-ClauseFilter
        public ISet<Clause> filter(ISet<Clause> clauses)
        {
            return clauses;
        }

        // END-ClauseFilter
        //
    }
}
