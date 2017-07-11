using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.probability.domain;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.util.math;

namespace tvn.cosine.ai.probability.util
{
    /**
     * A Utility Class for associating values with a set of finite Random Variables.
     * This is also the default implementation of the CategoricalDistribution and
     * Factor interfaces (as they are essentially dependent on the same underlying
     * data structures).
     * 
     * @author Ciaran O'Reilly
     */
    public class ProbabilityTable<T> : CategoricalDistribution<T>, Factor<T>
    {
        private double[] values = null;
        //
        private IDictionary<RandomVariable, RVInfo> randomVarInfo = new Dictionary<RandomVariable, RVInfo>();
        private int[] radices = null;
        private MixedRadixNumber queryMRN = null;
        //
        private string toString = null;
        private double sum = -1;

        public ProbabilityTable(params RandomVariable[] vars)
            : this(new double[ProbUtil.expectedSizeOfProbabilityTable<T>(vars)], vars)
        { }

        public ProbabilityTable(double[] vals, params RandomVariable[] vars)
        {
            if (null == vals)
            {
                throw new ArgumentException("Values must be specified");
            }
            if (vals.Length != ProbUtil.expectedSizeOfProbabilityTable<T>(vars))
            {
                throw new ArgumentException("ProbabilityTable of length "
                        + vals.Length + " is not the correct size, should be "
                        + ProbUtil.expectedSizeOfProbabilityTable<T>(vars)
                        + " in order to represent all possible combinations.");
            }
            if (null != vars)
            {
                foreach (RandomVariable rv in vars)
                {
                    // Track index information relevant to each variable.
                    randomVarInfo.Add(rv, new RVInfo(rv));
                }
            }

            values = new double[vals.Length];
            Array.Copy(vals, 0, values, 0, vals.Length);

            radices = createRadixs(randomVarInfo);

            if (radices.Length > 0)
            {
                queryMRN = new MixedRadixNumber(0, radices);
            }
        }

        public int size()
        {
            return values.Length;
        }

        //
        // START-ProbabilityDistribution 
        public ISet<RandomVariable> getFor()
        {
            return new HashSet<RandomVariable>(randomVarInfo.Keys);
        }

        public bool contains(RandomVariable rv)
        {
            return randomVarInfo.Keys.Contains(rv);
        }

        public double getValue(params T[] assignments)
        {
            return values[getIndex(assignments)];
        }

        public double getValue(params AssignmentProposition<T>[] assignments)
        {
            var assignmentsLength = assignments.Count();
            if (assignmentsLength != randomVarInfo.Count)
            {
                throw new ArgumentException("Assignments passed in is not the same size as variables making up probability table.");
            }
            int[] radixValues = new int[assignmentsLength];
            foreach (AssignmentProposition<T> ap in assignments)
            {
                RVInfo rvInfo = randomVarInfo[ap.getTermVariable()];
                if (null == rvInfo)
                {
                    throw new ArgumentException("Assignment passed for a variable that is not part of this probability table:"
                                    + ap.getTermVariable());
                }
                radixValues[rvInfo.getRadixIdx()] = rvInfo.getIdxForDomain(ap
                        .getValue());
            }
            return values[(int)queryMRN.getCurrentValueFor(radixValues)];
        }

        // END-ProbabilityDistribution
        //

        //
        // START-CategoricalDistribution
        public double[] getValues()
        {
            return values;
        }

        public void setValue(int idx, double value)
        {
            values[idx] = value;
            reinitLazyValues();
        }

        public double getSum()
        {
            if (-1 == sum)
            {
                sum = 0;
                for (int i = 0; i < values.Length; i++)
                {
                    sum += values[i];
                }
            }
            return sum;
        }

