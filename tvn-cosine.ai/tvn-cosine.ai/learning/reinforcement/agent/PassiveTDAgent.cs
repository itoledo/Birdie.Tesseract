using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.learning.reinforcement.agent
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 837.<br>
     * <br>
     * 
     * <pre>
     * function PASSIVE-TD-AGENT(percept) returns an action
     *   inputs: percept, a percept indicating the current state s' and reward signal r'
     *   persistent: &pi;, a fixed policy
     *               U, a table of utilities, initially empty
     *               N<sub>s</sub>, a table of frequencies for states, initially zero
     *               s,a,r, the previous state, action, and reward, initially null
     *               
     *   if s' is new then U[s'] <- r'
     *   if s is not null then
     *        increment N<sub>s</sub>[s]
     *        U[s] <- U[s] + &alpha;(N<sub>s</sub>[s])(r + &gamma;U[s'] - U[s])
     *   if s'.TERMINAL? then s,a,r <- null else s,a,r <- s',&pi;[s'],r'
     *   return a
     * </pre>
     * 
     * Figure 21.4 A passive reinforcement learning agent that learns utility
     * estimates using temporal differences. The step-size function &alpha;(n) is
     * chosen to ensure convergence, as described in the text.
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
    public class PassiveTDAgent<S, A> : ReinforcementAgent<S, A>
        where A : IAction
    {
        // persistent: &pi;, a fixed policy
        private IDictionary<S, A> pi = new Dictionary<S, A>();
        // U, a table of utilities, initially empty
        private IDictionary<S, double> U = new Dictionary<S, double>();
        // N<sub>s</sub>, a table of frequencies for states, initially zero
        private FrequencyCounter<S> Ns = new FrequencyCounter<S>();
        // s,a,r, the previous state, action, and reward, initially null
        private S s = default(S);
        private A a = default(A);
        private double r;
        //
        private double _alpha = 0.0;
        private double gamma = 0.0;

        /**
         * Constructor.
         * 
         * @param fixedPolicy
         *            &pi; a fixed policy.
         * @param alpha
         *            a fixed learning rate.
         * @param gamma
         *            discount to be used.
         */
        public PassiveTDAgent(IDictionary<S, A> fixedPolicy, double alpha, double gamma)
        {
            foreach (var v in fixedPolicy)
                this.pi.Add(v);
            this._alpha = alpha;
            this.gamma = gamma;
        }

        /**
         * Passive reinforcement learning that learns utility estimates using
         * temporal differences
         * 
         * @param percept
         *            a percept indicating the current state s' and reward signal
         *            r'.
         * @return an action
         */
        public override A execute(PerceptStateReward<S> percept)
        {
            // if s' is new then U[s'] <- r'
            S sDelta = percept.state();
            double rDelta = percept.reward();
            if (!U.ContainsKey(sDelta))
            {
                U[sDelta] = rDelta;
            }
            // if s is not null then
            if (null != s)
            {
                // increment N<sub>s</sub>[s]
                Ns.incrementFor(s);
                // U[s] <- U[s] + &alpha;(N<sub>s</sub>[s])(r + &gamma;U[s'] - U[s])
                double U_s = U[s];
                U[s] = U_s + alpha(Ns, s) * (r + gamma * U[sDelta] - U_s);
            }
            // if s'.TERMINAL? then s,a,r <- null else s,a,r <- s',&pi;[s'],r'
            if (isTerminal(sDelta))
            {
                s = default(S);
                a = default(A);
                r = 0;
            }
            else
            {
                s = sDelta;
                a = pi[sDelta];
                r = rDelta;
            }

            // return a
            return a;
        }

        public override IDictionary<S, double> getUtility()
        {
            return new Dictionary<S, double>(U);
        }

        public override void reset()
        {
            U = new Dictionary<S, double>();
            Ns.clear();
            s = default(S);
            a = default(A);
            r = 0;
        }

        //
        // PROTECTED METHODS
        //
        /**
         * AIMA3e pg. 836 'if we change &alpha; from a fixed parameter to a function
         * that decreases as the number of times a state has been visited increases,
         * then U<sup>&pi;</sup>(s) itself will converge to the correct value.<br>
         * <br>
         * <b>Note:</b> override this method to obtain the desired behavior.
         * 
         * @param Ns
         *            a frequency counter of observed states.
         * @param s
         *            the current state.
         * @return the learning rate to use based on the frequency of the state
         *         passed in.
         */
        protected double alpha(FrequencyCounter<S> Ns, S s)
        {
            // Default implementation is just to return a fixed parameter value
            // irrespective of the # of times a state has been encountered
            return _alpha;
        }

        //
        // PRIVATE METHODS
        //
        private bool isTerminal(S s)
        {
            bool terminal = false;
            IAction a = pi[s];
            if (null == a || a.IsNoOp())
            {
                // No actions possible in state is considered terminal.
                terminal = true;
            }
            return terminal;
        }
    }

}
