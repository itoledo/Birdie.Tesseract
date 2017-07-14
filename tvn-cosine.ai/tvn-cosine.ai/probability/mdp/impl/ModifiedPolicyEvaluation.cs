
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.probability.mdp.impl
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 657. 
     *  
     * For small state spaces, policy evaluation using exact solution methods is
     * often the most efficient approach. For large state spaces, O(n<sup>3</sup>)
     * time might be prohibitive. Fortunately, it is not necessary to do exact
     * policy evaluation. Instead, we can perform some number of simplified value
     * iteration steps (simplified because the policy is fixed) to give a reasonably
     * good approximation of utilities. The simplified Bellman update for this
     * process is: 
     *  
     * 
     * <pre>
     * U<sub>i+1</sub>(s) <- R(s) + &gamma;&Sigma;<sub>s'</sub>P(s'|s,&pi;<sub>i</sub>(s))U<sub>i</sub>(s')
     * </pre>
     * 
     * and this is repeated k times to produce the next utility estimate. The
     * resulting algorithm is called <b>modified policy iteration</b>. It is often
     * much more efficient than standard policy iteration or value iteration.
     * 
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
    public class ModifiedPolicyEvaluation<S, A> : PolicyEvaluation<S, A>
        where A : Action
    {
        // # iterations to use to produce the next utility estimate
        private int k;
        // discount &gamma; to be used.
        private double gamma;

        /**
         * Constructor.
         * 
         * @param k
         *            number iterations to use to produce the next utility estimate
         * @param gamma
         *            discount &gamma; to be used
         */
        public ModifiedPolicyEvaluation(int k, double gamma)
        {
            if (gamma > 1.0 || gamma <= 0.0)
            {
                throw new System.ArgumentException("Gamma must be > 0 and <= 1.0");
            }
            this.k = k;
            this.gamma = gamma;
        }

        //
        // START-PolicyEvaluation 
        public IDictionary<S, double> evaluate(IDictionary<S, A> pi_i, IDictionary<S, double> U, MarkovDecisionProcess<S, A> mdp)
        {
            IDictionary<S, double> U_i = new Dictionary<S, double>(U);
            IDictionary<S, double> U_ip1 = new Dictionary<S, double>(U);
            // repeat k times to produce the next utility estimate
            for (int i = 0; i < k; ++i)
            {
                // U<sub>i+1</sub>(s) <- R(s) +
                // &gamma;&Sigma;<sub>s'</sub>P(s'|s,&pi;<sub>i</sub>(s))U<sub>i</sub>(s')
                foreach (S s in U.Keys)
                {
                    double aSum = 0;
                    // Handle terminal states (i.e. no actions)
                    if (pi_i.ContainsKey(s))
                    {
                        foreach (S sDelta in U.Keys)
                        {
                            aSum += mdp.transitionProbability(sDelta, s, pi_i[s]) * U_i[sDelta];
                        }
                    }
                    U_ip1[s] = mdp.reward(s) + gamma * aSum;
                }

                foreach (var v in U_ip1)
                { 
                    U_i[v.Key] = v.Value; 
                }
            }
            return U_ip1;
        }

        // END-PolicyEvaluation
        //
    }
}
