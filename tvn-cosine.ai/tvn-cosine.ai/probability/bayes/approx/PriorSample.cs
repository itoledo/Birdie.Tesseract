﻿namespace tvn.cosine.ai.probability.bayes.approx
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 531.<br>
     * <br>
     * 
     * <pre>
     * function PRIOR-SAMPLE(bn) returns an event sampled from the prior specified by bn
     *   inputs: bn, a Bayesian network specifying joint distribution <b>P</b>(X<sub>1</sub>,...,X<sub>n</sub>)
     * 
     *   x <- an event with n elements
     *   foreach variable X<sub>i</sub> in X<sub>1</sub>,...,X<sub>n</sub> do
     *      x[i] <- a random sample from <b>P</b>(X<sub>i</sub> | parents(X<sub>i</sub>))
     *   return x
     * </pre>
     * 
     * Figure 14.13 A sampling algorithm that generates events from a Bayesian
     * network. Each variable is sampled according to the conditional distribution
     * given the values already sampled for the variable's parents.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class PriorSample
    {

        private IRandom randomizer = null;

        public PriorSample()
        {
            this(new JavaRandomizer(new Random()));
        }

        public PriorSample(Randomizer r)
        {
            this.randomizer = r;
        }

        // function PRIOR-SAMPLE(bn) returns an event sampled from the prior
        // specified by bn
        /**
         * The PRIOR-SAMPLE algorithm in Figure 14.13. A sampling algorithm that
         * generates events from a Bayesian network. Each variable is sampled
         * according to the conditional distribution given the values already
         * sampled for the variable's parents.
         * 
         * @param bn
         *            a Bayesian network specifying joint distribution
         *            <b>P</b>(X<sub>1</sub>,...,X<sub>n</sub>)
         * @return an event sampled from the prior specified by bn
         */
        public Map<RandomVariable, object> priorSample(BayesianNetwork bn)
        {
            // x <- an event with n elements
            Map<RandomVariable, object> x = Factory.CreateMap<RandomVariable, object>();
            // foreach variable X<sub>i</sub> in X<sub>1</sub>,...,X<sub>n</sub> do
            for (RandomVariable Xi : bn.getVariablesInTopologicalOrder())
            {
                // x[i] <- a random sample from
                // <b>P</b>(X<sub>i</sub> | parents(X<sub>i</sub>))
                x.Put(Xi, ProbUtil.randomSample(bn.getNode(Xi), x, randomizer));
            }
            // return x
            return x;
        }
    }

}
