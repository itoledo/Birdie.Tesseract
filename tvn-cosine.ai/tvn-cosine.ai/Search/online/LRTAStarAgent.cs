using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.informed;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.search.online
{
    /**
   * Artificial Intelligence A Modern Approach (3rd Edition): Figure 4.24, page
   * 152. 
   *  
   * 
   * <pre>
   * function LRTA*-AGENT(s') returns an action
   *   inputs: s', a percept that identifies the current state
   *   persistent: result, a table, indexed by state and action, initially empty
   *               H, a table of cost estimates indexed by state, initially empty
   *               s, a, the previous state and action, initially null
   *           
   *   if GOAL-TEST(s') then return stop
   *   if s' is a new state (not in H) then H[s'] &lt;- h(s')
   *   if s is not null
   *     result[s, a] &lt;- s'
   *     H[s] &lt;-        min LRTA*-COST(s, b, result[s, b], H)
   *             b (element of) ACTIONS(s)
   *   a &lt;- an action b in ACTIONS(s') that minimizes LRTA*-COST(s', b, result[s', b], H)
   *   s &lt;- s'
   *   return a
   *   
   * function LRTA*-COST(s, a, s', H) returns a cost estimate
   *   if s' is undefined then return h(s)
   *   else return c(s, a, s') + H[s']
   * </pre>
   * 
   * Figure 4.24 LRTA*-AGENT selects an action according to the value of
   * neighboring states, which are updated as the agent moves about the state
   * space. 
   *  
   * <b>Note:</b> This algorithm fails to exit if the goal does not exist (e.g.
   * A<->B Goal=X), this could be an issue with the implementation. Comments
   * welcome.
   * 
   * @author Ciaran O'Reilly
   * @author Mike Stampone
   */
    public class LRTAStarAgent<S, A> : AbstractAgent
        where A : agent.Action
    {
        private IOnlineSearchProblem<S, A> problem;
        private Func<Percept, S> ptsFn;
        private HeuristicEvaluationFunction<S> h;
        // persistent: result, a table, indexed by state and action, initially empty
        private readonly TwoKeyDictionary<S, A, S> result = new TwoKeyDictionary<S, A, S>();
        // H, a table of cost estimates indexed by state, initially empty
        private readonly IDictionary<S, double> H = new Dictionary<S, double>();
        // s, a, the previous state and action, initially null
        private S s = default(S);
        private A a = default(A);

        /**
         * Constructs a LRTA* agent with the specified search problem, percept to
         * state function, and heuristic function.
         * 
         * @param problem
         *            an online search problem for this agent to solve.
         * @param ptsFn
         *            a function which returns the problem state associated with a
         *            given Percept.
         * @param h
         *            heuristic function <em>h(n)</em>, which estimates the cost of
         *            the cheapest path from the state at node <em>n</em> to a goal
         *            state.
         */
        public LRTAStarAgent(IOnlineSearchProblem<S, A> problem, Func<Percept, S> ptsFn, HeuristicEvaluationFunction<S> h)
        {
            setProblem(problem);
            setPerceptToStateFunction(ptsFn);
            setHeuristicFunction(h);
        }

        /**
         * Returns the search problem of this agent.
         * 
         * @return the search problem of this agent.
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
        public Func<Percept, S> getPerceptToStateFunction()
        {
            return ptsFn;
        }

        /**
         * Sets the percept to state function of this agent.
         * 
         * @param ptsFn
         *            a function which returns the problem state associated with a
         *            given Percept.
         */
        public void setPerceptToStateFunction(Func<Percept, S> ptsFn)
        {
            this.ptsFn = ptsFn;
        }

        /**
         * Returns the heuristic function of this agent.
         */
        public HeuristicEvaluationFunction<S> getHeuristicFunction()
        {
            return h;
        }

        /**
         * Sets the heuristic function of this agent.
         * 
         * @param h
         *            heuristic function <em>h(n)</em>, which estimates the cost of
         *            the cheapest path from the state at node <em>n</em> to a goal
         *            state.
         */
        public void setHeuristicFunction(HeuristicEvaluationFunction<S> h)
        {
            this.h = h;
        }

        // function LRTA*-AGENT(s') returns an action
        // inputs: s', a percept that identifies the current state 
        public override agent.Action execute(Percept psPrimed)
        {
            S sPrimed = ptsFn(psPrimed);
            // if GOAL-TEST(s') then return stop
            if (problem.TestGoal(sPrimed))
            {
                a = default(A);
            }
            else
            {
                double min = 0;
                // if s' is a new state (not in H) then H[s'] <- h(s')
                if (!H.ContainsKey(sPrimed))
                {
                    H.Add(sPrimed, getHeuristicFunction()(sPrimed));
                }
                // if s is not null
                if (null != s)
                {
                    // result[s, a] <- s'
                    result.Add(s, a, sPrimed);

                    // H[s] <- min LRTA*-COST(s, b, result[s, b], H)
                    // b (element of) ACTIONS(s)
                    min = double.MaxValue;
                    foreach (A b in problem.GetActions(s))
                    {
                        double cost = lrtaCost(s, b, result[s, b]);
                        if (cost < min)
                        {
                            min = cost;
                        }
                    }
                    H.Add(s, min);
                }
                // a <- an action b in ACTIONS(s') that minimizes LRTA*-COST(s', b,
                // result[s', b], H)
                min = double.MaxValue;
                // Just in case no actions
                a = default(A);
                foreach (A b in problem.GetActions(sPrimed))
                {
                    double cost = lrtaCost(sPrimed, b, result[sPrimed, b]);
                    if (cost < min)
                    {
                        min = cost;
                        a = b;
                    }
                }
            }

            // s <- s'
            s = sPrimed;

            if (a == null)
            {
                // I'm either at the Goal or can't get to it,
                // which in either case I'm finished so just die.
                setAlive(false);
            }
            // return a
            return a != null ? a : DynamicAction.NO_OP as agent.Action;
        }

        //
        // PRIVATE METHODS
        //
        private void init()
        {
            setAlive(true);
            result.Clear();
            H.Clear();
            s = default(S);
            a = default(A);
        }

        // function LRTA*-COST(s, a, s', H) returns a cost estimate
        private double lrtaCost(S s, A action, S sDelta)
        {
            // if s' is undefined then return h(s)
            if (null == sDelta)
            {
                return getHeuristicFunction()(s);
            }
            // else return c(s, a, s') + H[s']
            return problem.GetStepCosts(s, action, sDelta) + H[sDelta];
        }
    } 
}
