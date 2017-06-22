using tvn_cosine.ai.Agents;

namespace tvn_cosine.ai.Search.Framework.Problems
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 67. 
    ///
    /// The goal test, which determines whether a given state is a goal state.
    /// </summary>
    public interface IGoalTest
    {
        /**
         * Returns <code>true</code> if the given state is a goal state.
         * 
         * @return <code>true</code> if the given state is a goal state.
         */
        bool isGoalState(IState state);
    }
}
