namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public interface Term : FOLNode
    {
        List<Term> getArgs();

        Term copy();
    }
}
