using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.otter
{
    public interface ClauseFilter
    {
        ISet<Clause> filter(ISet<Clause> clauses);
    }
}
