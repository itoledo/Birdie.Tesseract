using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public interface AtomicSentence : Sentence
    {
        new IQueue<Term> getArgs();
        new AtomicSentence copy();
    }
}
