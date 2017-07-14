using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.probability.bayes;
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
    public class CPT<T> : ConditionalProbabilityTable<T>
    {

        private RandomVariable on = null;
        private ISet<RandomVariable> parents = new HashSet<RandomVariable>();
        private ProbabilityTable<T> table = null;
        private List<T> onDomain = new List<T>();

        public CPT(RandomVariable on, double[] values,
                params RandomVariable[] conditionedOn)
        {
            this.on = on;
            if (null == conditionedOn)
            {
                conditionedOn = new RandomVariable[0];
            }
            RandomVariable[] tableVars = new RandomVariable[conditionedOn.Length + 1];
            for (int i = 0; i < conditionedOn.Length; ++i)
            {
                tableVars[i] = conditionedOn[i];
                parents.Add(conditionedOn[i]);
            }
            tableVars[conditionedOn.Length] = on;
            table = new ProbabilityTable<T>(values, tableVars);
            onDomain.AddRange(((FiniteDomain<T>)on.getDomain()).getPossibleValues());

            checkEachRowTotalsOne();
        }

        public double probabilityFor(params T[] values)
        {
            return table.getValue(values);
        }

        //
        // START-ConditionalProbabilityDistribution

        public RandomVariable getOn()
        {
            return on;
        }

        public ISet<RandomVariable> getParents()
        {
            return parents;
        }

        public ISet<RandomVariable> getFor()
        {
            return table.getFor();
        }

        public bool contains(RandomVariable rv)
        {
            return table.contains(rv);
        }

        public double getValue(params T[] eventValues)
        {
            return table.getValue(eventValues);
        }

        public double getValue(params AssignmentProposition<T>[] eventValues)
        {
            return table.getValue(eventValues);
        }

        public T getSample(double probabilityChoice, params T[] parentValues)
        {
            return ProbUtil.sample<T>(probabilityChoice, on, (getConditioningCase(parentValues) as CategoricalDistribution<T>).getValues());
        }

        public T getSample(double probabilityChoice, params AssignmentProposition<T>[] parentValues)
        {
            return ProbUtil.sample<T>(probabilityChoice, on, (getConditioningCase(parentValues) as CategoricalDistribution<T>).getValues());
        }

        // END-ConditionalProbabilityDistribution
        //

        //
        // START-ConditionalProbabilityTable 
        public ProbabilityDistribution<T> getConditioningCase(params T[] parentValues)
        {
            if (parentValues.Length != parents.Count)
            {
                throw new ArgumentException(
                        "The number of parent value arguments ["
                                + parentValues.Length
                                + "] is not equal to the number of parents ["
                                + parents.Count + "] for this CPT.");
            }
            AssignmentProposition<T>[] aps = new AssignmentProposition<T>[parentValues.Length];
            int idx = 0;
            foreach (RandomVariable parentRV in parents)
            {
                aps[idx] = new AssignmentProposition<T>(parentRV, parentValues[idx]);
                idx++;
            }

            return getConditioningCase(aps);
        }

        public ProbabilityDistribution<T> getConditioningCase(params AssignmentProposition<T>[] parentValues)
        {
            if (parentValues.Length != parents.Count)
            {
                throw new ArgumentException(
                        "The number of parent value arguments ["
                                + parentValues.Length
                                + "] is not equal to the number of parents ["
                                + parents.Count + "] for this CPT.");
            }
            ProbabilityTable<T> cc = new ProbabilityTable<T>(getOn());
            Iterator<T> pti = new Iterator<T>((possibleAssignment, probability) =>
            {

                int idx = 0;
                cc.getValues()[idx] = probability;
                idx++;
            });
            table.iterateOverTable(pti, parentValues);

            return cc;
        }

        public Factor<T> getFactorFor(params AssignmentProposition<T>[] evidence)
        {
            ISet<RandomVariable> fofVars = new HashSet<RandomVariable>(table.getFor());
            foreach (AssignmentProposition<T> ap in evidence)
            {
                fofVars.Remove(ap.getTermVariable());
            }
            ProbabilityTable<T> fof = new ProbabilityTable<T>(fofVars.ToArray());
            // Otherwise need to iterate through the table for the
            // non evidence variables.
            T[] termValues = new T[fofVars.Count];
            Iterator<T> di = new Iterator<T>((possibleWorld, probability) =>
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
                          termValues[i] = possibleWorld[rv];
                          ++i;
                      }
                      fof.getValues()[fof.getIndex(termValues)] += probability;
                  }
              });
            table.iterateOverTable(di, evidence);

            return fof;
        }

        // END-ConditionalProbabilityTable
        //

        //
        // PRIVATE METHODS
        //
        private void checkEachRowTotalsOne()
        {
            Iterator<T> di = new Iterator<T>((possibleWorld, probability) =>
            {
                int rowSize = onDomain.Count;
                int iterateCnt = 0;
                double rowProb = 0;

                iterateCnt++;
                rowProb += probability;
                if (iterateCnt % rowSize == 0)
                {
                    if (Math.Abs(1 - rowProb) > ProbabilityModel<T>.DEFAULT_ROUNDING_THRESHOLD)
                    {
                        throw new ArgumentException("Row "
                                + (iterateCnt / rowSize)
                                + " of CPT does not sum to 1.0.");
                    }
                    rowProb = 0;
                }
            });

            table.iterateOverTable(di);
        }
    }
}
