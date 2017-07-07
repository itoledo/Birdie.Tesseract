using System;

namespace tvn.cosine.ai.probability.proposition
{
    public abstract class AbstractTermProposition<T> : AbstractProposition<T>, TermProposition<T>
    {
        private RandomVariable termVariable = null;

        public AbstractTermProposition(RandomVariable var)
        {
            if (null == var)
            {
                throw new ArgumentException("The Random Variable for the Term must be specified.");
            }
            this.termVariable = var;
            addScope(this.termVariable);
        }

        public RandomVariable getTermVariable()
        {
            return termVariable;
        }
    } 
}
