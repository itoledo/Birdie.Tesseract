using System.Collections.Generic;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public interface Term : FOLNode
    {
        IList<Term> getArgs();

        new Term copy();
    }

}
