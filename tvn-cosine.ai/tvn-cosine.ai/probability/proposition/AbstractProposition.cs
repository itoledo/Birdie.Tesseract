using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.probability.proposition
{
    public abstract class AbstractProposition : Proposition
    {
        private ISet<RandomVariable> scope = CollectionFactory.CreateSet<RandomVariable>();
        private ISet<RandomVariable> unboundScope = CollectionFactory.CreateSet<RandomVariable>();

        public AbstractProposition()
        { }

        public virtual ISet<RandomVariable> getScope()
        {
            return scope;
        }

        public virtual ISet<RandomVariable> getUnboundScope()
        {
            return unboundScope;
        }

        public abstract bool holds(IMap<RandomVariable, object> possibleWorld);

        protected virtual void addScope(RandomVariable var)
        {
            scope.Add(var);
        }

        protected virtual void addScope(ICollection<RandomVariable> vars)
        {
            scope.AddAll(vars);
        }

        protected virtual void addUnboundScope(RandomVariable var)
        {
            unboundScope.Add(var);
        }

        protected virtual void addUnboundScope(ICollection<RandomVariable> vars)
        {
            unboundScope.AddAll(vars);
        }
    } 
}
