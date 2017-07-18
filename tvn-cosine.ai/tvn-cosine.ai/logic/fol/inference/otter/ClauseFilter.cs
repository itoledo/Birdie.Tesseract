namespace tvn.cosine.ai.logic.fol.inference.otter
{
    public interface ClauseFilter
    {
        ISet<Clause> filter(Set<Clause> clauses);
    }
}
