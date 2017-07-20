using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.probability.mdp.impl
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 657.<br>
     * <br>
     * For small state spaces, policy evaluation using exact solution methods is
     * often the most efficient approach. For large state spaces, O(n<sup>3</sup>)
     * time might be prohibitive. Fortunately, it is not necessary to do exact
     * policy evaluation. Instead, we can perform some number of simplified value
     * iteration steps (simplified because the policy is fixed) to give a reasonably
     * good approximation of utilities. The simplified Bellman update for this
     * process is:<br>
     * <br>
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
        where A : IAction
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
                throw new IllegalArgumentException("Gamma must be > 0 and <= 1.0");
            }
            this.k = k;
            this.gamma = gamma;
        }
         
        public IMap<S, double> evaluate(IMap<S, A> pi_i, IMap<S, double> U, MarkovDecisionProcess<S, A> mdp)
        {
            IMap<S, double> U_i = Factory.CreateMap<S, double>(U);
            IMap<S, double> U_ip1 = Factory.CreateMap<S, double>(U);
            // repeat k times to produce the next utility estimate
            for (int i = 0; i < k;++i)
            {
                // U<sub>i+1</sub>(s) <- R(s) +
                // &gamma;&Sigma;<sub>s'</sub>P(s'|s,&pi;<sub>i</sub>(s))U<sub>i</sub>(s')
                foreach (S s in U.GetKeys())
                {
                    A ap_i = pi_i.Get(s);
                    double aSum = 0;
                    // Handle terminal states (i.e. no actions)
                    if (null != ap_i)
                    {
                        foreach (S sDelta in U.GetKeys())
                        {
                            aSum += mdp.transitionProbability(sDelta, s, ap_i) * U_i.Get(sDelta);
                        }
                    }
                    U_ip1.Put(s, mdp.reward(s) + gamma * aSum);
                }

                U_i.PutAll(U_ip1);
            }
            return U_ip1;
        } 
    } 
}
