using tvn_cosine.ai.Agents;

namespace tvn_cosine.ai.Search.Framework.Problems
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 66. 
    ///
    /// A problem can be defined formally by five components:  
    ///
    ///* The initial state  that the agent starts in. 
    ///* A description of the possible actions available to the agent. 
    /// Given a particular state s, ACTIONS(s)returns the set of actions that can be executed in s. 
    /// * A description of what each action does; the formal name for this is the
    /// transition model, specified by a function RESULT(s, a) that returns the state that results from doing action a in state s. 
    /// * The <b>goal test</b>, which determines whether a given state is a goal state. 
    /// * A path cost function that assigns a numeric cost to each path. The
    /// problem-solving agent chooses a cost function that reflects its own
    /// performance measure. The <b>step cost</b> of taking action a in state s to
    /// reach state s' is denoted by c(s,a,s') 
    /// </summary>
    public class Problem
    {
        protected IState initialState;
        protected IActionsFunction actionsFunction;
        protected IResultFunction resultFunction;
        protected IGoalTest goalTest;
        protected IStepCostFunction stepCostFunction;

        /// <summary>
        /// Constructs a problem with the specified components, and a default step cost function (i.e. 1 per step).
        /// </summary>
        /// <param name="initialState">the initial state that the agent starts in.</param>
        /// <param name="actionsFunction">a description of the possible actions available to the agent.</param>
        /// <param name="resultFunction">a description of what each action does; the formal name for this is the transition model, specified by a function RESULT(s, a) that returns the state that results from doing action a in state s.</param>
        /// <param name="goalTest">test determines whether a given state is a goal state.</param>
        public Problem(IState initialState, IActionsFunction actionsFunction,
                IResultFunction resultFunction, IGoalTest goalTest)
            : this(initialState, actionsFunction, resultFunction, goalTest, new DefaultStepCostFunction())
        { }

        /// <summary>
        /// Constructs a problem with the specified components, which includes a step cost function.
        /// </summary>
        /// <param name="initialState">the initial state of the agent.</param>
        /// <param name="actionsFunction">a description of the possible actions available to the agent.</param>
        /// <param name="resultFunction">a description of what each action does; the formal name for this is the transition model, specified by a function RESULT(s, a) that returns the state that results from doing action a in state s.</param>
        /// <param name="goalTest">test determines whether a given state is a goal state.</param>
        /// <param name="stepCostFunction">a path cost function that assigns a numeric cost to each path. The problem-solving-agent chooses a cost function that reflects its own performance measure.</param>
        public Problem(IState initialState, IActionsFunction actionsFunction,
                       IResultFunction resultFunction, IGoalTest goalTest,
                       IStepCostFunction stepCostFunction)
        {
            this.initialState = initialState;
            this.actionsFunction = actionsFunction;
            this.resultFunction = resultFunction;
            this.goalTest = goalTest;
            this.stepCostFunction = stepCostFunction;
        }

        /// <summary>
        /// Returns the initial state of the agent.
        /// </summary>
        /// <returns>the initial state of the agent.</returns>
        public IState getInitialState()
        {
            return initialState;
        }


        /// <summary>
        /// Returns <code>true</code> if the given state is a goal state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns><code>true</code> if the given state is a goal state.</returns>
        public bool isGoalState(IState state)
        {
            return goalTest.isGoalState(state);
        }


        /// <summary>
        /// Returns the goal test.
        /// </summary>
        /// <returns>the goal test.</returns>
        public IGoalTest getGoalTest()
        {
            return goalTest;
        }

        /// <summary>
        /// Returns the description of the possible actions available to the agent.
        /// </summary>
        /// <returns>the description of the possible actions available to the agent.</returns>
        public IActionsFunction getActionsFunction()
        {
            return actionsFunction;
        }

        /// <summary>
        /// Returns the description of what each action does.
        /// </summary>
        /// <returns>the description of what each action does.</returns>
        public IResultFunction getResultFunction()
        {
            return resultFunction;
        }

        /// <summary>
        /// Returns the path cost function.
        /// </summary>
        /// <returns>the path cost function.</returns>
        public IStepCostFunction getStepCostFunction()
        {
            return stepCostFunction;
        }

        //
        // PROTECTED METHODS
        //
        protected Problem() { }
    }
}
