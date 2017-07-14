using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;

namespace tvn.cosine.ai.probability.full
{
    /**
     * An implementation of the FiniteProbabilityModel API using a full joint
     * distribution as the underlying model.
     * 
     * @author Ciaran O'Reilly
     */
    public class FullJointDistributionModel<T> : FiniteProbabilityModel<T>
    {
        private ProbabilityTable<T> distribution = null;
        private ISet<RandomVariable> representation = null;

        public FullJointDistributionModel(double[] values, params RandomVariable[] vars)
        {
            if (null == vars)
            {
                throw new ArgumentException("Random Variables describing the model's representation of the World need to be specified.");
            }

            distribution = new ProbabilityTable<T>(values, vars);

            representation = new HashSet<RandomVariable>();
            for (int i = 0; i < vars.Length; ++i)
            {
                representation.Add(vars[i]);
            }

            //TODO: make readonly
            //representation = Collections.unmodifiableSet(representation);
        }

        //
        // START-ProbabilityModel
        public override bool isValid()
        {
            // Handle rounding
            return Math.Abs(1 - distribution.getSum()) <= DEFAULT_ROUNDING_THRESHOLD;
        }

        public override double prior(params Proposition<T>[] phi)
        {
            return probabilityOf(ProbUtil.constructConjunction(phi));
        }

        public override double posterior(Proposition<T> phi, params Proposition<T>[] evidence)
        {

            Proposition<T> conjEvidence = ProbUtil.constructConjunction(evidence);

            // P(A | B) = P(A AND B)/P(B) - (13.3 AIMA3e)
            Proposition<T> aAndB = new ConjunctiveProposition<T>(phi, conjEvidence);
            double probabilityOfEvidence = prior(conjEvidence);
            if (0 != probabilityOfEvidence)
            {
                return prior(aAndB) / probabilityOfEvidence;
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
        public override CategoricalDistribution<T> priorDistribution(params Proposition<T>[] phi)
        {
            return jointDistribution(phi);
        }

        public override CategoricalDistribution<T> posteriorDistribution(Proposition<T> phi, params Proposition<T>[] evidence)
        {

            Proposition<T> conjEvidence = ProbUtil.constructConjunction(evidence);

            // P(A | B) = P(A AND B)/P(B) - (13.3 AIMA3e)
            CategoricalDistribution<T> dAandB = jointDistribution(phi, conjEvidence);
            CategoricalDistribution<T> dEvidence = jointDistribution(conjEvidence);

            return dAandB.divideBy(dEvidence);
        }

        public override CategoricalDistribution<T> jointDistribution(params Proposition<T>[] propositions)
        {
            ProbabilityTable<T> d = null;
            Proposition<T> conjProp = ProbUtil.constructConjunction(propositions);
            ISet<RandomVariable> vars = new HashSet<RandomVariable>(conjProp.getUnboundScope());

            if (vars.Count > 0)
            {
                RandomVariable[] distVars = vars.ToArray();

                ProbabilityTable<T> ud = new ProbabilityTable<T>(distVars);
                T[] values = new T[vars.Count];

                Iterator<T> di = new Iterator<T>((possibleWorld, probability) =>
                {
                    if (conjProp.holds(possibleWorld))
                    {
                        int i = 0;
                        foreach (RandomVariable rv in vars)
                        {
                            values[i] = possibleWorld[rv];
                            ++i;
                        }
                        int dIdx = ud.getIndex(values);
                        ud.setValue(dIdx, ud.getValues()[dIdx] + probability);
                    }
                });

                distribution.iterateOverTable(di);

                d = ud;
            }
            else
            {
                // No Unbound Variables, therefore just return
                // the singular probability related to the proposition.
                d = new ProbabilityTable<T>();
                d.setValue(0, prior(propositions));
            }
            return d;
        }

        // END-FiniteProbabilityModel
        //

        //
        // PRIVATE METHODS
        //
        private double probabilityOf(Proposition<T> phi)
        {
            double[] probSum = new double[1];
            Iterator<T> di = new Iterator<T>((possibleWorld, probability) =>
               {
                   if (phi.holds(possibleWorld))
                   {
                       probSum[0] += probability;
                   }
               });

            distribution.iterateOverTable(di);

            return probSum[0];
        }
    } 
}
