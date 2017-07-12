using System.Collections.Generic;

namespace tvn.cosine.ai.search.framework.problem
{
    /// <summary>
    /// Configurable problem which uses objects to explicitly represent the required functions.
    /// </summary>
    /// <typeparam name="S">The type used to represent states</typeparam>
    /// <typeparam name="A">The type of the actions to be used to navigate through the state space</typeparam>
    public class GeneralProblem<S, A> : ProblemBase<S, A>
    {
        private S initialState;

        private readonly ActionsFunction<S, A> actionsFunction;
        private readonly ResultFunction<S, A> resultFunction;
        private readonly GoalTest<S> goalTest;
        private readonly StepCostFunction<S, A> stepCostFunction;

        /// <summary>
        /// Constructs a problem with the specified components, which includes a step cost function.
        /// </summary>
        /// <param name="initialState">the initial state of the agent.</param>
        /// <param name="actionsFn">a description of the possible actions available to the agent.</param>
        /// <param name="resultFn">    
        /// a description of what each action does; the formal name for
        /// this is the transition model, specified by a function
        /// RESULT(s, a) that returns the state that results from doing
        /// action a in state s.</param>
        /// <param name="goalTest">test determines whether a given state is a goal state.</param>
        /// <param name="stepCostFn">
        /// a path cost function that assigns a numeric cost to each path.
        /// The problem-solving-agent chooses a cost function that
        /// reflects its own performance measure.</param>
        public GeneralProblem(S initialState, ActionsFunction<S, A> actionsFn, 
                              ResultFunction<S, A> resultFn, GoalTest<S> goalTest, 
                              StepCostFunction<S, A> stepCostFn)
        {
            this.initialState = initialState;
            this.actionsFunction = actionsFn;
            this.resultFunction = resultFn;
            this.goalTest = goalTest;
            this.stepCostFunction = stepCostFn;
        }

        /// <summary>
        /// Constructs a problem with the specified components, and a default step cost function (i.e. 1 per step).
        /// </summary>
        /// <param name="initialState">the initial state that the agent starts in.</param>
        /// <param name="actionsFn">a description of the possible actions available to the agent.</param>
        /// <param name="resultFn">
        /// a description of what each action does; the formal name for
        /// this is the transition model, specified by a function
        /// RESULT(s, a) that returns the state that results from doing
        /// action a in state s.</param>
        /// <param name="goalTest">test determines whether a given state is a goal state.</param>
        public GeneralProblem(S initialState, ActionsFunction<S, A> actionsFn, 
                              ResultFunction<S, A> resultFn, GoalTest<S> goalTest)
            : this(initialState, actionsFn, resultFn, goalTest, (s, a, sPrimed) => 1.0)
        { }

        public override S GetInitialState()
        {
            return initialState;
        }

        public override IList<A> GetActions(S state)
        {
            return actionsFunction(state);
        }

        public override S GetResult(S state, A action)
        {
            return resultFunction(state, action);
        }

        public override bool TestGoal(S state)
        {
            return goalTest(state);
        }

        public override double GetStepCosts(S state, A action, S statePrimed)
        {
            return stepCostFunction(state, action, statePrimed);
        }
    }
}
