using tvn.cosine.ai.probability.proposition;

namespace tvn.cosine.ai.probability.bayes.approx
{
    /**
     * General interface to be implemented by approximate Bayesian Inference
     * algorithms.
     * 
     * @author Ciaran O'Reilly
     */
    public interface BayesSampleInference<T>
    {
        /**
         * @param X
         *            the query variables.
         * @param observedEvidence
         *            observed values for variables E.
         * @param bn
         *            a Bayes net with variables {X} &cup; E &cup; Y /* Y = hidden
         *            variables
         * @param N
         *            the total number of samples to be generated
         * @return an estimate of <b>P</b>(X|e).
         */
        CategoricalDistribution<T> ask(RandomVariable[] X,
                 AssignmentProposition<T>[] observedEvidence,
                 BayesianNetwork<T> bn, int N);
    }
}
