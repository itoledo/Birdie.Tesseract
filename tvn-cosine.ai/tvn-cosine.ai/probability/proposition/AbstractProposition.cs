using System.Collections.Generic;

namespace tvn.cosine.ai.probability.proposition
{
    public abstract class AbstractProposition<T> : Proposition<T>
    {
        private ISet<RandomVariable> scope = new HashSet<RandomVariable>();
        private ISet<RandomVariable> unboundScope = new HashSet<RandomVariable>();

        public AbstractProposition()
        {

        }

        //
        // START-Proposition
        public ISet<RandomVariable> getScope()
        {
            return scope;
        }

        public ISet<RandomVariable> getUnboundScope()
        {
            return unboundScope;
        }

        public abstract bool holds(IDictionary<RandomVariable, T> possibleWorld);

        // END-Proposition
        //

        //
        // Protected Methods
        //
        protected void addScope(RandomVariable var)
        {
            scope.Add(var);
        }

        protected void addScope(ICollection<RandomVariable> vars)
        {
            foreach (var v in vars)
                scope.Add(v);
        }

        protected void addUnboundScope(RandomVariable var)
        {
            unboundScope.Add(var);
        }

        protected void addUnboundScope(ICollection<RandomVariable> vars)
        {
            foreach (var v in vars)
                unboundScope.Add(v);
        }
    } 
}
