using System;
using System.Collections.Generic;
using tvn.cosine.ai.probability.domain;
using tvn.cosine.ai.probability.proposition;

namespace tvn.cosine.ai.probability.util
{
    /**
     * Default implementation of the RandomVariable interface.
     * 
     * Note: Also implements the TermProposition interface so its easy to use
     * RandomVariables in conjunction with propositions about them in the
     * Probability Model APIs.
     * 
     * @author Ciaran O'Reilly
     */
    public class RandVar<T> : RandomVariable, TermProposition<T>
    {
        private string name = null;
        private Domain domain = null;
        private ISet<RandomVariable> scope = new HashSet<RandomVariable>();

        public RandVar(string name, Domain domain)
        {
            ProbUtil.checkValidRandomVariableName(name);
            if (null == domain)
            {
                throw new ArgumentException("Domain of RandomVariable must be specified.");
            }

            this.name = name;
            this.domain = domain;
            this.scope.Add(this);
        }

        //
        // START-RandomVariable 
        public string getName()
        {
            return name;
        }

        public Domain getDomain()
        {
            return domain;
        }

        // END-RandomVariable
        //

        //
        // START-TermProposition 
        public RandomVariable getTermVariable()
        {
            return this;
        }

        public ISet<RandomVariable> getScope()
        {
            return scope;
        }

        public ISet<RandomVariable> getUnboundScope()
        {
            return scope;
        }

        public bool holds(IDictionary<RandomVariable, T> possibleWorld)
        {
            return possibleWorld.ContainsKey(getTermVariable());
        }

        // END-TermProposition
        //

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (!(o is RandomVariable))
            {
                return false;
            }

            // The name (not the name:domain combination) uniquely identifies a
            // Random Variable
            RandomVariable other = (RandomVariable)o;

            return this.name.Equals(other.getName());
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override string ToString()
        {
            return getName();
        }
    }

}
