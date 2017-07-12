using System.Collections.Generic;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.probability.mdp
{
    /// <summary>
    /// Get the set of actions for state s.
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <param name="state"></param>
    /// <returns></returns>
    public delegate ISet<A> ActionsFunction<S, A>(S s) where A : IAction; 
}
