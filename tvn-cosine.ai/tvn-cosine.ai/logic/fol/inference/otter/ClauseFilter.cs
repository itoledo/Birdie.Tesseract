namespace tvn.cosine.ai.logic.fol.inference.otter
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface ClauseFilter
    {
        Set<Clause> filter(Set<Clause> clauses);
    }
}
