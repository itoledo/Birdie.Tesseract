using tvn.cosine.collections.api;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.otter
{
    public interface ClauseFilter
    {
        ISet<Clause> filter(ISet<Clause> clauses);
    }
}
