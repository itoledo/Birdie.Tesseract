using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.probability.domain;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;

namespace tvn.cosine.ai.probability.bayes.impl
{
    /**
     * Default implementation of the ConditionalProbabilityTable interface.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class CPT : ConditionalProbabilityTable
    { 
        private RandomVariable on = null;
        private ISet<RandomVariable> parents = Factory.CreateSet<RandomVariable>();
        private ProbabilityTable table = null;
        private IQueue<object> onDomain = Factory.CreateQueue<object>();

        public CPT(RandomVariable on, double[] values, params RandomVariable[] conditionedOn)
        {
            this.on = on;
            if (null == conditionedOn)
            {
                conditionedOn = new RandomVariable[0];
            }
            RandomVariable[] tableVars = new RandomVariable[conditionedOn.Length + 1];
            for (int i = 0; i < conditionedOn.Length; i++)
            {
                tableVars[i] = conditionedOn[i];
                parents.Add(conditionedOn[i]);
            }
            tableVars[conditionedOn.Length] = on;
            table = new ProbabilityTable(values, tableVars);
            onDomain.AddAll(((FiniteDomain)on.getDomain()).getPossibleValues());

            checkEachRowTotalsOne();
        }

        public virtual double probabilityFor(params object[] values)
        {
            return table.getValue(values);
        }

        public virtual RandomVariable getOn()
        {
            return on;
        }


        public virtual ISet<RandomVariable> getParents()
        {
            return parents;
        }


        public virtual ISet<RandomVariable> getFor()
        {
            return table.getFor();
        }


        public virtual bool contains(RandomVariable rv)
        {
            return table.contains(rv);
        }


        public virtual double getValue(params object[] eventValues)
        {
            return table.getValue(eventValues);
        }


        public virtual double getValue(params AssignmentProposition[] eventValues)
        {
            return table.getValue(eventValues);
        }


        public virtual object getSample(double probabilityChoice, params object[] parentValues)
        {
            return ProbUtil.sample(probabilityChoice, on, getConditioningCase(parentValues).getValues());
        }


        public virtual object getSample(double probabilityChoice, params AssignmentProposition[] parentValues)
        {
            return ProbUtil.sample(probabilityChoice, on, getConditioningCase(parentValues).getValues());
        }

        ProbabilityDistribution ConditionalProbabilityDistribution.getConditioningCase(params object[] parentValues)
        {
            return getConditioningCase(parentValues);
        }
         
        public virtual CategoricalDistribution getConditioningCase(params object[] parentValues)
        {
            if (parentValues.Length != parents.Size())
            {
                throw new IllegalArgumentException(
                        "The number of parent value arguments ["
                                + parentValues.Length
                                + "] is not equal to the number of parents ["
                                + parents.Size() + "] for this CPT.");
            }
            AssignmentProposition[] aps = new AssignmentProposition[parentValues.Length];
            int idx = 0;
            foreach (RandomVariable parentRV in parents)
            {
                aps[idx] = new AssignmentProposition(parentRV, parentValues[idx]);
                idx++;
            }

            return getConditioningCase(aps);
        }

        class GetConditionCaseIterator : ProbabilityTable.ProbabilityTableIterator
        {
            private ProbabilityTable cc;
            private int idx = 0;

            public GetConditionCaseIterator(ProbabilityTable cc)
            {
                this.cc = cc;
            }

            public void iterate(IMap<RandomVariable, object> possibleAssignment, double probability)
            {
                cc.getValues()[idx] = probability;
                idx++;
            }
        }

        ProbabilityDistribution ConditionalProbabilityDistribution.getConditioningCase(params AssignmentProposition[] parentValues)
        {
            return getConditioningCase(parentValues);
        }

        public virtual CategoricalDistribution getConditioningCase(params AssignmentProposition[] parentValues)
        {
            if (parentValues.Length != parents.Size())
            {
                throw new IllegalArgumentException(
                        "The number of parent value arguments ["
                                + parentValues.Length
                                + "] is not equal to the number of parents ["
                                + parents.Size() + "] for this CPT.");
            }
            ProbabilityTable cc = new ProbabilityTable(getOn());
            ProbabilityTable.ProbabilityTableIterator pti = new GetConditionCaseIterator(cc);
            table.iterateOverTable(pti, parentValues);

            return cc;
        }

        class getFactorForIterator : ProbabilityTable.ProbabilityTableIterator
        {
            private ProbabilityTable fof;
            private object[] termValues;

            public getFactorForIterator(object[] termValues, ProbabilityTable fof)
            {
                this.termValues = termValues;
                this.fof = fof;
            }

            public void iterate(IMap<RandomVariable, object> possibleWorld, double probability)
            {
                if (0 == termValues.Length)
                {
                    fof.getValues()[0] += probability;
                }
                else
                {
                    int i = 0;
                    foreach (RandomVariable rv in fof.getFor())
                    {
                        termValues[i] = possibleWorld.Get(rv);
                        i++;
                    }
                    fof.getValues()[fof.getIndex(termValues)] += probability;
                }
            }
        }

        public virtual Factor getFactorFor(params AssignmentProposition[] evidence)
        {
            ISet<RandomVariable> fofVars = Factory.CreateSet<RandomVariable>(table.getFor());
            foreach (AssignmentProposition ap in evidence)
            {
                fofVars.Remove(ap.getTermVariable());
            }
            ProbabilityTable fof = new ProbabilityTable(fofVars);
            // Otherwise need to iterate through the table for the
            // non evidence variables.
            object[] termValues = new object[fofVars.Size()];
            ProbabilityTable.ProbabilityTableIterator di = new getFactorForIterator(termValues, fof);
            table.iterateOverTable(di, evidence);

            return fof;
        }

        class checkEachRowTotalsOneIterator : ProbabilityTable.ProbabilityTableIterator
        {
            private int rowSize;
            private int iterateCnt = 0;
            private double rowProb = 0;
            private IQueue<object> onDomain;

            public checkEachRowTotalsOneIterator(IQueue<object> onDomain)
            {
                this.onDomain = onDomain;
                rowSize = onDomain.Size();
            }

            public void iterate(IMap<RandomVariable, object> possibleWorld,
                    double probability)
            {
                iterateCnt++;
                rowProb += probability;
                if (iterateCnt % rowSize == 0)
                {
                    if (System.Math.Abs(1 - rowProb) > ProbabilityModelImpl.DEFAULT_ROUNDING_THRESHOLD)
                    {
                        throw new IllegalArgumentException("Row "
                                + (iterateCnt / rowSize)
                                + " of CPT does not sum to 1.0.");
                    }
                    rowProb = 0;
                }
            }
        }

        private void checkEachRowTotalsOne()
        {
            ProbabilityTable.ProbabilityTableIterator di = new checkEachRowTotalsOneIterator(onDomain);

            table.iterateOverTable(di);
        }
    }
}
