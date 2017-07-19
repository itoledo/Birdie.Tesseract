using tvn.cosine.ai.agent;
using tvn.cosine.ai.common.collections;

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
        private ISet<S> _states ;
        private S initialState ;
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

        public virtual ISet<S> states()
        {
            return _states;
        }

        public virtual S getInitialState()
        {
            return initialState;
        }

        public virtual ISet<A> actions(S s)
        {
            return actionsFunction.actions(s);
        }


        public virtual double transitionProbability(S sDelta, S s, A a)
        {
            return transitionProbabilityFunction.probability(sDelta, s, a);
        }


        public virtual double reward(S s)
        {
            return rewardFunction.reward(s);
        } 
    } 
}
