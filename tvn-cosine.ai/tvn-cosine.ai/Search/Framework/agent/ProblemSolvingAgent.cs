using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.qsearch;

namespace tvn.cosine.ai.search.framework.agent
{
    /// <summary>
    /// Modified copy of class
    /// SimpleProblemSolvingAgent which can be used for
    /// online search, too. Here, attribute #plan (original:
    /// seq ) is protected. Static pseudo code variable state is used in
    /// a more general sense including world state as well as agent state aspects.
    /// This allows the agent to change the plan, if unexpected percepts are
    /// observed. In the concrete java code, state corresponds with the agent
    /// instance itself (this).
    /// </summary>
    /// <typeparam name="S">The type used to represent states</typeparam>
    /// <typeparam name="A">The type of the actions to be used to navigate through the state space</typeparam>
    public abstract class ProblemSolvingAgent<S, A> : AbstractAgent
        where A : IAction
    {
        /// <summary>
        /// Plan, an action sequence, initially empty.
        /// </summary>
        protected IQueue<A> plan = new FifoQueue<A>();

        /// <summary>
        /// Template method, which corresponds to pseudo code function PROBLEM-SOLVING-AGENT(percept) .
        /// </summary>
        /// <param name="p"></param>
        /// <returns>an action</returns>
        public override IAction Execute(IPercept p)
        {
            IAction action = DynamicAction.NO_OP;
            // state <- UPDATE-STATE(state, percept)
            UpdateState(p);
            // if plan is empty then do
            while (plan.Count == 0)
            {
                // state.goal <- FORMULATE-GOAL(state)
                var goal = FormulateGoal();
                if (null != goal)
                {
                    // problem <- FORMULATE-PROBLEM(state, goal)
                    IProblem<S, A> problem = FormulateProblem(goal);
                    // state.plan <- SEARCH(problem)
                    IList<A> actions = Search(problem);
                    if (null != actions)
                        foreach (var v in actions)
                            plan.Add(v);
                    else if (!TryWithAnotherGoal())
                    {
                        // unable to identify a path
                        SetAlive(false);
                        break;
                    }
                }
                else
                {
                    // no further goal to achieve
                    SetAlive(false);
                    break;
                }
            }
            if (!(plan.Count == 0))
            {
                // action <- FIRST(plan)
                // plan <- REST(plan)
                action = plan.remove();
            }
            return action;
        }

        /// <summary> 
        /// Primitive operation, which decides after a search for a plan failed,
        /// whether to stop the whole task with a failure, or to go on with
        /// formulating another goal. This implementation always returns false. If
        /// the agent defines local goals to reach an externally specified global
        /// goal, it might be interesting, not to stop when the first local goal
        /// turns out to be unreachable.
        /// </summary>
        /// <returns></returns>
        protected bool TryWithAnotherGoal()
        {
            return false;
        }

        /// <summary> 
        /// Primitive operation, responsible for updating the state of the agent with
        /// respect to latest feedback from the world. In this version,
        /// implementations have access to the agent's current goal and plan, so they
        /// can modify them if needed. For example, if the plan didn't work because
        /// the model of the world proved to be wrong, implementations could update
        /// the model and also clear the plan.
        /// </summary>
        /// <param name="p"></param>
        protected abstract void UpdateState(IPercept p);

        /// <summary>
        /// Primitive operation, responsible for goal generation. In this version,
        /// implementations are allowed to return empty to indicate that the agent has
        /// finished the job an should die. Implementations can access the current
        /// goal (which is a possibly modified version of the last formulated goal).
        /// This might be useful in situations in which plan execution has failed.
        /// </summary>
        /// <returns></returns>
        protected abstract S FormulateGoal();

        /// <summary>
        /// Primitive operation, responsible for search problem generation.
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        protected abstract IProblem<S, A> FormulateProblem(S goal);

        /// <summary>
        /// Primitive operation, responsible for the generation of an action list (plan) for the given search problem.
        /// </summary>
        /// <param name="problem"></param>
        /// <returns></returns>
        protected abstract IList<A> Search(IProblem<S, A> problem);
    }
}
