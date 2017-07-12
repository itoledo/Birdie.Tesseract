using System;
using System.Collections.Generic; 
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.search.online
{
    /**
  * Artificial Intelligence A Modern Approach (3rd Edition): Figure 4.21, page
  * 150. 
  *  
  * 
  * <pre>
  * function ONLINE-DFS-AGENT(s') returns an action
  *   inputs: s', a percept that identifies the current state
  *   persistent: result, a table, indexed by state and action, initially empty
  *               untried, a table that lists, for each state, the actions not yet tried
  *               unbacktracked, a table that lists, for each state, the backtracks not yet tried
  *               s, a, the previous state and action, initially null
  *    
  *   if GOAL-TEST(s') then return stop
  *   if s' is a new state (not in untried) then untried[s'] &lt;- ACTIONS(s')
  *   if s is not null then
  *       result[s, a] &lt;- s'
  *       add s to the front of the unbacktracked[s']
  *   if untried[s'] is empty then
  *       if unbacktracked[s'] is empty then return stop
  *       else a &lt;- an action b such that result[s', b] = POP(unbacktracked[s'])
  *   else a &lt;- POP(untried[s'])
  *   s &lt;- s'
  *   return a
  * </pre>
  * 
  * Figure 4.21 An online search agent that uses depth-first exploration. The
  * agent is applicable only in state spaces in which every action can be
  * "undone" by some other action. 
  * 
  * @author Ciaran O'Reilly
  * @author Ruediger Lunde
  * 
  */
    public class OnlineDFSAgent<S, A> : AbstractAgent
        where A : agent.IAction
    {
        private IOnlineSearchProblem<S, A> problem;
        private Func<IPercept, S> ptsFn;
        // persistent: result, a table, indexed by state and action, initially empty
        private readonly TwoKeyDictionary<S, A, S> result = new TwoKeyDictionary<S, A, S>();
        // untried, a table that lists, for each state, the actions not yet tried
        private readonly IDictionary<S, IList<A>> untried = new Dictionary<S, IList<A>>();
        // unbacktracked, a table that lists,
        // for each state, the backtracks not yet tried
        private readonly IDictionary<S, IList<S>> unbacktracked = new Dictionary<S, IList<S>>();
        // s, a, the previous state and action, initially null
        private S s = default(S);
        private A a = default(A);

        /**
         * Constructs an online DFS agent with the specified search problem and
         * percept to state function.
         * 
         * @param problem
         *            an online search problem for this agent to solve
         * @param ptsFn
         *            a function which returns the problem state associated with a
         *            given Percept.
         */
        public OnlineDFSAgent(IOnlineSearchProblem<S, A> problem, Func<IPercept, S> ptsFn)
        {
            setProblem(problem);
            setPerceptToStateFunction(ptsFn);
        }

        /**
         * Returns the search problem for this agent.
         * 
         * @return the search problem for this agent.
         */
        public IOnlineSearchProblem<S, A> getProblem()
        {
            return problem;
        }

        /**
         * Sets the search problem for this agent to solve.
         * 
         * @param problem
         *            the search problem for this agent to solve.
         */
        public void setProblem(IOnlineSearchProblem<S, A> problem)
        {
            this.problem = problem;
            init();
        }

        /**
         * Returns the percept to state function of this agent.
         * 
         * @return the percept to state function of this agent.
         */
        public Func<IPercept, S> getPerceptToStateFunction()
        {
            return ptsFn;
        }

        /**
         * Sets the percept to state functino of this agent.
         * 
         * @param ptsFn
         *            a function which returns the problem state associated with a
         *            given Percept.
         */
        public void setPerceptToStateFunction(Func<IPercept, S> ptsFn)
        {
            this.ptsFn = ptsFn;
        }

        // function ONLINE-DFS-AGENT(s') returns an action
        // inputs: s', a percept that identifies the current state

        public override agent.IAction Execute(IPercept psPrimed)
        {
            S sPrimed = ptsFn(psPrimed);
            // if GOAL-TEST(s') then return stop
            if (problem.testGoal(sPrimed))
            {
                a = default(A);
            }
            else
            {
                // if s' is a new state (not in untried) then untried[s'] <-
                // ACTIONS(s')
                if (!untried.ContainsKey(sPrimed))
                {
                    untried.Add(sPrimed, problem.getActions(sPrimed));
                }

                // if s is not null then do
                if (null != s)
                {
                    // Note: If I've already seen the result of this
                    // [s, a] then don't put it back on the unbacktracked
                    // list otherwise you can keep oscillating
                    // between the same states endlessly.
                    if (!(sPrimed.Equals(result[s, a])))
                    {
                        // result[s, a] <- s'
                        result.Add(s, a, sPrimed);

                        // Ensure the unbacktracked always has a list for s'
                        if (!unbacktracked.ContainsKey(sPrimed))
                        {
                            unbacktracked.Add(sPrimed, new List<S>());
                        }

                        // add s to the front of the unbacktracked[s']
                        unbacktracked[sPrimed].Insert(0, s);
                    }
                }
                // if untried[s'] is empty then
                if (untried[sPrimed].Count == 0)
                {
                    // if unbacktracked[s'] is empty then return stop
                    if (unbacktracked[sPrimed].Count == 0)
                    {
                        a = default(A);
                    }
                    else
                    {
                        // else a <- an action b such that result[s', b] =
                        // POP(unbacktracked[s'])
                        S popped = unbacktracked[sPrimed][0];
                        unbacktracked[sPrimed].RemoveAt(0);
                        foreach (Pair<S, A> sa in result.Keys)
                        {
                            if (sa.First.Equals(sPrimed) && result[sa].Equals(popped))
                            {
                                a = sa.Second;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // else a <- POP(untried[s'])
                    a = untried[sPrimed][0];
                    untried[sPrimed].RemoveAt(0);
                }
            }

            if (a == null)
            {
                // I'm either at the Goal or can't get to it,
                // which in either case I'm finished so just die.
                setAlive(false);
            }

            // s <- s'
            s = sPrimed;
            // return a
            return a != null ? a : NoOpAction.NO_OP as agent.IAction;
        }

        //
        // PRIVATE METHODS
        //

        private void init()
        {
            setAlive(true);
            result.Clear();
            untried.Clear();
            unbacktracked.Clear();
            s = default(S);
            a = default(A);
        }
    } 
}
