using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.probability.bayes.approx
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 534.<br>
     * <br>
     * 
     * <pre>
     * function LIKELIHOOD-WEIGHTING(X, e, bn, N) returns an estimate of <b>P</b>(X|e)
     *   inputs: X, the query variable
     *           e, observed values for variables E
     *           bn, a Bayesian network specifying joint distribution <b>P</b>(X<sub>1</sub>,...,X<sub>n</sub>)
     *           N, the total number of samples to be generated
     *   local variables: W, a vector of weighted counts for each value of X, initially zero
     *   
     *   for j = 1 to N do
     *       <b>x</b>,w <- WEIGHTED-SAMPLE(bn,e)
     *       W[x] <- W[x] + w where x is the value of X in <b>x</b>
     *   return NORMALIZE(W)
     * --------------------------------------------------------------------------------------
     * function WEIGHTED-SAMPLE(bn, e) returns an event and a weight
     *   
     *    w <- 1; <b>x</b> <- an event with n elements initialized from e
     *    foreach variable X<sub>i</sub> in X<sub>1</sub>,...,X<sub>n</sub> do
     *        if X<sub>i</sub> is an evidence variable with value x<sub>i</sub> in e
     *            then w <- w * P(X<sub>i</sub> = x<sub>i</sub> | parents(X<sub>i</sub>))
     *            else <b>x</b>[i] <- a random sample from <b>P</b>(X<sub>i</sub> | parents(X<sub>i</sub>))
     *    return <b>x</b>, w
     * </pre>
     * 
     * Figure 14.15 The likelihood-weighting algorithm for inference in Bayesian
     * networks. In WEIGHTED-SAMPLE, each nonevidence variable is sampled according
     * to the conditional distribution given the values already sampled for the
     * variable's parents, while a weight is accumulated based on the likelihood for
     * each evidence variable.<br>
     * <br>
     * <b>Note:</b> The implementation has been extended to handle queries with
     * multiple variables. <br>
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class LikelihoodWeighting<T> : BayesSampleInference<T>
    {
        private Random randomizer = null;

        public LikelihoodWeighting()
            : this(new Random())
        { }

        public LikelihoodWeighting(Random r)
        {
            this.randomizer = r;
        }

        // function LIKELIHOOD-WEIGHTING(X, e, bn, N) returns an estimate of
        // <b>P</b>(X|e)
        /**
         * The LIKELIHOOD-WEIGHTING algorithm in Figure 14.15. For answering queries
         * given evidence in a Bayesian Network.
         * 
         * @param X
         *            the query variables
         * @param e
         *            observed values for variables E
         * @param bn
         *            a Bayesian network specifying joint distribution
         *            <b>P</b>(X<sub>1</sub>,...,X<sub>n</sub>)
         * @param N
         *            the total number of samples to be generated
         * @return an estimate of <b>P</b>(X|e)
         */
        public CategoricalDistribution<T> likelihoodWeighting(RandomVariable[] X,
                AssignmentProposition<T>[] e, BayesianNetwork<T> bn, int N)
        {
            // local variables: W, a vector of weighted counts for each value of X,
            // initially zero
            double[] W = new double[ProbUtil.expectedSizeOfCategoricalDistribution<T>(X)];

            // for j = 1 to N do
            for (int j = 0; j < N; j++)
            {
                // <b>x</b>,w <- WEIGHTED-SAMPLE(bn,e)
                Pair<IDictionary<RandomVariable, T>, double> x_w = weightedSample(bn, e);
                // W[x] <- W[x] + w where x is the value of X in <b>x</b>
                W[ProbUtil.indexOf(X, x_w.First)] += x_w.Second;
            }
            // return NORMALIZE(W)
            return new ProbabilityTable<T>(W, X).normalize();
        }

        // function WEIGHTED-SAMPLE(bn, e) returns an event and a weight
        /**
         * The WEIGHTED-SAMPLE function in Figure 14.15.
         * 
         * @param e
         *            observed values for variables E
         * @param bn
         *            a Bayesian network specifying joint distribution
         *            <b>P</b>(X<sub>1</sub>,...,X<sub>n</sub>)
         * @return return <b>x</b>, w - an event with its associated weight.
         */
        public Pair<IDictionary<RandomVariable, T>, double> weightedSample(BayesianNetwork<T> bn, AssignmentProposition<T>[] e)
        {
            // w <- 1;
            double w = 1.0;
            // <b>x</b> <- an event with n elements initialized from e
            IDictionary<RandomVariable, T> x = new Dictionary<RandomVariable, T>();
            foreach (AssignmentProposition<T> ap in e)
            {
                x.Add(ap.getTermVariable(), ap.getValue());
            }

            // foreach variable X<sub>i</sub> in X<sub>1</sub>,...,X<sub>n</sub> do
            foreach (RandomVariable Xi in bn.getVariablesInTopologicalOrder())
            {
                // if X<sub>i</sub> is an evidence variable with value x<sub>i</sub>
                // in e
                if (x.ContainsKey(Xi))
                {
                    // then w <- w * P(X<sub>i</sub> = x<sub>i</sub> |
                    // parents(X<sub>i</sub>))
                    w *= bn.getNode(Xi)
                            .getCPD()
                            .getValue(
                                    ProbUtil.getEventValuesForXiGivenParents(
                                            bn.getNode(Xi), x));
                }
                else
                {
                    // else <b>x</b>[i] <- a random sample from
                    // <b>P</b>(X<sub>i</sub> | parents(X<sub>i</sub>))
                    x.Add(Xi, ProbUtil.randomSample(bn.getNode(Xi), x, randomizer));
                }
            }
            // return <b>x</b>, w
            return new Pair<IDictionary<RandomVariable, T>, double>(x, w);
        }

        //
        // START-BayesSampleInference 
        public CategoricalDistribution<T> ask(RandomVariable[] X,
               AssignmentProposition<T>[] observedEvidence,
               BayesianNetwork<T> bn, int N)
        {
            return likelihoodWeighting(X, observedEvidence, bn, N);
        }

        // END-BayesSampleInference
        //
    }
}
