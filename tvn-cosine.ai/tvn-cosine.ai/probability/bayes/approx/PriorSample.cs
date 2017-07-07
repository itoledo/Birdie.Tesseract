using System;
using System.Collections.Generic;
using tvn.cosine.ai.probability.util;

namespace tvn.cosine.ai.probability.bayes.approx
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
    public class PriorSample<T>
    {
        private Random randomizer = null;

        public PriorSample()
            : this(new Random())
        {  }

        public PriorSample(Random r)
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
        public IDictionary<RandomVariable, T> priorSample(BayesianNetwork<T> bn)
        {
            // x <- an event with n elements
            IDictionary<RandomVariable, T> x = new Dictionary<RandomVariable, T>();
            // foreach variable X<sub>i</sub> in X<sub>1</sub>,...,X<sub>n</sub> do
            foreach (RandomVariable Xi in bn.getVariablesInTopologicalOrder())
            {
                // x[i] <- a random sample from
                // <b>P</b>(X<sub>i</sub> | parents(X<sub>i</sub>))
                x.Add(Xi, ProbUtil.randomSample(bn.getNode(Xi), x, randomizer));
            }
            // return x
            return x;
        }
    } 
}
