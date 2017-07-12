using System.Collections.Generic;
using tvn.cosine.ai.search.framework.problem;

namespace tvn.cosine.ai.search.nondeterministic
{
    /// <summary>
    /// Non-deterministic problems may have multiple results for a given state and
    /// action; this class handles these results by mimicking Problem and replacing
    /// ResultFunction (one result) with ResultsFunction (a set of results).
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="A"></typeparam>
    public class NondeterministicProblem<S, A>
    { 
        protected S initialState;
        protected ActionsFunction<S, A> actionsFunction;
        protected GoalTest<S> goalTest;
        protected StepCostFunction<S, A> stepCostFunction;
        protected ResultsFunction<S, A> resultsFunction;
         
        public NondeterministicProblem(S initialState, ActionsFunction<S, A> actionsFn, 
                                       ResultsFunction<S, A> resultsFn, GoalTest<S> goalTest)
            : this(initialState, actionsFn, resultsFn, goalTest, (s, a, sPrimed) => 1.0)
        { }


        public NondeterministicProblem(S initialState, ActionsFunction<S, A> actionsFn, 
                                       ResultsFunction<S, A> resultsFn, GoalTest<S> goalTest, 
                                       StepCostFunction<S, A> stepCostFn)
        {
            this.initialState = initialState;
            this.actionsFunction = actionsFn;
            this.resultsFunction = resultsFn;
            this.goalTest = goalTest;
            this.stepCostFunction = stepCostFn;
        }

        /**
         * Returns the initial state of the agent.
         * 
         * @return the initial state of the agent.
         */
        public S getInitialState()
        {
            return initialState;
        }

        /**
         * Returns <code>true</code> if the given state is a goal state.
         * 
         * @return <code>true</code> if the given state is a goal state.
         */
        public bool testGoal(S state)
        {
            return goalTest(state);
        }

        /**
         * Returns the description of the possible actions available to the agent.
         */
        public IList<A> getActions(S state)
        {
            return actionsFunction(state);
        }

        /**
         * Return the description of what each action does.
         * 
         * @return the description of what each action does.
         */
        public IList<S> getResults(S state, A action)
        {
            return resultsFunction(state, action);
        }

        /**
         * Returns the <b>step cost</b> of taking action <code>action</code> in state <code>state</code> to reach state
         * <code>stateDelta</code> denoted by c(s, a, s').
         */
        double getStepCosts(S state, A action, S stateDelta)
        {
            return stepCostFunction(state, action, stateDelta);
        }
    }

}
