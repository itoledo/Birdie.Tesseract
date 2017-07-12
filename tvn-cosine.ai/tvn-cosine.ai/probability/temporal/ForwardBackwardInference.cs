using System.Collections.Generic;
using tvn.cosine.ai.probability.proposition;

namespace tvn.cosine.ai.probability.temporal
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 576. 
     *  
     * 
     * Generic interface for calling different implementations of the
     * forward-backward algorithm for smoothing: computing posterior probabilities
     * of a sequence of states given a sequence of observations.
     * 
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public interface ForwardBackwardInference<T> : ForwardStepInference<T>, BackwardStepInference<T>
    {

        /**
         * The forward-backward algorithm for smoothing: computing posterior
         * probabilities of a sequence of states given a sequence of observations.
         * 
         * @param ev
         *            a vector of evidence values for steps 1,...,t
         * @param prior
         *            the prior distribution on the initial state,
         *            <b>P</b>(X<sub>0</sub>)
         * @return a vector of smoothed estimates for steps 1,...,t
         */
        IList<CategoricalDistribution<T>> forwardBackward(IList<IList<AssignmentProposition<T>>> ev, CategoricalDistribution<T> prior);
    } 
}
