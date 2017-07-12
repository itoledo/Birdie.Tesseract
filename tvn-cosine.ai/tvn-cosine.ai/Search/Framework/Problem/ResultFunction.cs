namespace tvn.cosine.ai.search.framework.problem
{ 
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 67. <para />
    ///  
    /// A description of what each action does; the formal name for this is the
    /// transition model, specified by a function RESULT(s, a) that returns the state
    /// that results from doing action a in state s. We also use the term successor
    /// to refer to any state reachable from a given state by a single action.
    /// </summary>
    /// <typeparam name="S">The type used to represent states</typeparam>
    /// <typeparam name="A">The type of the actions to be used to navigate through the state space</typeparam>
    /// <param name="state"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public delegate S ResultFunction<S, A>(S state, A action);
}
