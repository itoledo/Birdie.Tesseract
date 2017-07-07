using System;

namespace tvn.cosine.ai.probability.domain
{
    public abstract class AbstractContinuousDomain : ContinuousDomain
    {
        public bool isFinite()
        {
            return false;
        }

        public bool isInfinite()
        {
            return true;
        }

        public int size()
        {
            throw new Exception("You cannot determine the size of an infinite domain");
        }

        public abstract bool isOrdered();
        // END-Domain
        //
    }

}
