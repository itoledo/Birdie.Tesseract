using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.probability.domain
{
    public abstract class AbstractContinuousDomain : ContinuousDomain
    {
        public virtual bool isFinite()
        {
            return false;
        }

        public virtual bool isInfinite()
        {
            return true;
        }

        public virtual int size()
        {
            throw new IllegalStateException("You cannot determine the size of an infinite domain");
        }

        public abstract bool isOrdered();
    }
}
