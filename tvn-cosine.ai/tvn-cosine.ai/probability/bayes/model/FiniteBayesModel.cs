using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.probability.bayes.exact;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;

namespace tvn.cosine.ai.probability.bayes.model
{
    /**
     * Very simple implementation of the FiniteProbabilityModel API using a Bayesian
     * Network, consisting of FiniteNodes, to represent the underlying model.<br>
     * <br>
     * <b>Note:</b> The implementation currently doesn't take advantage of the use
     * of evidence values when calculating posterior values using the provided
     * Bayesian Inference implementation (e.g enumerationAsk). Instead it simply
     * creates a joint distribution over the scope of the propositions (i.e. using
     * the inference implementation with just query variables corresponding to the
     * scope of the propositions) and then iterates over this to get the appropriate
     * probability values. A smarter version, in the general case, would need to
     * dynamically create deterministic nodes to represent the outcome of logical
     * and derived propositions (e.g. conjuncts and summations over variables) in
     * order to be able to correctly calculate using evidence values.
     * 
     * @author Ciaran O'Reilly
     */
    public class FiniteBayesModel : FiniteProbabilityModel
    {
        private BayesianNetwork bayesNet = null;
        private ISet<RandomVariable> representation = CollectionFactory.CreateSet<RandomVariable>();
        private BayesInference bayesInference = null;

        public FiniteBayesModel(BayesianNetwork bn)
            : this(bn, new EnumerationAsk())
        { }

        public FiniteBayesModel(BayesianNetwork bn, BayesInference bi)
        {
            if (null == bn)
            {
                throw new IllegalArgumentException("Bayesian Network describing the model must be specified.");
            }
            this.bayesNet = bn;
            this.representation.AddAll(bn.getVariablesInTopologicalOrder());
            setBayesInference(bi);
        }

        public virtual BayesInference getBayesInference()
        {
            return bayesInference;
        }

        public virtual void setBayesInference(BayesInference bi)
        {
            this.bayesInference = bi;
        }

        //
        // START-ProbabilityModel
        public virtual bool isValid()
        {
            // Handle rounding 
            int counter = 0;
            Proposition[] propositionArray = new Proposition[representation.Size()];
            foreach (Proposition prop in representation)
            {
                propositionArray[counter] = prop;
                ++counter;
            }
            return System.Math.Abs(1 - prior(propositionArray)) <= ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD;
        }

        class CategoricalDistributionIteraorPrior : CategoricalDistributionIterator
        {
            private Proposition conjunct;
            private double[] probSum;

            public CategoricalDistributionIteraorPrior(Proposition conjunct, double[] probSum)
            {
                this.conjunct = conjunct;
                this.probSum = probSum;
            }

            public void iterate(IMap<RandomVariable, object> possibleWorld, double probability)
            {
                if (conjunct.holds(possibleWorld))
                {
                    probSum[0] += probability;
                }
            }
        }

        public virtual double prior(params Proposition[] phi)
        {
            // Calculating the prior, therefore no relevant evidence
            // just query over the scope of proposition phi in order
            // to get a joint distribution for these
            Proposition conjunct = ProbUtil.constructConjunction(phi);
            RandomVariable[] X = conjunct.getScope().ToArray();
            CategoricalDistribution d = bayesInference.ask(X, new AssignmentProposition[0], bayesNet);

            // Then calculate the probability of the propositions phi
            // be seeing where they hold.
            double[] probSum = new double[1];
            CategoricalDistributionIterator di = new CategoricalDistributionIteraorPrior(conjunct, probSum);
            d.iterateOver(di);

            return probSum[0];
        }

        public virtual double posterior(Proposition phi, params Proposition[] evidence)
        {

            Proposition conjEvidence = ProbUtil.constructConjunction(evidence);

            // P(A | B) = P(A AND B)/P(B) - (13.3 AIMA3e)
            Proposition aAndB = new ConjunctiveProposition(phi, conjEvidence);
            double probabilityOfEvidence = prior(conjEvidence);
            if (0 != probabilityOfEvidence)
            {
                return prior(aAndB) / probabilityOfEvidence;
            }

            return 0;
        }

        public virtual ISet<RandomVariable> getRepresentation()
        {
            return representation;
        }

        public virtual CategoricalDistribution priorDistribution(params Proposition[] phi)
        {
            return jointDistribution(phi);
        }

        public virtual CategoricalDistribution posteriorDistribution(Proposition phi, params Proposition[] evidence)
        {

            Proposition conjEvidence = ProbUtil.constructConjunction(evidence);

            // P(A | B) = P(A AND B)/P(B) - (13.3 AIMA3e)
            CategoricalDistribution dAandB = jointDistribution(phi, conjEvidence);
            CategoricalDistribution dEvidence = jointDistribution(conjEvidence);

            CategoricalDistribution rVal = dAandB.divideBy(dEvidence);
            // Note: Need to ensure normalize() is called
            // in order to handle the case where an approximate
            // algorithm is used (i.e. won't evenly divide
            // as will have calculated on separate approximate
            // runs). However, this should only be done
            // if the all of the evidences scope are bound (if not
            // you are returning in essence a set of conditional
            // distributions, which you do not want normalized).
            bool unboundEvidence = false;
            foreach (Proposition e in evidence)
            {
                if (e.getUnboundScope().Size() > 0)
                {
                    unboundEvidence = true;
                    break;
                }
            }
            if (!unboundEvidence)
            {
                rVal.normalize();
            }

            return rVal;
        }

        class CategoricalDistributionIteratorJointDistribution : CategoricalDistributionIterator
        {
            private Proposition conjProp;
            private ProbabilityTable ud;
            private object[] values;
            private ISet<RandomVariable> vars;

            public CategoricalDistributionIteratorJointDistribution(Proposition conjProp, ISet<RandomVariable> vars, ProbabilityTable ud, object[] values)
            {
                this.conjProp = conjProp;
                this.vars = vars;
                this.ud = ud;
                this.values = values;
            }

            public void iterate(IMap<RandomVariable, object> possibleWorld, double probability)
            {
                if (conjProp.holds(possibleWorld))
                {
                    int i = 0;
                    foreach (RandomVariable rv in vars)
                    {
                        values[i] = possibleWorld.Get(rv);
                       ++i;
                    }
                    int dIdx = ud.getIndex(values);
                    ud.setValue(dIdx, ud.getValues()[dIdx] + probability);
                }
            }
        }

        public virtual CategoricalDistribution jointDistribution(params Proposition[] propositions)
        {
            ProbabilityTable d = null;
            Proposition conjProp = ProbUtil.constructConjunction(propositions);
            ISet<RandomVariable> vars = CollectionFactory.CreateSet<RandomVariable>(conjProp.getUnboundScope());

            if (vars.Size() > 0)
            {
                RandomVariable[] distVars = new RandomVariable[vars.Size()];
                int i = 0;
                foreach (RandomVariable rv in vars)
                {
                    distVars[i] = rv;
                   ++i;
                }

                ProbabilityTable ud = new ProbabilityTable(distVars);
                object[] values = new object[vars.Size()];

                CategoricalDistributionIterator di = new CategoricalDistributionIteratorJointDistribution(conjProp, vars, ud, values);

                RandomVariable[] X = conjProp.getScope().ToArray();
                bayesInference.ask(X, new AssignmentProposition[0], bayesNet).iterateOver(di);

                d = ud;
            }
            else
            {
                // No Unbound Variables, therefore just return
                // the singular probability related to the proposition.
                d = new ProbabilityTable();
                d.setValue(0, prior(propositions));
            }
            return d;
        }
    }
}
