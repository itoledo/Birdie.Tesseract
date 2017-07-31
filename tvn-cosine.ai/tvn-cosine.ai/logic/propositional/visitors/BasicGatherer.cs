using tvn.cosine.collections.api;
using tvn.cosine.ai.logic.propositional.parsing;
using tvn.cosine.ai.logic.propositional.parsing.ast;
using tvn.cosine.ai.util;

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
            return SetOps.union(arg, s.getSimplerSentence(0).accept(this, arg));
        }
         
        public virtual ISet<T> visitBinarySentence(ComplexSentence s, ISet<T> arg)
        {
            ISet<T> termunion = SetOps.union(
                    s.getSimplerSentence(0).accept(this, arg), s
                            .getSimplerSentence(1).accept(this, arg));
            return SetOps.union(arg, termunion);
        }
    }
}
