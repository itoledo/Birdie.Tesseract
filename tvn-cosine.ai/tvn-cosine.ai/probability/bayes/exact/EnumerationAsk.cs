﻿using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.probability.domain;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.probability.bayes.exact
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 14.9, page
     * 525.<br>
     * <br>
     * 
     * <pre>
     * function ENUMERATION-ASK(X, e, bn) returns a distribution over X
     *   inputs: X, the query variable
     *           e, observed values for variables E
     *           bn, a Bayes net with variables {X} &cup; E &cup; Y /* Y = hidden variables //
     *           
     *   Q(X) <- a distribution over X, initially empty
     *   for each value x<sub>i</sub> of X do
     *       Q(x<sub>i</sub>) <- ENUMERATE-ALL(bn.VARS, e<sub>x<sub>i</sub></sub>)
     *          where e<sub>x<sub>i</sub></sub> is e extended with X = x<sub>i</sub>
     *   return NORMALIZE(Q(X))
     *   
     * ---------------------------------------------------------------------------------------------------
     * 
     * function ENUMERATE-ALL(vars, e) returns a real number
     *   if EMPTY?(vars) then return 1.0
     *   Y <- FIRST(vars)
     *   if Y has value y in e
     *       then return P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e)
     *       else return &sum;<sub>y</sub> P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e<sub>y</sub>)
     *           where e<sub>y</sub> is e extended with Y = y
     * </pre>
     * 
     * Figure 14.9 The enumeration algorithm for answering queries on Bayesian
     * networks. <br>
     * <br>
     * <b>Note:</b> The implementation has been extended to handle queries with
     * multiple variables. <br>
     * 
     * @author Ciaran O'Reilly
     */
    public class EnumerationAsk : BayesInference
    {
        public EnumerationAsk()
        { }

        class ProbabilityTableIteratorImpl : ProbabilityTable.ProbabilityTableIterator
        {
            private BayesianNetwork bn;
            int cnt = 0;
            private ObservedEvidence e;
            private EnumerationAsk enumerationAsk;
            private ProbabilityTable q;
            private RandomVariable[] x;

            public ProbabilityTableIteratorImpl(BayesianNetwork bn, ProbabilityTable q, ObservedEvidence e, RandomVariable[] x, EnumerationAsk enumerationAsk)
            {
                this.bn = bn;
                this.q = q;
                this.e = e;
                this.x = x;
                this.enumerationAsk = enumerationAsk;
            }

            /**
			 * <pre>
			 * Q(x<sub>i</sub>) <- ENUMERATE-ALL(bn.VARS, e<sub>x<sub>i</sub></sub>)
			 *   where e<sub>x<sub>i</sub></sub> is e extended with X = x<sub>i</sub>
			 * </pre>
			 */
            public void iterate(IMap<RandomVariable, object> possibleWorld, double probability)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    e.setExtendedValue(x[i], possibleWorld.Get(x[i]));
                }
                q.setValue(cnt, enumerationAsk.enumerateAll(bn.getVariablesInTopologicalOrder(), e));
                cnt++;
            }
        }

        // function ENUMERATION-ASK(X, e, bn) returns a distribution over X
        /**
         * The ENUMERATION-ASK algorithm in Figure 14.9 evaluates expression trees
         * (Figure 14.8) using depth-first recursion.
         * 
         * @param X
         *            the query variables.
         * @param observedEvidence
         *            observed values for variables E.
         * @param bn
         *            a Bayes net with variables {X} &cup; E &cup; Y /* Y = hidden
         *            variables //
         * @return a distribution over the query variables.
         */
        public CategoricalDistribution enumerationAsk(RandomVariable[] X,
                AssignmentProposition[] observedEvidence,
                BayesianNetwork bn)
        {

            // Q(X) <- a distribution over X, initially empty
            ProbabilityTable Q = new ProbabilityTable(X);
            ObservedEvidence e = new ObservedEvidence(X, observedEvidence, bn);
            // for each value x<sub>i</sub> of X do
            ProbabilityTable.ProbabilityTableIterator di = new ProbabilityTableIteratorImpl(bn, Q, e, X, this);
            Q.iterateOverTable(di);

            // return NORMALIZE(Q(X))
            return Q.normalize();
        }

        //
        // START-BayesInference
        public CategoricalDistribution ask(RandomVariable[] X,
                AssignmentProposition[] observedEvidence,
                BayesianNetwork bn)
        {
            return this.enumerationAsk(X, observedEvidence, bn);
        }

        // END-BayesInference
        //

        //
        // PROTECTED METHODS
        //
        // function ENUMERATE-ALL(vars, e) returns a real number
        protected double enumerateAll(IQueue<RandomVariable> vars, ObservedEvidence e)
        {
            // if EMPTY?(vars) then return 1.0
            if (0 == vars.Size())
            {
                return 1;
            }
            // Y <- FIRST(vars)
            RandomVariable Y = Util.first(vars);
            // if Y has value y in e
            if (e.containsValue(Y))
            {
                // then return P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e)
                return e.posteriorForParents(Y) * enumerateAll(Util.rest(vars), e);
            }
            /**
             * <pre>
             *  else return &sum;<sub>y</sub> P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e<sub>y</sub>)
             *       where e<sub>y</sub> is e extended with Y = y
             * </pre>
             */
            double sum = 0;
            foreach (object y in ((FiniteDomain)Y.getDomain()).getPossibleValues())
            {
                e.setExtendedValue(Y, y);
                sum += e.posteriorForParents(Y) * enumerateAll(Util.rest(vars), e);
            }

            return sum;
        }

        protected class ObservedEvidence
        {
            private BayesianNetwork bn = null;
            private object[] extendedValues = null;
            private int hiddenStart = 0;
            private int extendedIdx = 0;
            private RandomVariable[] var = null;
            private IMap<RandomVariable, int> varIdxs = Factory.CreateMap<RandomVariable, int>();

            public ObservedEvidence(RandomVariable[] queryVariables,
                    AssignmentProposition[] e, BayesianNetwork bn)
            {
                this.bn = bn;

                int maxSize = bn.getVariablesInTopologicalOrder().Size();
                extendedValues = new object[maxSize];
                var = new RandomVariable[maxSize];
                // query variables go first
                int idx = 0;
                for (int i = 0; i < queryVariables.Length; i++)
                {
                    var[idx] = queryVariables[i];
                    varIdxs.Put(var[idx], idx);
                    idx++;
                }
                // initial evidence variables go next
                for (int i = 0; i < e.Length; i++)
                {
                    var[idx] = e[i].getTermVariable();
                    varIdxs.Put(var[idx], idx);
                    extendedValues[idx] = e[i].getValue();
                    idx++;
                }
                extendedIdx = idx - 1;
                hiddenStart = idx;
                // the remaining slots are left open for the hidden variables
                foreach (RandomVariable rv in bn.getVariablesInTopologicalOrder())
                {
                    if (!varIdxs.ContainsKey(rv))
                    {
                        var[idx] = rv;
                        varIdxs.Put(var[idx], idx);
                        idx++;
                    }
                }
            }

            public void setExtendedValue(RandomVariable rv, object value)
            {
                int idx = varIdxs.Get(rv);
                extendedValues[idx] = value;
                if (idx >= hiddenStart)
                {
                    extendedIdx = idx;
                }
                else
                {
                    extendedIdx = hiddenStart - 1;
                }
            }

            public bool containsValue(RandomVariable rv)
            {
                return varIdxs.Get(rv) <= extendedIdx;
            }

            public double posteriorForParents(RandomVariable rv)
            {
                Node n = bn.getNode(rv);
                if (!(n is FiniteNode))
                {
                    throw new IllegalArgumentException("Enumeration-Ask only works with finite Nodes.");
                }
                FiniteNode fn = (FiniteNode)n;
                object[] vals = new object[1 + fn.getParents().Size()];
                int idx = 0;
                foreach (Node pn in n.getParents())
                {
                    vals[idx] = extendedValues[varIdxs.Get(pn.getRandomVariable())];
                    idx++;
                }
                vals[idx] = extendedValues[varIdxs.Get(rv)];

                return fn.getCPT().getValue(vals);
            }
        }

        //
        // PRIVATE METHODS
        //
    }
}
