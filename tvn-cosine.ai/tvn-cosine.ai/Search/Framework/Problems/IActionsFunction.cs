using System.Collections.Generic;
using tvn_cosine.ai.Agents;

namespace tvn_cosine.ai.Search.Framework.Problems
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach(3rd Edition): page 67. 
    ///
    /// Given a particular state s, ACTIONS(s)returns the set of actions that can be
    /// executed in s.We say that each of these actions is applicable in s.
    /// </summary>
    public interface IActionsFunction
    { 
        /// <summary>
        /// Given a particular state s, returns the set of actions that can be executed in s.
        /// </summary>
        /// <param name="s">a particular state.</param>
        /// <returns>the set of actions that can be executed in s.</returns>
        ISet<IAction> actions(IState s);
    }
}