        public CategoricalDistribution<T> normalize()
        {
            double s = getSum();
            if (s != 0 && s != 1.0)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = values[i] / s;
                }
                reinitLazyValues();
            }
            return this;
        }

        public int getIndex(params T[] assignments)
        {
            var assignmentsLength = assignments.Count();
            if (assignmentsLength != randomVarInfo.Count)
            {
                throw new ArgumentException("Assignments passed in is not the same size as variables making up the table.");
            }
            int[] radixValues = new int[assignmentsLength];
            int i = 0;
            foreach (RVInfo rvInfo in randomVarInfo.Values)
            {
                radixValues[rvInfo.getRadixIdx()] = rvInfo.getIdxForDomain(assignments.ElementAt(i));
                i++;
            }

            return (int)queryMRN.getCurrentValueFor(radixValues);
        }

        public CategoricalDistribution<T> marginal(params RandomVariable[] vars)
        {
            return sumOut(vars) as CategoricalDistribution<T>;
        }

        public CategoricalDistribution<T> divideBy(CategoricalDistribution<T> divisor)
        {
            return divideBy((ProbabilityTable<T>)divisor);
        }

        public CategoricalDistribution<T> multiplyBy(CategoricalDistribution<T> multiplier)
        {
            return pointwiseProduct((ProbabilityTable<T>)multiplier);
        }

        public CategoricalDistribution<T> multiplyByPOS(CategoricalDistribution<T> multiplier, params RandomVariable[] prodVarOrder)
        {
            return pointwiseProductPOS((ProbabilityTable<T>)multiplier, prodVarOrder);
        }

        public void iterateOver(Iterator<T> cdi)
        {
            iterateOverTable(new CategoricalDistributionIteratorAdapter(cdi));
        }

        public void iterateOver(Iterator<T> cdi, params AssignmentProposition<T>[] fixedValues)
        {
            iterateOverTable(new CategoricalDistributionIteratorAdapter(cdi), fixedValues);
        }

        // END-CategoricalDistribution
        //

        //
        // START-Factor 
        public ISet<RandomVariable> getArgumentVariables()
        {
            return new HashSet<RandomVariable>(randomVarInfo.Keys);
        }

        public Factor<T> sumOut(params RandomVariable[] vars)
        {
            ISet<RandomVariable> soutVars = new HashSet<RandomVariable>(this.randomVarInfo.Keys);
            foreach (RandomVariable rv in vars)
            {
                soutVars.Remove(rv);
            }
            ProbabilityTable<T> summedOut = new ProbabilityTable<T>(soutVars.ToArray());
            if (1 == summedOut.getValues().Length)
            {
                summedOut.getValues()[0] = getSum();
            }
            else
            {
                // Otherwise need to iterate through this distribution
                // to calculate the summed out distribution.
                T[] termValues = new T[summedOut.randomVarInfo.Count];
                Iterator<T> di = new Iterator<T>((possibleWorld, probability) =>
                {
                    int i = 0;
                    foreach (RandomVariable rv in summedOut.randomVarInfo.Keys)
                    {
                        termValues[i] = possibleWorld[rv];
                        i++;
                    }
                    summedOut.getValues()[summedOut.getIndex(termValues)] += probability;
                });

                iterateOverTable(di);
            }

            return summedOut;
        }

        public Factor<T> pointwiseProduct(Factor<T> multiplier)
        {
            return pointwiseProduct((ProbabilityTable<T>)multiplier);
        }

        public Factor<T> pointwiseProductPOS(Factor<T> multiplier,params RandomVariable[] prodVarOrder)
        {
            return pointwiseProductPOS((ProbabilityTable<T>)multiplier, prodVarOrder);
        }

        public void iterateOverFactor(Iterator<T> fi)
        {
            iterateOverTable(new FactorIteratorAdapter(fi));
        }

        public void iterateOverFactorAssignmentProposition(Iterator<T> fi, params AssignmentProposition<T>[] fixedValues)
        {
            iterateOverTable(new FactorIteratorAdapter(fi), fixedValues);
        }

        // END-Factor
        //

        /**
         * Iterate over all the possible value assignments for the Random Variables
         * comprising this ProbabilityTable.
         * 
         * @param pti
         *            the ProbabilityTable Iterator to iterate.
         */
        public void iterateOverTable(Iterator<T> pti)
        {
            IDictionary<RandomVariable, T> possibleWorld = new Dictionary<RandomVariable, T>();
            MixedRadixNumber mrn = new MixedRadixNumber(0, radices);
            do
            {
                foreach (RVInfo rvInfo in randomVarInfo.Values)
                {
                    possibleWorld.Add(rvInfo.getVariable(), rvInfo
                            .getDomainValueAt(mrn.getCurrentNumeralValue(rvInfo
                                    .getRadixIdx())));
                }
                pti.iterate(possibleWorld, values[mrn.intValue()]);

            } while (mrn.increment());
        }

        /**
         * Iterate over all possible values assignments for the Random Variables
         * comprising this ProbabilityTable that are not in the fixed set of values.
         * This allows you to iterate over a subset of possible combinations.
         * 
         * @param pti
         *            the ProbabilityTable Iterator to iterate
         * @param fixedValues
         *            Fixed values for a subset of the Random Variables comprising
         *            this Probability Table.
         */
        public void iterateOverTable(Iterator<T> pti, params AssignmentProposition<T>[] fixedValues)
        {
            IDictionary<RandomVariable, T> possibleWorld = new Dictionary<RandomVariable, T>();
            MixedRadixNumber tableMRN = new MixedRadixNumber(0, radices);
            int[] tableRadixValues = new int[radices.Length];

            // Assert that the Random Variables for the fixed values
            // are part of this probability table and assign
            // all the fixed values to the possible world.
            foreach (AssignmentProposition<T> ap in fixedValues)
            {
                if (!randomVarInfo.ContainsKey(ap.getTermVariable()))
                {
                    throw new ArgumentException("Assignment proposition [" + ap + "] does not belong to this probability table.");
                }
                possibleWorld.Add(ap.getTermVariable(), ap.getValue());
                RVInfo fixedRVI = randomVarInfo[ap.getTermVariable()];
                tableRadixValues[fixedRVI.getRadixIdx()] = fixedRVI.getIdxForDomain(ap.getValue());
            }
            // If have assignments for all the random variables
            // in this probability table
            if (fixedValues.Count() == randomVarInfo.Count)
            {
                // Then only 1 iteration call is required.
                pti.iterate(possibleWorld, getValue(fixedValues));
            }
            else
            {
                // Else iterate over the non-fixed values
                ISet<RandomVariable> freeVariables = new HashSet<RandomVariable>(this.randomVarInfo.Keys.Except(possibleWorld.Keys));
                IDictionary<RandomVariable, RVInfo> freeVarInfo = new Dictionary<RandomVariable, RVInfo>();
                // Remove the fixed Variables
                foreach (RandomVariable fv in freeVariables)
                {
                    freeVarInfo.Add(fv, new RVInfo(fv));
                }
                int[] freeRadixValues = createRadixs(freeVarInfo);
                MixedRadixNumber freeMRN = new MixedRadixNumber(0, freeRadixValues);
                T fval;
                // Iterate through all combinations of the free variables
                do
                {
                    // Put the current assignments for the free variables
                    // into the possible world and update
                    // the current index in the table MRN
                    foreach (RVInfo freeRVI in freeVarInfo.Values)
                    {
                        fval = freeRVI.getDomainValueAt(freeMRN
                                .getCurrentNumeralValue(freeRVI.getRadixIdx()));
                        possibleWorld.Add(freeRVI.getVariable(), fval);

                        tableRadixValues[randomVarInfo[freeRVI.getVariable()].getRadixIdx()] = freeRVI.getIdxForDomain(fval);
                    }
                    pti.iterate(possibleWorld, values[(int)tableMRN.getCurrentValueFor(tableRadixValues)]);

                } while (freeMRN.increment());
            }
        }

        public ProbabilityTable<T> divideBy(ProbabilityTable<T> divisor)
        {
            if (!randomVarInfo.Keys.Intersect(divisor.randomVarInfo.Keys).Any())
            {
                throw new ArgumentException("Divisor must be a subset of the dividend.");
            }

            ProbabilityTable<T> quotient = new ProbabilityTable<T>(randomVarInfo.Keys.ToArray());

            if (1 == divisor.getValues().Length)
            {
                double d = divisor.getValues()[0];
                for (int i = 0; i < quotient.getValues().Length; i++)
                {
                    if (0 == d)
                    {
                        quotient.getValues()[i] = 0;
                    }
                    else
                    {
                        quotient.getValues()[i] = getValues()[i] / d;
                    }
                }
            }
            else
            {
                ISet<RandomVariable> dividendDivisorDiff = new HashSet<RandomVariable>(this.randomVarInfo.Keys.Except(divisor.randomVarInfo.Keys));
                IDictionary<RandomVariable, RVInfo> tdiff = null;
                MixedRadixNumber tdMRN = null;
                if (dividendDivisorDiff.Count > 0)
                {
                    tdiff = new Dictionary<RandomVariable, RVInfo>();
                    foreach (RandomVariable rv in dividendDivisorDiff)
                    {
                        tdiff.Add(rv, new RVInfo(rv));
                    }
                    tdMRN = new MixedRadixNumber(0, createRadixs(tdiff));
                }
                IDictionary<RandomVariable, RVInfo> diff = tdiff;
                MixedRadixNumber dMRN = tdMRN;
                int[] qRVs = new int[quotient.radices.Length];
                MixedRadixNumber qMRN = new MixedRadixNumber(0, quotient.radices);
                Iterator<T> divisorIterator = new Iterator<T>((possibleWorld, probability) =>
                {
                    Action<double> updateQuotient = (p) =>
                    {
                        int offset = (int)qMRN.getCurrentValueFor(qRVs);
                        if (0 == p)
                        {
                            quotient.getValues()[offset] = 0;
                        }
                        else
                        {
                            quotient.getValues()[offset] += getValues()[offset] / p;
                        }
                    };

                    foreach (RandomVariable rv in possibleWorld.Keys)
                    {
                        RVInfo rvInfo = quotient.randomVarInfo[rv];
                        qRVs[rvInfo.getRadixIdx()] = rvInfo.getIdxForDomain(possibleWorld[rv]);
                    }
                    if (null != diff)
                    {
                        // Start from 0 off the diff
                        dMRN.setCurrentValueFor(new int[diff.Count]);
                        do
                        {
                            foreach (RandomVariable rv in diff.Keys)
                            {
                                RVInfo drvInfo = diff[rv];
                                RVInfo qrvInfo = quotient.randomVarInfo[rv];
                                qRVs[qrvInfo.getRadixIdx()] = dMRN.getCurrentNumeralValue(drvInfo.getRadixIdx());
                            }
                            updateQuotient(probability);
                        } while (dMRN.increment());
                    }
                    else
                    {
                        updateQuotient(probability);
                    }
                });

                divisor.iterateOverTable(divisorIterator);
            }

            return quotient;
        }

        public ProbabilityTable<T> pointwiseProduct(ProbabilityTable<T> multiplier)
        {
            ISet<RandomVariable> prodVars = new HashSet<RandomVariable>(randomVarInfo.Keys.Union(multiplier.randomVarInfo.Keys));
            return pointwiseProductPOS(multiplier, prodVars.ToArray());
        }

        public ProbabilityTable<T> pointwiseProductPOS(ProbabilityTable<T> multiplier, params RandomVariable[] prodVarOrder)
        {
            ProbabilityTable<T> product = new ProbabilityTable<T>(prodVarOrder);
            if (product.randomVarInfo.Keys.Union(randomVarInfo.Keys).Union(multiplier.randomVarInfo.Keys).Count() != product.randomVarInfo.Keys.Count)
            {
                throw new ArgumentException("Specified list deatailing order of mulitplier is inconsistent.");
            }

            // If no variables in the product
            if (1 == product.getValues().Length)
            {
                product.getValues()[0] = getValues()[0] * multiplier.getValues()[0];
            }
            else
            {

                // Otherwise need to iterate through the product
                // to calculate its values based on the terms.
                T[] term1Values = new T[randomVarInfo.Count];
                T[] term2Values = new T[multiplier.randomVarInfo.Count];
                Iterator<T> di = new Iterator<T>((possibleWorld, probability) =>
                {
                    Func<T[], ProbabilityTable<T>, IDictionary<RandomVariable, T>, int> termIdx = (tv, d, pw) =>
                    {
                        if (0 == tv.Length)
                        {
                            // The term has no variables so always position 0.
                            return 0;
                        }

                        int i = 0;
                        foreach (RandomVariable rv in d.randomVarInfo.Keys)
                        {
                            tv[i] = pw[rv];
                            i++;
                        }

                        return d.getIndex(tv);
                    };
                    int idx = 0;

                    int term1Idx = termIdx(term1Values, this, possibleWorld);
                    int term2Idx = termIdx(term2Values, multiplier, possibleWorld);

                    product.getValues()[idx] = getValues()[term1Idx] * multiplier.getValues()[term2Idx];

                    idx++;
                });
                product.iterateOverTable(di);
            }

            return product;
        }

        public override string ToString()
        {
            if (null == toString)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<");
                for (int i = 0; i < values.Length; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(values[i]);
                }
                sb.Append(">");

                toString = sb.ToString();
            }
            return toString;
        }

        //
        // PRIVATE METHODS
        //
        private void reinitLazyValues()
        {
            sum = -1;
            toString = null;
        }

        private int[] createRadixs(IDictionary<RandomVariable, RVInfo> mapRtoInfo)
        {
            int[] r = new int[mapRtoInfo.Count];
            // Read in reverse order so that the enumeration
            // through the distributions is of the following
            // order using a MixedRadixNumber, e.g. for two bool s:
            // X Y
            // true true
            // true false
            // false true
            // false false
            // which corresponds with how displayed in book.
            int x = mapRtoInfo.Count - 1;
            foreach (RVInfo rvInfo in mapRtoInfo.Values)
            {
                r[x] = rvInfo.getDomainSize();
                rvInfo.setRadixIdx(x);
                x--;
            }
            return r;
        }

        private class RVInfo
        {
            private RandomVariable variable;
            private FiniteDomain<T> varDomain;
            private int radixIdx = 0;

            public RVInfo(RandomVariable rv)
            {
                variable = rv;
                varDomain = (FiniteDomain<T>)variable.getDomain();
            }

            public RandomVariable getVariable()
            {
                return variable;
            }

            public int getDomainSize()
            {
                return varDomain.size();
            }

            public int getIdxForDomain(T value)
            {
                return varDomain.getOffset(value);
            }

            public T getDomainValueAt(int idx)
            {
                return varDomain.getValueAt(idx);
            }

            public void setRadixIdx(int idx)
            {
                radixIdx = idx;
            }

            public int getRadixIdx()
            {
                return radixIdx;
            }
        }

        private class CategoricalDistributionIteratorAdapter : Iterator<T>
        {
            private Iterator<T> cdi = null;

            public CategoricalDistributionIteratorAdapter(Iterator<T> cdi)
                : base(cdi.iterate)
            {
                this.cdi = cdi;
            }
        }

        private class FactorIteratorAdapter : Iterator<T>
        {
            private Iterator<T> fi = null;

            public FactorIteratorAdapter(Iterator<T> fi)
                : base(fi.iterate)
            {
                this.fi = fi;
            }

        }
    }
}
