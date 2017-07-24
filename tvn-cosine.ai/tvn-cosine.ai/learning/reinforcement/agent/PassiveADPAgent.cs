using tvn.cosine.ai.agent.api;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.datastructures;
using tvn.cosine.ai.probability.mdp;
using tvn.cosine.ai.probability.mdp.impl;
using tvn.cosine.ai.util;

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
        private IMap<S, A> pi = Factory.CreateInsertionOrderedMap<S, A>();
        // mdp, an MDP with model P, rewards R, discount &gamma;
        private MDP<S, A> mdp = null;
        private IMap<Pair<S, Pair<S, A>>, double> P = Factory.CreateInsertionOrderedMap<Pair<S, Pair<S, A>>, double>();
        private IMap<S, double> R = Factory.CreateInsertionOrderedMap<S, double>();
        private PolicyEvaluation<S, A> policyEvaluation = null;
        // U, a table of utilities, initially empty
        private IMap<S, double> U = Factory.CreateInsertionOrderedMap<S, double>();
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

        class RewardFunctionImpl : RewardFunction<S>
        {
            private IMap<S, double> R;

            public RewardFunctionImpl(IMap<S, double> r)
            {
                this.R = r;
            }

            public double reward(S s)
            {
                return R.Get(s);
            }
        }

        class TransitionProbabilityFunctionImpl : TransitionProbabilityFunction<S, A>
        {
            private IMap<Pair<S, Pair<S, A>>, double> P;

            public TransitionProbabilityFunctionImpl(IMap<Pair<S, Pair<S, A>>, double> p)
            {
                this.P = p;
            }

            public double probability(S sDelta, S s, A a)
            {
                double p = P.Get(new Pair<S, Pair<S, A>>(sDelta, new Pair<S, A>(s, a)));

                return p;
            }
        }

        public PassiveADPAgent(IMap<S, A> fixedPolicy, ISet<S> states,
                S initialState, ActionsFunction<S, A> actionsFunction,
                PolicyEvaluation<S, A> policyEvaluation)
        {
            this.pi.AddAll(fixedPolicy);
            this.mdp = new MDP<S, A>(states, initialState, actionsFunction,
                    new TransitionProbabilityFunctionImpl(P),
                    new RewardFunctionImpl(R));

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
                U.Put(sDelta, rDelta);
                R.Put(sDelta, rDelta);
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
                        P.Put(t_sa, (double)NsDelta_sa.getCount(t_sa)
                                  / (double)Nsa.getCount(sa));
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
                a = pi.Get(sDelta);
            }
            // return a
            return a;
        }


        public override IMap<S, double> getUtility()
        {
            return Factory.CreateReadOnlyMap<S, double>(U);
        }


        public override void reset()
        {
            P.Clear();
            R.Clear();
            U = Factory.CreateInsertionOrderedMap<S, double>();
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
            if (0 == mdp.actions(s).Size())
            {
                // No actions possible in state is considered terminal.
                terminal = true;
            }
            return terminal;
        }
    }

}
