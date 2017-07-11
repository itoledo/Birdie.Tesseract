using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.otter
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface ClauseSimplifier
    {
        Clause simplify(Clause c);
    }
}
