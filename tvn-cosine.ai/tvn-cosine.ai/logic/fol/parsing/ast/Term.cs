using tvn.cosine.collections.api;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public interface Term : FOLNode
    {
        new ICollection<Term> getArgs();

        new Term copy();
    }
}
