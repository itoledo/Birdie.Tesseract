using System.Collections.Generic;

namespace tvn.cosine.ai.search.framework.problem
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach(3rd Edition): page 66.<para /> 
    /// A problem can be defined formally by five components: <para />
    ///  
    /// * The initial state that the agent starts in. <para />
    /// * A description of the possible<b> actions</b> available to the agent.
    /// Given a particular state s, ACTIONS(s) returns the set of actions that can be
    /// executed in s. <para />
    /// * A description of what each action does; the formal name for this is the
    /// transition model, specified by a function RESULT(s, a) that returns the
    /// state that results from doing action a in state s.<para />
    /// * The goal test, which determines whether a given state is a goal
    /// state.<para />
    /// * A path cost function that assigns a numeric cost to each path. The
    /// problem-solving agent chooses a cost function that reflects its own
    /// performance measure. The <b>step cost</b> of taking action a in state s to
    /// reach state s' is denoted by c(s,a,s')<para />
    ///
    /// This implementation provides an additional solution test. It can be used to
    /// compute more than one solution or to formulate acceptance criteria for the
    /// sequence of actions.
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="A"></typeparam>
    public abstract class ProblemBase<S, A> : IProblem<S, A>
    {
        public abstract IList<A> GetActions(S state);
        public abstract S GetInitialState();
        public abstract S GetResult(S state, A action);
        public abstract double GetStepCosts(S state, A action, S stateDelta);
        public abstract bool TestGoal(S state);

        public bool TestSolution(Node<S, A> node)
        {
            return TestGoal(node.getState());
        }
    }
}
