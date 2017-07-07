using System.Collections.Generic;
using tvn.cosine.ai.probability.proposition;

namespace tvn.cosine.ai.probability.bayes
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 512.<br>
     * <br>
     * A Conditional Probability Table, or CPT, can be used for representing
     * conditional probabilities for discrete (finite) random variables. Each row in
     * a CPT contains the conditional probability of each node value for a
     * <b>conditioning case</b>.
     * 
     * @author Ciaran O'Reilly
     */
    public interface ConditionalProbabilityTable<T> : ConditionalProbabilityDistribution<T>
    {
        /**
         * Construct a Factor consisting of the Random Variables from the
         * Conditional Probability Table that are not part of the evidence (see
         * AIMA3e pg. 524).
         * 
         * @param evidence
         * @return a Factor for the Random Variables from the Conditional
         *         Probability Table that are not part of the evidence.
         */
        Factor<T> getFactorFor(params AssignmentProposition<T>[] evidence);
    }
}
