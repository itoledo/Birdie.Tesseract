using System.Collections.Generic;

namespace tvn.cosine.ai.search.framework.problem
{ 
    /// <summary>
    /// Artificial Intelligence A Modern Approach(3rd Edition): page 67. <para />
    ///
    /// Given a particular state s, ACTIONS(s)returns the set of actions that can be
    /// executed in s.We say that each of these actions is applicable  in s.
    /// </summary>
    /// <typeparam name="S">The type used to represent states</typeparam>
    /// <typeparam name="A">The type of the actions to be used to navigate through the state space</typeparam>
    /// <param name="state"></param>
    /// <returns></returns>
    public delegate IList<A> ActionsFunction<S, A>(S state);
}
