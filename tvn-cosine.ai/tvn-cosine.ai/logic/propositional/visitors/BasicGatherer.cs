using System.Collections.Generic;
using System.Linq;
using tvn.cosine.ai.logic.propositional.parsing;
using tvn.cosine.ai.logic.propositional.parsing.ast;

namespace tvn.cosine.ai.logic.propositional.visitors
{
    /**
     * Super class of Visitors that are "read only" and gather information from an
     * existing parse tree .
     * 
     * @author Ravi Mohan
     * 
     * @param <T>
     *            the type of elements to be gathered.
     */
    public abstract class BasicGatherer<T> : PLVisitor<ISet<T>, ISet<T>>
    {
        public virtual ISet<T> visitPropositionSymbol(PropositionSymbol s, ISet<T> arg)
        {
            return arg;
        }

        public virtual ISet<T> visitUnarySentence(ComplexSentence s, ISet<T> arg)
        {
            return new HashSet<T>(arg.Union(s.getSimplerSentence(0).accept(this, arg)));
        }

        public virtual ISet<T> visitBinarySentence(ComplexSentence s, ISet<T> arg)
        {
            ISet<T> termunion = new HashSet<T>(s.getSimplerSentence(0).accept(this, arg).Union(s
                            .getSimplerSentence(1).accept(this, arg)));
            return new HashSet<T>(arg.Union(termunion));
        }
    }

}
