using System.Collections.Generic;

namespace tvn.cosine.ai.search.nondeterministic
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 136. <para />
    ///  
    /// Closely related to ResultFunction, but for non-deterministic problems; in
    /// these problems, the outcome of an action will be a set of results, not a
    /// single result. This class implements the functionality of RESULTS(s, a), page
    /// 136, returning the states resulting from doing action a in state s. 
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <param name="state">a particular state.</param>
    /// <param name="action">an action to be performed in state s.</param>
    /// <returns>the states that result from doing action a in state s.</returns>
    public delegate IList<S> ResultsFunction<S, A>(S state, A action); 
}
