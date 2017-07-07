using tvn.cosine.ai.probability.proposition;

namespace tvn.cosine.ai.probability.bayes
{
    /**
     * General interface to be implemented by Bayesian Inference algorithms.
     * 
     * @author Ciaran O'Reilly
     */
    public interface BayesInference<T>
    {
        /**
         * @param X
         *            the query variables.
         * @param observedEvidence
         *            observed values for variables E.
         * @param bn
         *            a Bayes net with variables {X} &cup; E &cup; Y /* Y = hidden
         *            variables
         * @return a distribution over the query variables.
         */
        CategoricalDistribution<T> ask(RandomVariable[] X, AssignmentProposition<T>[] observedEvidence, BayesianNetwork<T> bn);
    }
}
