namespace tvn.cosine.ai.search.framework.problem
{ 
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 67. <para />
    /// The goal test, which determines whether a given state is a goal state.
    /// </summary>
    /// <typeparam name="S">The type used to represent states</typeparam>
    /// <param name="state"></param>
    /// <returns></returns>
    public delegate bool GoalTest<S>(S state);
}

