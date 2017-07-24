using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.otter.defaultimpl
{
    public class DefaultClauseFilter : ClauseFilter
    {
        public DefaultClauseFilter()
        { }

        public ISet<Clause> filter(ISet<Clause> clauses)
        {
            return clauses;
        }
    }
}
