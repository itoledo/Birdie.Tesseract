using System.Collections.Generic;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol
{
    /**
  * @author Ravi Mohan
  * 
  */
    public class PredicateCollector : FOLVisitor
    {
        public PredicateCollector()
        {

        }

        public IList<Predicate> getPredicates(Sentence s)
        {
            return (IList<Predicate>)s.accept(this, new List<Predicate>());
        }


        public object visitPredicate(Predicate p, object arg)
        {
           IList<Predicate> predicates = (IList<Predicate>)arg;
            predicates.Add(p);
            return predicates;
        }

        public object visitTermEquality(TermEquality equality, object arg)
        {
            return arg;
        }

        public object visitVariable(Variable variable, object arg)
        {
            return arg;
        }

        public object visitConstant(Constant constant, object arg)
        {
            return arg;
        }

        public object visitFunction(Function function, object arg)
        {
            return arg;
        }

        public object visitNotSentence(NotSentence sentence, object arg)
        {
            sentence.getNegated().accept(this, arg);
            return arg;
        }

        public object visitConnectedSentence(ConnectedSentence sentence, object arg)
        {
            sentence.getFirst().accept(this, arg);
            sentence.getSecond().accept(this, arg);
            return arg;
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence,
                object arg)
        {
            sentence.getQuantified().accept(this, arg);
            return arg;
        }
    }
}
