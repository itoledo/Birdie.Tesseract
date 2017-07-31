using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol
{
    public class PredicateCollector : FOLVisitor
    {
        public PredicateCollector()
        { }

        public ICollection<Predicate> getPredicates(Sentence s)
        {
            return (ICollection<Predicate>)s.accept(this, CollectionFactory.CreateQueue<Predicate>());
        }

        public object visitPredicate(Predicate p, object arg)
        {
            ICollection<Predicate> predicates = (ICollection<Predicate>)arg;
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

        public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
        {
            sentence.getQuantified().accept(this, arg);
            return arg;
        }
    }
}
