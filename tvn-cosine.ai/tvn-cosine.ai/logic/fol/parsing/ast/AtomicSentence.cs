namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public interface AtomicSentence : Sentence
    {
        IQueue<Term> getArgs();

        AtomicSentence copy();
    }
}
