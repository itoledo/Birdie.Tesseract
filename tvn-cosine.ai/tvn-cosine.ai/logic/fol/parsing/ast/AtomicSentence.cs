using tvn.cosine.collections.api;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public interface AtomicSentence : Sentence
    {
        new ICollection<Term> getArgs();
        new AtomicSentence copy();
    }
}
