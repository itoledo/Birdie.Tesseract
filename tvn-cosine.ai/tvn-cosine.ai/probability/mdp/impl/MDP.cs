using System.Collections.Generic;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.probability.mdp.impl
{
    /**
     * Default implementation of the MarkovDecisionProcess<S, A> interface.
     * 
     * @param <S>
     *            the state type.
     * @param <A>
     *            the action type.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class MDP<S, A> : MarkovDecisionProcess<S, A>
        where A : Action
    {

        private ISet<S> _states;
        private S initialState;
        private ActionsFunction<S, A> actionsFunction = null;
        private TransitionProbabilityFunction<S, A> transitionProbabilityFunction = null;
        private RewardFunction<S> rewardFunction = null;

        public MDP(ISet<S> states, S initialState,
               ActionsFunction<S, A> actionsFunction,
               TransitionProbabilityFunction<S, A> transitionProbabilityFunction,
               RewardFunction<S> rewardFunction)
        {
            this._states = states;
            this.initialState = initialState;
            this.actionsFunction = actionsFunction;
            this.transitionProbabilityFunction = transitionProbabilityFunction;
            this.rewardFunction = rewardFunction;
        }

        //
        // START-MarkovDecisionProcess 
        public ISet<S> states()
        {
            return _states;
        }

        public S getInitialState()
        {
            return initialState;
        }

        public ISet<A> actions(S s)
        {
            return actionsFunction(s);
        }

        public double transitionProbability(S sDelta, S s, A a)
        {
            return transitionProbabilityFunction(sDelta, s, a);
        }

        public double reward(S s)
        {
            return rewardFunction(s);
        }

        // END-MarkovDecisionProcess
        //
    }
}
