using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.probability;
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
    public class FiniteBayesModel<T> : FiniteProbabilityModel<T>
    {


        private BayesianNetwork<T> bayesNet = null;
        private ISet<RandomVariable> representation = new HashSet<RandomVariable>();
        private BayesInference<T> bayesInference = null;

        public FiniteBayesModel(BayesianNetwork<T> bn)
            : this(bn, new EnumerationAsk<T>())
        { }

        public FiniteBayesModel(BayesianNetwork<T> bn, BayesInference<T> bi)
        {
            if (null == bn)
            {
                throw new ArgumentException("Bayesian Network describing the model must be specified.");
            }
            this.bayesNet = bn;
            foreach (var v in bn.getVariablesInTopologicalOrder())
                this.representation.Add(v);
            setBayesInference(bi);
        }

        public BayesInference<T> getBayesInference()
        {
            return bayesInference;
        }

        public void setBayesInference(BayesInference<T> bi)
        {
            this.bayesInference = bi;
        }

        //
        // START-ProbabilityModel
        public override bool isValid()
        {
            // Handle rounding
            return Math.Abs(1 - prior(new Proposition<T>[representation.Count])) <= ProbabilityModel<T>.DEFAULT_ROUNDING_THRESHOLD;
        }

        public override double prior(IEnumerable<Proposition<T>> phi)
        {
            // Calculating the prior, therefore no relevant evidence
            // just query over the scope of proposition phi in order
            // to get a joint distribution for these
            Proposition<T> conjunct = ProbUtil.constructConjunction<T>(phi.ToArray());
            RandomVariable[] X = conjunct.getScope().ToArray();
            CategoricalDistribution<T> d = bayesInference.ask(X, new AssignmentProposition<T>[0], bayesNet);

            // Then calculate the probability of the propositions phi
            // be seeing where they hold.
            double[] probSum = new double[1];
            Iterator<T> di = new Iterator<T>((possibleWorld, probability) =>
            {
                if (conjunct.holds(possibleWorld))
                {
                    probSum[0] += probability;
                }
            });
            d.iterateOver(di);

            return probSum[0];
        }

        public override double posterior(Proposition<T> phi, IEnumerable<Proposition<T>> evidence)
        {

            Proposition<T> conjEvidence = ProbUtil.constructConjunction<T>(evidence.ToArray());

            // P(A | B) = P(A AND B)/P(B) - (13.3 AIMA3e)
            Proposition<T> aAndB = new ConjunctiveProposition<T>(phi, conjEvidence);
            double probabilityOfEvidence = prior(new[] { conjEvidence });
            if (0 != probabilityOfEvidence)
            {
                return prior(new[] { aAndB }) / probabilityOfEvidence;
            }

            return 0;
        }

        public override ISet<RandomVariable> getRepresentation()
        {
            return representation;
        }

        // END-ProbabilityModel
        //

        //
        // START-FiniteProbabilityModel
        public override CategoricalDistribution<T> priorDistribution(IEnumerable<Proposition<T>> phi)
        {
            return jointDistribution(phi);
        }

        public override CategoricalDistribution<T> posteriorDistribution(Proposition<T> phi, IEnumerable<Proposition<T>> evidence)
        {

            Proposition<T> conjEvidence = ProbUtil.constructConjunction<T>(evidence.ToArray());

            // P(A | B) = P(A AND B)/P(B) - (13.3 AIMA3e)
            CategoricalDistribution<T> dAandB = jointDistribution(new[] { phi, conjEvidence });
            CategoricalDistribution<T> dEvidence = jointDistribution(new[] { conjEvidence });

            CategoricalDistribution<T> rVal = dAandB.divideBy(dEvidence);
            // Note: Need to ensure normalize() is called
            // in order to handle the case where an approximate
            // algorithm is used (i.e. won't evenly divide
            // as will have calculated on separate approximate
            // runs). However, this should only be done
            // if the all of the evidences scope are bound (if not
            // you are returning in essence a set of conditional
            // distributions, which you do not want normalized).
            bool unboundEvidence = false;
            foreach (Proposition<T> e in evidence)
            {
                if (e.getUnboundScope().Count > 0)
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

        public override CategoricalDistribution<T> jointDistribution(IEnumerable<Proposition<T>> propositions)
        {
            ProbabilityTable<T> d = null;
            Proposition<T> conjProp = ProbUtil.constructConjunction<T>(propositions.ToArray());
            ISet<RandomVariable> vars = new HashSet<RandomVariable>(conjProp.getUnboundScope());

            if (vars.Count > 0)
            {
                RandomVariable[] distVars = new RandomVariable[vars.Count];
                int i = 0;
                foreach (RandomVariable rv in vars)
                {
                    distVars[i] = rv;
                    i++;
                }

                ProbabilityTable<T> ud = new ProbabilityTable<T>(distVars);
                T[] values = new T[vars.Count];

                Iterator<T> di = new Iterator<T>((possibleWorld, probability) =>
               {
                   if (conjProp.holds(possibleWorld))
                   {
                       int b = 0;
                       foreach (RandomVariable rv in vars)
                       {
                           values[i] = possibleWorld[rv];
                           b++;
                       }
                       int dIdx = ud.getIndex(values);
                       ud.setValue(dIdx, ud.getValues()[dIdx] + probability);
                   }
               });

                RandomVariable[] X = conjProp.getScope().ToArray();
                bayesInference.ask(X, new AssignmentProposition<T>[0], bayesNet).iterateOver(di);

                d = ud;
            }
            else
            {
                // No Unbound Variables, therefore just return
                // the singular probability related to the proposition.
                d = new ProbabilityTable<T>(new RandomVariable[] { });
                d.setValue(0, prior(propositions));
            }
            return d;
        }

        // END-FiniteProbabilityModel
        //
    }

}
