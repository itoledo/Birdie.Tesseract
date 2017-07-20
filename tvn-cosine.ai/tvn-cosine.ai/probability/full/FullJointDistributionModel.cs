using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
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
    public class FullJointDistributionModel : FiniteProbabilityModel
    {
        private ProbabilityTable distribution = null;
        private ISet<RandomVariable> representation = null;

        public FullJointDistributionModel(double[] values, params RandomVariable[] vars)
        {
            if (null == vars)
            {
                throw new IllegalArgumentException("Random Variables describing the model's representation of the World need to be specified.");
            }

            distribution = new ProbabilityTable(values, vars);

            representation = Factory.CreateSet<RandomVariable>();
            for (int i = 0; i < vars.Length;++i)
            {
                representation.Add(vars[i]);
            }
            representation = Factory.CreateReadOnlySet<RandomVariable>(representation);
        }

        //
        // START-ProbabilityModel
        public bool isValid()
        {
            // Handle rounding
            return System.Math.Abs(1 - distribution.getSum()) <= ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD;
        }

        public double prior(params Proposition[] phi)
        {
            return probabilityOf(ProbUtil.constructConjunction(phi));
        }

        public double posterior(Proposition phi, params Proposition[] evidence)
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

        public ISet<RandomVariable> getRepresentation()
        {
            return representation;
        }

        // END-ProbabilityModel
        //

        //
        // START-FiniteProbabilityModel
        public CategoricalDistribution priorDistribution(params Proposition[] phi)
        {
            return jointDistribution(phi);
        }

        public CategoricalDistribution posteriorDistribution(Proposition phi,
                params Proposition[] evidence)
        {

            Proposition conjEvidence = ProbUtil.constructConjunction(evidence);

            // P(A | B) = P(A AND B)/P(B) - (13.3 AIMA3e)
            CategoricalDistribution dAandB = jointDistribution(phi, conjEvidence);
            CategoricalDistribution dEvidence = jointDistribution(conjEvidence);

            return dAandB.divideBy(dEvidence);
        }

        class ProbabilityTableIterator : ProbabilityTable.ProbabilityTableIterator
        {
            private Proposition conjProp;
            private ProbabilityTable ud;
            private object[] values;
            private ISet<RandomVariable> vars;

            public ProbabilityTableIterator(Proposition conjProp, ProbabilityTable ud, object[] values, ISet<RandomVariable> vars)
            {
                this.conjProp = conjProp;
                this.ud = ud;
                this.values = values;
                this.vars = vars;
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

        public CategoricalDistribution jointDistribution(params Proposition[] propositions)
        {
            ProbabilityTable d = null;
            Proposition conjProp = ProbUtil.constructConjunction(propositions);
            ISet<RandomVariable> vars = Factory.CreateSet<RandomVariable>(conjProp.getUnboundScope());

            if (vars.Size() > 0)
            {
                RandomVariable[] distVars = vars.ToArray();

                ProbabilityTable ud = new ProbabilityTable(distVars);
                object[] values = new object[vars.Size()];

                ProbabilityTable.ProbabilityTableIterator di = new ProbabilityTableIterator(conjProp, ud, values, vars);

                distribution.iterateOverTable(di);

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
         
        class ProbabilityTableIteratorImpl2 : ProbabilityTable.ProbabilityTableIterator
        {
            private Proposition phi;
            private double[] probSum;

            public ProbabilityTableIteratorImpl2(double[] probSum, Proposition phi)
            {
                this.probSum = probSum;
                this.phi = phi;
            }

            public void iterate(IMap<RandomVariable, object> possibleWorld, double probability)
            {
                if (phi.holds(possibleWorld))
                {
                    probSum[0] += probability;
                }
            }
        }
        
        private double probabilityOf(Proposition phi)
        {
            double[] probSum = new double[1];
            ProbabilityTable.ProbabilityTableIterator di = new ProbabilityTableIteratorImpl2(probSum, phi);

            distribution.iterateOverTable(di);

            return probSum[0];
        }
    } 
}
