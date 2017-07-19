﻿using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.probability.proposition
{
    public abstract class AbstractTermProposition : AbstractProposition, TermProposition
    {
        private RandomVariable termVariable = null;

        public AbstractTermProposition(RandomVariable var)
        {
            if (null == var)
            {
                throw new IllegalArgumentException("The Random Variable for the Term must be specified.");
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
