using System.Collections.Generic;

namespace tvn.cosine.ai.search.framework.problem
{ 
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 147. <para />
    ///  
    /// An online search problem must be solved by an agent executing actions, rather
    /// than by pure computation. We assume a deterministic and fully observable
    /// environment (Chapter 17 relaxes these assumptions), but we stipulate that the
    /// agent knows only the following:  <para />
    ///  
    /// * ACTIONS(s), which returns a list of actions allowed in state s;<para />
    /// * The step-cost function c(s, a, s') - note that this cannot be used until the agent knows that s' is the outcome; and <para />
    /// * GOAL-TEST(s).<para />
    /// </summary>
    /// <typeparam name="S">The type used to represent states</typeparam>
    /// <typeparam name="A">The type of the actions to be used to navigate through the state space</typeparam>
    public interface IOnlineSearchProblem<S, A>
    {
        /// <summary>
        /// Returns the initial state of the agent.
        /// </summary>
        /// <returns>the initial state of the agent.</returns>
        S GetInitialState();

        /// <summary>
        /// Returns the description of the possible actions available to the agent.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>the description of the possible actions available to the agent.</returns>
        IList<A> GetActions(S state);

        /// <summary>
        /// Determines whether a given state is a goal state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>whether a given state is a goal state.</returns>
        bool TestGoal(S state);

        /// <summary>
        /// Returns the step cost of taking action action in state state to reach state stateDelta denoted by c(s, a, s').
        /// </summary>
        /// <param name="state"></param>
        /// <param name="action"></param>
        /// <param name="stateDelta"></param>
        /// <returns>the step cost of taking action action in state state to reach state stateDelta denoted by c(s, a, s').</returns>
        double GetStepCosts(S state, A action, S stateDelta);
    }
}
