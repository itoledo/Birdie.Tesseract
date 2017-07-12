using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.probability;
using tvn.cosine.ai.probability.bayes;
using tvn.cosine.ai.probability.proposition;
using tvn.cosine.ai.probability.util;

namespace tvn.cosine.ai.probability.bayes.exact
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 14.11, page
     * 528. 
     *  
     * 
     * <pre>
     * function ELIMINATION-ASK(X, e, bn) returns a distribution over X
     *   inputs: X, the query variable
     *           e, observed values for variables E
     *           bn, a Bayesian network specifying joint distribution P(X<sub>1</sub>, ..., X<sub>n</sub>)
     *   
     *   factors <- []
     *   for each var in ORDER(bn.VARS) do
     *       factors <- [MAKE-FACTOR(var, e) | factors]
     *       if var is hidden variable the factors <- SUM-OUT(var, factors)
     *   return NORMALIZE(POINTWISE-PRODUCT(factors))
     * </pre>
     * 
     * Figure 14.11 The variable elimination algorithm for inference in Bayesian
     * networks.  
     *  
     * <b>Note:</b> The implementation has been extended to handle queries with
     * multiple variables.  
     * 
     * @author Ciaran O'Reilly
     */
    public class EliminationAsk<T> : BayesInference<T>
    {
        //
        private static readonly ProbabilityTable<T> _identity = new ProbabilityTable<T>(new double[] { 1.0 }, new RandomVariable[] { });

        public EliminationAsk()
        {

        }

        // function ELIMINATION-ASK(X, e, bn) returns a distribution over X
        /**
         * The ELIMINATION-ASK algorithm in Figure 14.11.
         * 
         * @param X
         *            the query variables.
         * @param e
         *            observed values for variables E.
         * @param bn
         *            a Bayes net with variables {X} &cup; E &cup; Y /* Y = hidden
         *            variables //
         * @return a distribution over the query variables.
         */
        public CategoricalDistribution<T> eliminationAsk(RandomVariable[] X, AssignmentProposition<T>[] e, BayesianNetwork<T> bn)
        {

            ISet<RandomVariable> hidden = new HashSet<RandomVariable>();
            IList<RandomVariable> VARS = new List<RandomVariable>();
            calculateVariables(X, e, bn, hidden, VARS);

            // factors <- []
            IList<Factor<T>> factors = new List<Factor<T>>();
            // for each var in ORDER(bn.VARS) do
            foreach (RandomVariable var in order(bn, VARS))
            {
                // factors <- [MAKE-FACTOR(var, e) | factors]
                factors.Insert(0, makeFactor(var, e, bn));
                // if var is hidden variable then factors <- SUM-OUT(var, factors)
                if (hidden.Contains(var))
                {
                    factors = sumOut(var, factors, bn);
                }
            }
            // return NORMALIZE(POINTWISE-PRODUCT(factors))
            Factor<T> product = pointwiseProduct(factors);
            // Note: Want to ensure the order of the product matches the
            // query variables
            return ((ProbabilityTable<T>)product.pointwiseProductPOS(_identity, X)).normalize();
        }

        //
        // START-BayesInference
        public CategoricalDistribution<T> ask(RandomVariable[] X,
                AssignmentProposition<T>[] observedEvidence,
                BayesianNetwork<T> bn)
        {
            return this.eliminationAsk(X, observedEvidence, bn);
        }

        // END-BayesInference
        //

        //
        // PROTECTED METHODS
        //
        /**
         * <b>Note:</b>Override this method for a more efficient implementation as
         * outlined in AIMA3e pgs. 527-28. Calculate the hidden variables from the
         * Bayesian Network. The default implementation does not perform any of
         * these. 
         *  
         * Two calcuations to be performed here in order to optimize iteration over
         * the Bayesian Network: 
         * 1. Calculate the hidden variables to be enumerated over. An optimization
         * (AIMA3e pg. 528) is to remove 'every variable that is not an ancestor of
         * a query variable or evidence variable as it is irrelevant to the query'
         * (i.e. sums to 1). 2. The subset of variables from the Bayesian Network to
         * be retained after irrelevant hidden variables have been removed.
         * 
         * @param X
         *            the query variables.
         * @param e
         *            observed values for variables E.
         * @param bn
         *            a Bayes net with variables {X} &cup; E &cup; Y /* Y = hidden
         *            variables //
         * @param hidden
         *            to be populated with the relevant hidden variables Y.
         * @param bnVARS
         *            to be populated with the subset of the random variables
         *            comprising the Bayesian Network with any irrelevant hidden
         *            variables removed.
         */
        protected void calculateVariables(RandomVariable[] X,
                 AssignmentProposition<T>[] e, BayesianNetwork<T> bn,
                ISet<RandomVariable> hidden, ICollection<RandomVariable> bnVARS)
        {
            foreach (var v in bn.getVariablesInTopologicalOrder())
                bnVARS.Add(v);
            foreach (var v in bnVARS)
                hidden.Add(v);

            foreach (RandomVariable x in X)
            {
                hidden.Remove(x);
            }
            foreach (AssignmentProposition<T> ap in e)
            {
                foreach (var v in ap.getScope())
                    hidden.Remove(v);
            }

            return;
        }

        /**
         * <b>Note:</b>Override this method for a more efficient implementation as
         * outlined in AIMA3e pgs. 527-28. The default implementation does not
         * perform any of these. 
         * 
         * @param bn
         *            the Bayesian Network over which the query is being made. Note,
         *            is necessary to provide this in order to be able to determine
         *            the dependencies between variables.
         * @param vars
         *            a subset of the RandomVariables making up the Bayesian
         *            Network, with any irrelevant hidden variables alreay removed.
         * @return a possibly opimal ordering for the random variables to be
         *         iterated over by the algorithm. For example, one fairly effective
         *         ordering is a greedy one: eliminate whichever variable minimizes
         *         the size of the next factor to be constructed.
         */
        protected IList<RandomVariable> order(BayesianNetwork<T> bn, ICollection<RandomVariable> vars)
        {
            // Note: Trivial Approach:
            // For simplicity just return in the reverse order received,
            // i.e. received will be the default topological order for
            // the Bayesian Network and we want to ensure the network
            // is iterated from bottom up to ensure when hidden variables
            // are come across all the factors dependent on them have
            // been seen so far.
            IList<RandomVariable> order = vars.Reverse().ToList();

            return order;
        }

        //
        // PRIVATE METHODS
        //
        private Factor<T> makeFactor(RandomVariable var, AssignmentProposition<T>[] e, BayesianNetwork<T> bn)
        {
            Node<T> n = bn.getNode(var);
            if (!(n is FiniteNode<T>))
            {
                throw new ArgumentException("Elimination-Ask only works with finite Nodes.");
            }
            FiniteNode<T> fn = (FiniteNode<T>)n;
            List<AssignmentProposition<T>> evidence = new List<AssignmentProposition<T>>();
            foreach (AssignmentProposition<T> ap in e)
            {
                if (fn.getCPT().contains(ap.getTermVariable()))
                {
                    evidence.Add(ap);
                }
            }

            return fn.getCPT().getFactorFor(evidence.ToArray());
        }

        private IList<Factor<T>> sumOut(RandomVariable var, IList<Factor<T>> factors, BayesianNetwork<T> bn)
        {
            IList<Factor<T>> summedOutFactors = new List<Factor<T>>();
            IList<Factor<T>> toMultiply = new List<Factor<T>>();
            foreach (Factor<T> f in factors)
            {
                if (f.contains(var))
                {
                    toMultiply.Add(f);
                }
                else
                {
                    // This factor does not contain the variable
                    // so no need to sum out - see AIMA3e pg. 527.
                    summedOutFactors.Add(f);
                }
            }

            summedOutFactors.Add(pointwiseProduct(toMultiply).sumOut(new[] { var }));

            return summedOutFactors;
        }

        private Factor<T> pointwiseProduct(IList<Factor<T>> factors)
        {
            Factor<T> product = factors[0];
            for (int i = 1; i < factors.Count; i++)
            {
                product = product.pointwiseProduct(factors[i]);
            }

            return product;
        }
    }
}
