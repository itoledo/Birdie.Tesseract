﻿using System.Collections.Generic;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;

namespace tvn.cosine.ai.probability.bayes.approx
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 533.<br>
     * <br>
     * 
     * <pre>
     * function REJECTION-SAMPLING(X, e, bn, N) returns an estimate of <b>P</b>(X|e)
     *   inputs: X, the query variable
     *           e, observed values for variables E
     *           bn, a Bayesian network
     *           N, the total number of samples to be generated
     *   local variables: <b>N</b>, a vector of counts for each value of X, initially zero
     *   
     *   for j = 1 to N do
     *       <b>x</b> <- PRIOR-SAMPLE(bn)
     *       if <b>x</b> is consistent with e then
     *          <b>N</b>[x] <- <b>N</b>[x] + 1 where x is the value of X in <b>x</b>
     *   return NORMALIZE(<b>N</b>)
     * </pre>
     * 
     * Figure 14.14 The rejection-sampling algorithm for answering queries given
     * evidence in a Bayesian Network.<br>
     * <br>
     * <b>Note:</b> The implementation has been extended to handle queries with
     * multiple variables. <br>
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class RejectionSampling<T> : BayesSampleInference<T>
    {
        private PriorSample<T> ps = null;

        public RejectionSampling()
            : this(new PriorSample<T>())
        { }

        public RejectionSampling(PriorSample<T> ps)
        {
            this.ps = ps;
        }

        // function REJECTION-SAMPLING(X, e, bn, N) returns an estimate of
        // <b>P</b>(X|e)
        /**
         * The REJECTION-SAMPLING algorithm in Figure 14.14. For answering queries
         * given evidence in a Bayesian Network.
         * 
         * @param X
         *            the query variables
         * @param e
         *            observed values for variables E
         * @param bn
         *            a Bayesian network
         * @param Nsamples
         *            the total number of samples to be generated
         * @return an estimate of <b>P</b>(X|e)
         */
        public CategoricalDistribution<T> rejectionSampling(RandomVariable[] X, AssignmentProposition<T>[] e, BayesianNetwork<T> bn, int Nsamples)
        {
            // local variables: <b>N</b>, a vector of counts for each value of X,
            // initially zero
            double[] N = new double[ProbUtil.expectedSizeOfCategoricalDistribution<T>(X)];

            // for j = 1 to N do
            for (int j = 0; j < Nsamples; j++)
            {
                // <b>x</b> <- PRIOR-SAMPLE(bn)
                IDictionary<RandomVariable, T> x = ps.priorSample(bn);
                // if <b>x</b> is consistent with e then
                if (isConsistent(x, e))
                {
                    // <b>N</b>[x] <- <b>N</b>[x] + 1
                    // where x is the value of X in <b>x</b>
                    N[ProbUtil.indexOf(X, x)] += 1.0;
                }
            }
            // return NORMALIZE(<b>N</b>)
            return new ProbabilityTable<T>(N, X).normalize();
        }

        //
        // START-BayesSampleInference 
        public CategoricalDistribution<T> ask(RandomVariable[] X,
               AssignmentProposition<T>[] observedEvidence,
               BayesianNetwork<T> bn, int N)
        {
            return rejectionSampling(X, observedEvidence, bn, N);
        }

        // END-BayesSampleInference
        //

        //
        // PRIVATE METHODS
        //
        private bool isConsistent(IDictionary<RandomVariable, T> x, AssignmentProposition<T>[] e)
        {
            foreach (AssignmentProposition<T> ap in e)
            {
                if (!ap.getValue().Equals(x[ap.getTermVariable()]))
                {
                    return false;
                }
            }
            return true;
        }
    }

}
