using System.Collections.Generic;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface AtomicSentence : Sentence
    {
        IList<Term> getArgs();

        new AtomicSentence copy();
    }
}
