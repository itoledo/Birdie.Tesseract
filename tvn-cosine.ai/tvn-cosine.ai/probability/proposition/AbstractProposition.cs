using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.probability.proposition
{
    public abstract class AbstractProposition : Proposition
    {
        private ISet<RandomVariable> scope = Factory.CreateSet<RandomVariable>();
        private ISet<RandomVariable> unboundScope = Factory.CreateSet<RandomVariable>();

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

        protected virtual void addScope(IQueue<RandomVariable> vars)
        {
            scope.AddAll(vars);
        }

        protected virtual void addUnboundScope(RandomVariable var)
        {
            unboundScope.Add(var);
        }

        protected virtual void addUnboundScope(IQueue<RandomVariable> vars)
        {
            unboundScope.AddAll(vars);
        }
    } 
}
