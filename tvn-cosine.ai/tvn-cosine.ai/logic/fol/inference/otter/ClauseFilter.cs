namespace tvn.cosine.ai.logic.fol.inference.otter
{
    public interface ClauseFilter
    {
        Set<Clause> filter(Set<Clause> clauses);
    }
}
