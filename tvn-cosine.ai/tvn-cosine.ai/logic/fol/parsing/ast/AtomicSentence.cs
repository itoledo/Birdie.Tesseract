namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public interface AtomicSentence extends Sentence
    {
        List<Term> getArgs();

        AtomicSentence copy();
    }
}
