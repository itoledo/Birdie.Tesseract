using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.probability.api;

namespace tvn.cosine.ai.probability.proposition
{
    public abstract class AbstractTermProposition : AbstractProposition, TermProposition
    {
        private IRandomVariable termVariable = null;

        public AbstractTermProposition(IRandomVariable var)
        {
            if (null == var)
            {
                throw new IllegalArgumentException("The Random Variable for the Term must be specified.");
            }
            this.termVariable = var;
            addScope(this.termVariable);
        }

        public IRandomVariable getTermVariable()
        {
            return termVariable;
        }
    }
}
