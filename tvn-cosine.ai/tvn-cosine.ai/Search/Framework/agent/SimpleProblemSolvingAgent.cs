using System.Collections.Generic;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.qsearch;

namespace tvn.cosine.ai.search.framework.agent
{ 
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): Figure 3.1, page 67. <para />
    ///    
    /// Figure 3.1 A simple problem-solving agent. It first formulates a goal and a
    /// problem, searches for a sequence of actions that would solve the problem, and
    /// then executes the actions one at a time. When this is complete, it formulates
    /// another goal and starts over. 
    /// </summary>
    /// <typeparam name="S">The type used to represent states</typeparam>
    /// <typeparam name="A">The type of the actions to be used to navigate through the state space</typeparam>
    public abstract class SimpleProblemSolvingAgent<S, A> : AbstractAgent
        where A : Action
    {
        /// <summary>
        /// an action sequence, initially empty
        /// </summary>
        private IQueue<A> seq = new FifoQueue<A>();
         
        private bool formulateGoalsIndefinitely = true; 
        private int maxGoalsToFormulate = 1; 
        private int goalsFormulated = 0;

        /// <summary>
        /// Constructs a simple problem solving agent which will formulate goals indefinitely.
        /// </summary>
        public SimpleProblemSolvingAgent()
        {
            formulateGoalsIndefinitely = true;
        }
     
        /// <summary>
        /// Constructs a simple problem solving agent which will formulate, at maximum, the specified number of goals.
        /// </summary>
        /// <param name="maxGoalsToFormulate">the maximum number of goals this agent is to formulate.</param>
        public SimpleProblemSolvingAgent(int maxGoalsToFormulate)
        {
            formulateGoalsIndefinitely = false;
            this.maxGoalsToFormulate = maxGoalsToFormulate;
        }

        /// <summary>
        /// SIMPLE-PROBLEM-SOLVING-AGENT(percept) 
        /// </summary>
        /// <param name="p"></param>
        /// <returns>an action </returns>
        public override Action execute(Percept p)
        {
            // return value if at goal or goal not found
            Action action = DynamicAction.NO_OP; 

            // state <- UPDATE-STATE(state, percept)
            UpdateState(p);
            // if seq is empty then do
            if (seq.isEmpty())
            {
                if (formulateGoalsIndefinitely || goalsFormulated < maxGoalsToFormulate)
                {
                    if (goalsFormulated > 0)
                    {
                        NotifyViewOfMetrics();
                    }
                    // goal <- FORMULATE-GOAL(state)
                    S goal = FormulateGoal();
                    goalsFormulated++;
                    // problem <- FORMULATE-PROBLEM(state, goal)
                    IProblem<S, A> problem = FormulateProblem(goal);
                    // seq <- SEARCH(problem)
                    IList<A> actions = Search(problem);
                    if (null != actions)
                        foreach (var v in actions)
                            seq.Add(v);
                }
                else
                {
                    // Agent no longer wishes to achieve any more goals
                    setAlive(false);
                    NotifyViewOfMetrics();
                }
            }

            if (seq.Count > 0)
            {
                // action <- FIRST(seq)
                // seq <- REST(seq)
                action = seq.remove();
            }

            return action;
        }
         
        protected abstract void UpdateState(Percept p); 
        protected abstract S FormulateGoal(); 
        protected abstract IProblem<S, A> FormulateProblem(S goal); 
        protected abstract IList<A> Search(IProblem<S, A> problem); 
        protected abstract void NotifyViewOfMetrics();
    }
}
