using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public interface Term : FOLNode
    {
        new ICollection<Term> getArgs();

        new Term copy();
    }
}
