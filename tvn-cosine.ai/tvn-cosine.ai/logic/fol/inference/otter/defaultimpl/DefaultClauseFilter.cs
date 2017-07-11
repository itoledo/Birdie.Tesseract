using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.logic.fol.inference.otter.defaultimpl
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class DefaultClauseFilter : ClauseFilter
    {

        public DefaultClauseFilter()
        {

        }

        //
        // START-ClauseFilter
        public Set<Clause> filter(Set<Clause> clauses)
        {
            return clauses;
        }

        // END-ClauseFilter
        //
    }
}
