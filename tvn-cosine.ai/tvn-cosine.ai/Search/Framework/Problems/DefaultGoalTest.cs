using tvn_cosine.ai.Agents;

namespace tvn_cosine.ai.Search.Framework.Problems
{
    /// <summary>
    /// Checks whether a given state equals an explicitly specified goal state.
    /// </summary>
    public class DefaultGoalTest : IGoalTest
    { 
        private IState goalState;

        public DefaultGoalTest(IState goalState)
        {
            this.goalState = goalState;
        }

        public bool isGoalState(IState state)
        {
            return goalState.Equals(state);
        }
    }
}
