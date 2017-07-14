using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.probability.mdp.search
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 653. 
     *  
     * 
     * <pre>
     * function VALUE-ITERATION(mdp, &epsilon;) returns a utility function
     *   inputs: mdp, an MDP with states S, actions A(s), transition model P(s' | s, a),
     *             rewards R(s), discount &gamma;
     *           &epsilon; the maximum error allowed in the utility of any state
     *   local variables: U, U', vectors of utilities for states in S, initially zero
     *                    &delta; the maximum change in the utility of any state in an iteration
     *                    
     *   repeat
     *       U <- U'; &delta; <- 0
     *       for each state s in S do
     *           U'[s] <- R(s) + &gamma;  max<sub>a &isin; A(s)</sub> &Sigma;<sub>s'</sub>P(s' | s, a) U[s']
     *           if |U'[s] - U[s]| > &delta; then &delta; <- |U'[s] - U[s]|
     *   until &delta; < &epsilon;(1 - &gamma;)/&gamma;
     *   return U
     * </pre>
     * 
     * Figure 17.4 The value iteration algorithm for calculating utilities of
     * states. The termination condition is from Equation (17.8): 
     * 
     * <pre>
     * if ||U<sub>i+1</sub> - U<sub>i</sub>|| < &epsilon;(1 - &gamma;)/&gamma; then ||U<sub>i+1</sub> - U|| < &epsilon;
     * </pre>
     * 
     * @param <S>
     *            the state type.
     * @param <A>
     *            the action type.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     * 
     */
    public class ValueIteration<S, A>
        where A : Action
    {
        // discount &gamma; to be used.
        private double gamma = 0;

        /**
         * Constructor.
         * 
         * @param gamma
         *            discount &gamma; to be used.
         */
        public ValueIteration(double gamma)
        {
            if (gamma > 1.0 || gamma <= 0.0)
            {
                throw new System.ArgumentException("Gamma must be > 0 and <= 1.0");
            }
            this.gamma = gamma;
        }

        // function VALUE-ITERATION(mdp, &epsilon;) returns a utility function
        /**
         * The value iteration algorithm for calculating the utility of states.
         * 
         * @param mdp
         *            an MDP with states S, actions A(s),  
         *            transition model P(s' | s, a), rewards R(s)
         * @param epsilon
         *            the maximum error allowed in the utility of any state
         * @return a vector of utilities for states in S
         */
        public IDictionary<S, double> valueIteration(MarkovDecisionProcess<S, A> mdp, double epsilon)
        {
            //
            // local variables: U, U', vectors of utilities for states in S,
            // initially zero
            IDictionary<S, double> U = Util.create(mdp.states(), 0D);
            IDictionary<S, double> Udelta = Util.create(mdp.states(), 0D);
            // &delta; the maximum change in the utility of any state in an
            // iteration
            double delta = 0;
            // Note: Just calculate this once for efficiency purposes:
            // &epsilon;(1 - &gamma;)/&gamma;
            double minDelta = epsilon * (1 - gamma) / gamma;

            // repeat
            do
            {
                // U <- U'; &delta; <- 0
                foreach (var v in Udelta)
                    U.Add(v);
                delta = 0;
                // for each state s in S do
                foreach (S s in mdp.states())
                {
                    // max<sub>a &isin; A(s)</sub>
                    ISet<A> actions = mdp.actions(s);
                    // Handle terminal states (i.e. no actions).
                    double aMax = 0;
                    if (actions.Count > 0)
                    {
                        aMax = double.NegativeInfinity;
                    }
                    foreach (A a in actions)
                    {
                        // &Sigma;<sub>s'</sub>P(s' | s, a) U[s']
                        double aSum = 0;
                        foreach (S sDelta in mdp.states())
                        {
                            aSum += mdp.transitionProbability(sDelta, s, a) * U[sDelta];
                        }
                        if (aSum > aMax)
                        {
                            aMax = aSum;
                        }
                    }
                    // U'[s] <- R(s) + &gamma;
                    // max<sub>a &isin; A(s)</sub>
                    Udelta.Add(s, mdp.reward(s) + gamma * aMax);
                    // if |U'[s] - U[s]| > &delta; then &delta; <- |U'[s] - U[s]|
                    double aDiff = System.Math.Abs(Udelta[s] - U[s]);
                    if (aDiff > delta)
                    {
                        delta = aDiff;
                    }
                }
                // until &delta; < &epsilon;(1 - &gamma;)/&gamma;
            } while (delta > minDelta);

            // return U
            return U;
        }
    } 
}
