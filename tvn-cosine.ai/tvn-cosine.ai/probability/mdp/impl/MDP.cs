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
        private mdp.ActionsFunction<S, A> actionsFunction = null;
        private mdp.TransitionProbabilityFunction<S, A> transitionProbabilityFunction = null;
        private mdp.RewardFunction<S> rewardFunction = null;

        public MDP(ISet<S> states, S initialState,
                mdp.ActionsFunction<S, A> actionsFunction,
                mdp.TransitionProbabilityFunction<S, A> transitionProbabilityFunction,
                mdp.RewardFunction<S> rewardFunction)
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
            return actionsFunction.actions(s);
        }

        public double transitionProbability(S sDelta, S s, A a)
        {
            return transitionProbabilityFunction.probability(sDelta, s, a);
        }

        public double reward(S s)
        {
            return rewardFunction.reward(s);
        }

        // END-MarkovDecisionProcess
        //
    }
}
