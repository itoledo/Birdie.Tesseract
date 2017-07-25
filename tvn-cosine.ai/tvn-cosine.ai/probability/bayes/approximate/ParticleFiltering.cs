﻿using tvn.cosine.ai.common;
using tvn.cosine.ai.common.api;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.probability.api;
using tvn.cosine.ai.probability.bayes.api;
using tvn.cosine.ai.probability.bayes.exact;
using tvn.cosine.ai.probability.bayes.model;
using tvn.cosine.ai.probability.domain;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.probability.bayes.approximate
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 598.<br>
     * <br>
     * 
     * <pre>
     * function PARTICLE-FILTERING(<b>e</b>, N, dbn) returns a set of samples for the next time step
     *   inputs: <b>e</b>, the new incoming evidence
     *           N, the number of samples to be maintained
     *           dbn, a DBN with prior <b>P</b>(<b>X</b><sub>0</sub>), transition model <b>P</b>(<b>X</b><sub>1</sub> | <b>X</b><sub>0</sub>), sensor model <b>P</b>(<b>E</b><sub>1</sub> | <b>X</b><sub>1</sub>)
     *   persistent: S, a vector of samples of size N, initially generated from <b>P</b>(<b>X</b><sub>0</sub>)
     *   local variables: W, a vector of weights of size N
     *   
     *   for i = 1 to N do
     *       S[i] <- sample from <b>P</b>(<b>X</b><sub>1</sub> | <b>X</b><sub>0</sub> = S[i]) /* step 1 
     *       W[i] <- <b>P</b>(<b>e</b> | <b>X</b><sub>1</sub> = S[i]) /* step 2
     *   S <- WEIGHTED-SAMPLE-WITH-REPLACEMENT(N, S, W) /* step 3
     *   return S
     * </pre>
     * 
     * Figure 15.17 The particle filtering algorithm implemented as a recursive
     * update operation with state (the set of samples). Each of the sampling
     * operations involves sampling the relevant slice variables in topological
     * order, much as in PRIOR-SAMPLE. The WEIGHTED-SAMPLE-WITH-REPLACEMENT
     * operation can be implemented to run in O(N) expected time. The step numbers
     * refer to the description in the text.
     * 
     * <ol>
     * <li>Each sample is propagated forward by sampling the next state value
     * <b>x</b><sub>t+1</sub> given the current value <b>x</b><sub>t</sub> for the
     * sample, based on the transition model <b>P</b>(<b>X</b><sub>t+1</sub> |
     * <b>x</b><sub>t</sub>).</li>
     * <li>Each sample is weighted by the likelihood it assigns to the new evidence,
     * P(<b>e</b><sub>t+1</sub> | <b>x</b><sub>t+1</sub>).</li>
     * <li>The population is resampled to generate a new population of N samples.
     * Each new sample is selected from the current population; the probability that
     * a particular sample is selected is proportional to its weight. The new
     * samples are unweighted.</li>
     * </ol>
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * 
     */
    public class ParticleFiltering
    {
        private int N = 0;
        private IDynamicBayesianNetwork dbn = null;
        private AssignmentProposition[][] S;
        //
        private IRandom randomizer = null;
        private PriorSample priorSampler = null;
        private AssignmentProposition[][] S_tp1;
        private IFiniteProbabilityModel sensorModel = null;
        private IRandomVariable sampleIndexes = null;

        /**
         * Construct a Particle Filtering instance.
         * 
         * @param N
         *            the number of samples to be maintained
         * @param dbn
         *            a DBN with prior <b>P</b>(<b>X</b><sub>0</sub>), transition
         *            model <b>P</b>(<b>X</b><sub>1</sub> | <b>X</b><sub>0</sub>),
         *            sensor model <b>P</b>(<b>E</b><sub>1</sub> |
         *            <b>X</b><sub>1</sub>)
         */
        public ParticleFiltering(int N, IDynamicBayesianNetwork dbn)
            : this(N, dbn, CommonFactory.CreateRandom())
        { }

        /**
         * Construct a Particle Filtering instance.
         * 
         * @param N
         *            the number of samples to be maintained
         * @param dbn
         *            a DBN with prior <b>P</b>(<b>X</b><sub>0</sub>), transition
         *            model <b>P</b>(<b>X</b><sub>1</sub> | <b>X</b><sub>0</sub>),
         *            sensor model <b>P</b>(<b>E</b><sub>1</sub> |
         *            <b>X</b><sub>1</sub>)
         * @param randomizer
         *            a IRandom to be used for sampling purposes.
         */
        public ParticleFiltering(int N, IDynamicBayesianNetwork dbn, IRandom randomizer)
        {
            this.randomizer = randomizer;
            this.priorSampler = new PriorSample(this.randomizer);
            initPersistent(N, dbn);
        }

        /**
         * The particle filtering algorithm implemented as a recursive update
         * operation with state (the set of samples).
         * 
         * @param e
         *            <b>e</b>, the new incoming evidence
         * @return a vector of samples of size N, where each sample is a vector of
         *         assignment propositions for the X_1 state variables, which is
         *         intended to represent the generated sample for time t.
         */
        public AssignmentProposition[][] particleFiltering(AssignmentProposition[] e)
        {
            // local variables: W, a vector of weights of size N
            double[] W = new double[N];

            // for i = 1 to N do
            for (int i = 0; i < N;++i)
            {
                /* step 1 */
                // S[i] <- sample from <b>P</b>(<b>X</b><sub>1</sub> |
                // <b>X</b><sub>0</sub> = S[i])
                sampleFromTransitionModel(i);
                /* step 2 */
                // W[i] <- <b>P</b>(<b>e</b> | <b>X</b><sub>1</sub> = S[i])
                W[i] = sensorModel.posterior(ProbUtil.constructConjunction(e), S_tp1[i]);
            }
            /* step 3 */
            // S <- WEIGHTED-SAMPLE-WITH-REPLACEMENT(N, S, W)
            S = weightedSampleWithReplacement(N, S, W);

            // return S
            return S;
        }

        /**
         * Reset this instances persistent variables to be used between called to
         * particleFiltering().
         * 
         * @param N
         *            the number of samples to be maintained
         * @param dbn
         *            a DBN with prior <b>P</b>(<b>X</b><sub>0</sub>), transition
         *            model <b>P</b>(<b>X</b><sub>1</sub> | <b>X</b><sub>0</sub>),
         *            sensor model <b>P</b>(<b>E</b><sub>1</sub> |
         *            <b>X</b><sub>1</sub>)
         */
        public void initPersistent(int N, IDynamicBayesianNetwork dbn)
        {
            this.N = N;
            this.dbn = dbn;
            // persistent: S, a vector of samples of size N, initially generated
            // from <b>P</b>(<b>X</b><sub>0</sub>)
            S = new AssignmentProposition[N][];
            S_tp1 = new AssignmentProposition[N][];
            int[] indexes = new int[N];
            for (int i = 0; i < N;++i)
            {
                S[i] = new AssignmentProposition[this.dbn.GetX_0().Size()];
                S_tp1[i] = new AssignmentProposition[this.dbn.GetX_0().Size()];
                indexes[i] = i;
                IMap<IRandomVariable, object> sample = priorSampler.priorSample(this.dbn.GetPriorNetwork());
                int idx = 0;
                foreach (var sa in sample)
                {
                    S[i][idx] = new AssignmentProposition(this.dbn.GetX_0_to_X_1().Get(sa.GetKey()), sa.GetValue());
                    S_tp1[i][idx] = new AssignmentProposition(this.dbn.GetX_0_to_X_1().Get(sa.GetKey()), sa.GetValue());
                    idx++;
                }
            }

            sensorModel = new FiniteBayesModel(dbn, new EliminationAsk()); 
            sampleIndexes = new RandVar("SAMPLE_INDEXES", new FiniteIntegerDomain(indexes));
        }

        private void sampleFromTransitionModel(int i)
        {
            // x <- an event initialized with S[i]
            IMap<IRandomVariable, object> x = CollectionFactory.CreateInsertionOrderedMap<IRandomVariable, object>();
            for (int n = 0; n < S[i].Length; n++)
            {
                AssignmentProposition x1 = S[i][n];
                x.Put(this.dbn.GetX_1_to_X_0().Get(x1.getTermVariable()), x1.getValue());
            }

            // foreach variable X<sub>1<sub>i</sub></sub> in
            // X<sub>1<sub>1</sub></sub>,...,X<sub>1<sub>n<</sub>/sub> do
            foreach (IRandomVariable X1_i in dbn.GetX_1_VariablesInTopologicalOrder())
            {
                // x1[i] <- a random sample from
                // <b>P</b>(X<sub>1<sub>i</sub></sub> |
                // parents(X<sub>1<sub>i</sub></sub>))
                x.Put(X1_i, ProbUtil.randomSample(dbn.GetNode(X1_i), x, randomizer));
            }

            // S[i] <- sample from <b>P</b>(<b>X</b><sub>1</sub> |
            // <b>X</b><sub>0</sub> = S[i])
            for (int n = 0; n < S_tp1[i].Length; n++)
            {
                AssignmentProposition x1 = S_tp1[i][n];
                x1.setValue(x.Get(x1.getTermVariable()));
            }
        }

        /**
         * The population is re-sampled to generate a new population of N samples.
         * Each new sample is selected from the current population; the probability
         * that a particular sample is selected is proportional to its weight. The
         * new samples are un-weighted.
         * 
         * @param N
         *            the number of samples
         * @param S
         *            a vector of samples of size N, where each sample is a vector
         *            of assignment propositions for the X_1 state variables, which
         *            is intended to represent the sample for time t
         * @param W
         *            a vector of weights of size N
         * 
         * @return a new vector of samples of size N sampled from S based on W
         */
        private AssignmentProposition[][] weightedSampleWithReplacement(int N,
                AssignmentProposition[][] S, double[] W)
        {
            AssignmentProposition[][] newS = new AssignmentProposition[N][];

            double[] normalizedW = Util.normalize(W);

            for (int i = 0; i < N;++i)
            {
                newS[i] = new AssignmentProposition[this.dbn.GetX_0().Size()];
                int sample = (int)ProbUtil.sample(randomizer.NextDouble(), sampleIndexes, normalizedW);
                for (int idx = 0; idx < S_tp1[i].Length; idx++)
                {
                    AssignmentProposition ap = S_tp1[sample][idx];
                    newS[i][idx] = new AssignmentProposition(ap.getTermVariable(), ap.getValue());
                }
            }

            return newS;
        }
    } 
}
