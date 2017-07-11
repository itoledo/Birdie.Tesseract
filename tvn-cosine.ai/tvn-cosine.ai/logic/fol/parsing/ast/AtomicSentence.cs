using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface AtomicSentence extends Sentence
    {
        List<Term> getArgs();

        AtomicSentence copy();
    }
}
