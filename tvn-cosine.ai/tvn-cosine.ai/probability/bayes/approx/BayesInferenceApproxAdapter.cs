using tvn.cosine.ai.probability.proposition;

namespace tvn.cosine.ai.probability.bayes.approx
{
    /**
     * An Adapter class to let BayesSampleInference implementations to be used in
     * places where calls are being made to the BayesInference API.
     * 
     * @author Ciaran O'Reilly
     */
    public class BayesInferenceApproxAdapter<T> : BayesInference<T>
    {
        private int N = 1000;
        private BayesSampleInference<T> adaptee = null;

        public BayesInferenceApproxAdapter(BayesSampleInference<T> adaptee)
        {
            this.adaptee = adaptee;
        }

        public BayesInferenceApproxAdapter(BayesSampleInference<T> adaptee, int N)
        {
            this.adaptee = adaptee;
            this.N = N;
        }

        /**
         * 
         * @return the number of Samples when calling the BayesSampleInference
         *         adaptee.
         */
        public int getN()
        {
            return N;
        }

        /**
         * 
         * @param n
         *            the numver of samples to be generated when calling the
         *            BayesSampleInference adaptee.
         */
        public void setN(int n)
        {
            N = n;
        }

        /**
         * 
         * @return The BayesSampleInference implementation to be adapted to the
         *         BayesInference API.
         */
        public BayesSampleInference<T> getAdaptee()
        {
            return adaptee;
        }

        /**
         * 
         * @param adaptee
         *            the BayesSampleInference implementation be be apated to the
         *            BayesInference API.
         */
        public void setAdaptee(BayesSampleInference<T> adaptee)
        {
            this.adaptee = adaptee;
        }

        //
        // START-BayesInference 
        public CategoricalDistribution<T> ask(RandomVariable[] X,
                  AssignmentProposition<T>[] observedEvidence,
                  BayesianNetwork<T> bn)
        {
            return adaptee.ask(X, observedEvidence, bn, N);
        }

        // END-BayesInference
        //
    } 
}
