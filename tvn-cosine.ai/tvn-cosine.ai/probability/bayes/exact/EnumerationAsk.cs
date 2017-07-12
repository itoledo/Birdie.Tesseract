using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.bayes;
using tvn.cosine.ai.probability.domain;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;

namespace tvn.cosine.ai.probability.bayes.exact
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 14.9, page
     * 525. 
     *  
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
     * networks.  
     *  
     * <b>Note:</b> The implementation has been extended to handle queries with
     * multiple variables.  
     * 
     * @author Ciaran O'Reilly
     */
    public class EnumerationAsk<T> : BayesInference<T>
    {
        public EnumerationAsk()
        {

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
        public CategoricalDistribution<T> enumerationAsk(RandomVariable[] X,
                 AssignmentProposition<T>[] observedEvidence,
                 BayesianNetwork<T> bn)
        {

            // Q(X) <- a distribution over X, initially empty
            ProbabilityTable<T> Q = new ProbabilityTable<T>(X);
            ObservedEvidence e = new ObservedEvidence(X, observedEvidence, bn);
            // for each value x<sub>i</sub> of X do
            Iterator<T> di = new Iterator<T>((possibleWorld, probability) =>
           {
               int cnt = 0;
               for (int i = 0; i < X.Length; i++)
               {
                   e.setExtendedValue(X[i], possibleWorld[X[i]]);
               }
               Q.setValue(cnt, enumerateAll(bn.getVariablesInTopologicalOrder(), e));
               cnt++;
           });

            Q.iterateOverTable(di);
            // return NORMALIZE(Q(X))
            return Q.normalize();
        }

        //
        // START-BayesInference
        public CategoricalDistribution<T> ask(RandomVariable[] X,
                AssignmentProposition<T>[] observedEvidence,
                BayesianNetwork<T> bn)
        {
            return this.enumerationAsk(X, observedEvidence, bn);
        }

        // END-BayesInference
        //

        //
        // PROTECTED METHODS
        //
        // function ENUMERATE-ALL(vars, e) returns a real number
        protected double enumerateAll(IList<RandomVariable> vars, ObservedEvidence e)
        {
            // if EMPTY?(vars) then return 1.0
            if (0 == vars.Count)
            {
                return 1;
            }
            // Y <- FIRST(vars)
            RandomVariable Y = vars.First();
            // if Y has value y in e
            if (e.containsValue(Y))
            {
                // then return P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e)
                return e.posteriorForParents(Y) * enumerateAll(vars.Skip(1).ToList(), e);
            }
            /**
             * <pre>
             *  else return &sum;<sub>y</sub> P(y | parents(Y)) * ENUMERATE-ALL(REST(vars), e<sub>y</sub>)
             *       where e<sub>y</sub> is e extended with Y = y
             * </pre>
             */
            double sum = 0;
            foreach (T y in ((FiniteDomain<T>)Y.getDomain()).getPossibleValues())
            {
                e.setExtendedValue(Y, y);
                sum += e.posteriorForParents(Y) * enumerateAll(vars.Skip(1).ToList(), e);
            }

            return sum;
        }

        protected class ObservedEvidence
        {
            private BayesianNetwork<T> bn = null;
            private T[] extendedValues = null;
            private int hiddenStart = 0;
            private int extendedIdx = 0;
            private RandomVariable[] var = null;
            private IDictionary<RandomVariable, int> varIdxs = new Dictionary<RandomVariable, int>();

            public ObservedEvidence(RandomVariable[] queryVariables, AssignmentProposition<T>[] e, BayesianNetwork<T> bn)
            {
                this.bn = bn;

                int maxSize = bn.getVariablesInTopologicalOrder().Count;
                extendedValues = new T[maxSize];
                var = new RandomVariable[maxSize];
                // query variables go first
                int idx = 0;
                for (int i = 0; i < queryVariables.Length; i++)
                {
                    var[idx] = queryVariables[i];
                    varIdxs.Add(var[idx], idx);
                    idx++;
                }
                // initial evidence variables go next
                for (int i = 0; i < e.Length; i++)
                {
                    var[idx] = e[i].getTermVariable();
                    varIdxs.Add(var[idx], idx);
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
                        varIdxs.Add(var[idx], idx);
                        idx++;
                    }
                }
            }

            public void setExtendedValue(RandomVariable rv, T value)
            {
                int idx = varIdxs[rv];
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
                return varIdxs[rv] <= extendedIdx;
            }

            public double posteriorForParents(RandomVariable rv)
            {
                Node<T> n = bn.getNode(rv);
                if (!(n is FiniteNode<T>))
                {
                    throw new ArgumentException("Enumeration-Ask only works with finite Nodes.");
                }
                FiniteNode<T> fn = (FiniteNode<T>)n;
                T[] vals = new T[1 + fn.getParents().Count];
                int idx = 0;
                foreach (Node<T> pn in n.getParents())
                {
                    vals[idx] = extendedValues[varIdxs[pn.getRandomVariable()]];
                    idx++;
                }
                vals[idx] = extendedValues[varIdxs[rv]];

                return fn.getCPT().getValue(vals);
            }
        }

        //
        // PRIVATE METHODS
        //
    }
}
