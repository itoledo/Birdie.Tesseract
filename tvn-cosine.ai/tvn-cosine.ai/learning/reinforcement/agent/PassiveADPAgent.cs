using System.Collections.Generic;
using System.Collections.ObjectModel;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.probability.mdp;
using tvn.cosine.ai.probability.mdp.impl;
using tvn.cosine.ai.util;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.learning.reinforcement.agent
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 834.<br>
     * <br>
     * 
     * <pre>
     * function PASSIVE-ADP-AGENT(percept) returns an action
     *   inputs: percept, a percept indicating the current state s' and reward signal r'
     *   persistent: &pi;, a fixed policy
     *               mdp, an MDP with model P, rewards R, discount &gamma;
     *               U, a table of utilities, initially empty
     *               N<sub>sa</sub>, a table of frequencies for state-action pairs, initially zero
     *               N<sub>s'|sa</sub>, a table of outcome frequencies give state-action pairs, initially zero
     *               s, a, the previous state and action, initially null
     *               
     *   if s' is new then U[s'] <- r'; R[s'] <- r'
     *   if s is not null then
     *        increment N<sub>sa</sub>[s,a] and N<sub>s'|sa</sub>[s',s,a]
     *        for each t such that N<sub>s'|sa</sub>[t,s,a] is nonzero do
     *            P(t|s,a) <-  N<sub>s'|sa</sub>[t,s,a] / N<sub>sa</sub>[s,a]
     *   U <- POLICY-EVALUATION(&pi;, U, mdp)
     *   if s'.TERMINAL? then s,a <- null else s,a <- s',&pi;[s']
     *   return a
     * </pre>
     * 
     * Figure 21.2 A passive reinforcement learning agent based on adaptive dynamic
     * programming. The POLICY-EVALUATION function solves the fixed-policy Bellman
     * equations, as described on page 657.
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
    public class PassiveADPAgent<S, A> : ReinforcementAgent<S, A>
        where A : IAction
    {
        // persistent: &pi;, a fixed policy
        private IDictionary<S, A> pi = new Dictionary<S, A>();
        // mdp, an MDP with model P, rewards R, discount &gamma;
        private MDP<S, A> mdp = null;
        private IDictionary<Pair<S, Pair<S, A>>, double> P = new Dictionary<Pair<S, Pair<S, A>>, double>();
        private IDictionary<S, double> R = new Dictionary<S, double>();
        private PolicyEvaluation<S, A> policyEvaluation = null;
        // U, a table of utilities, initially empty
        private IDictionary<S, double> U = new Dictionary<S, double>();
        // N<sub>sa</sub>, a table of frequencies for state-action pairs, initially
        // zero
        private FrequencyCounter<Pair<S, A>> Nsa = new FrequencyCounter<Pair<S, A>>();
        // N<sub>s'|sa</sub>, a table of outcome frequencies give state-action
        // pairs, initially zero
        private FrequencyCounter<Pair<S, Pair<S, A>>> NsDelta_sa = new FrequencyCounter<Pair<S, Pair<S, A>>>();
        // s, a, the previous state and action, initially null
        private S s = default(S);
        private A a = default(A);

        /**
         * Constructor.
         * 
         * @param fixedPolicy
         *            &pi; a fixed policy.
         * @param states
         *            the possible states in the world (i.e. fully observable).
         * @param initialState
         *            the initial state for the agent.
         * @param actionsFunction
         *            a function that lists the legal actions from a state.
         * @param policyEvaluation
         *            a function for evaluating a policy.
         */
        public PassiveADPAgent(IDictionary<S, A> fixedPolicy, ISet<S> states,
                S initialState, ActionsFunction<S, A> actionsFunction,
                PolicyEvaluation<S, A> policyEvaluation)
        {
            foreach (var v in fixedPolicy)
                this.pi.Add(v);

            this.mdp = new MDP<S, A>(states, initialState, actionsFunction,
                    (sDelta, s, a) =>
                    {
                        var key = new Pair<S, Pair<S, A>>(sDelta, new Pair<S, A>(s, a));
                        if (P.ContainsKey(key))
                            return P[key];
                        else return 0D;
                    },
                    (s) =>
                    {
                        return R[s];
                    });

            this.policyEvaluation = policyEvaluation;

        }

        /**
         * Passive reinforcement learning based on adaptive dynamic programming.
         * 
         * @param percept
         *            a percept indicating the current state s' and reward signal
         *            r'.
         * @return an action
         */
        public override A execute(PerceptStateReward<S> percept)
        {
            // if s' is new then U[s'] <- r'; R[s'] <- r'
            S sDelta = percept.state();
            double rDelta = percept.reward();
            if (!U.ContainsKey(sDelta))
            {
                U[sDelta] = rDelta;
                R[sDelta] = rDelta;
            }
            // if s is not null then
            if (null != s)
            {
                // increment N<sub>sa</sub>[s,a] and N<sub>s'|sa</sub>[s',s,a]
                Pair<S, A> sa = new Pair<S, A>(s, a);
                Nsa.incrementFor(sa);
                NsDelta_sa.incrementFor(new Pair<S, Pair<S, A>>(sDelta, sa));
                // for each t such that N<sub>s'|sa</sub>[t,s,a] is nonzero do
                foreach (S t in mdp.states())
                {
                    Pair<S, Pair<S, A>> t_sa = new Pair<S, Pair<S, A>>(t, sa);
                    if (0 != NsDelta_sa.getCount(t_sa))
                    {
                        // P(t|s,a) <- N<sub>s'|sa</sub>[t,s,a] /
                        // N<sub>sa</sub>[s,a]
                        P[t_sa] = (double)NsDelta_sa.getCount(t_sa) / (double)Nsa.getCount(sa);
                    }
                }
            }
            // U <- POLICY-EVALUATION(&pi;, U, mdp)
            U = policyEvaluation.evaluate(pi, U, mdp);
            // if s'.TERMINAL? then s,a <- null else s,a <- s',&pi;[s']
            if (isTerminal(sDelta))
            {
                s = default(S);
                a = default(A);

            }
            else
            {
                s = sDelta;
                a = pi[sDelta];
            }
            // return a
            return a;
        }

        public override IDictionary<S, double> getUtility()
        {
            return new ReadOnlyDictionary<S, double>(U);
        }

        public override void reset()
        {
            P.Clear();
            R.Clear();
            U = new Dictionary<S, double>();
            Nsa.clear();
            NsDelta_sa.clear();
            s = default(S);
            a = default(A);
        }

        //
        // PRIVATE METHODS
        //
        private bool isTerminal(S s)
        {
            bool terminal = false;
            if (0 == mdp.actions(s).Count)
            {
                // No actions possible in state is considered terminal.
                terminal = true;
            }
            return terminal;
        }
    } 
}
