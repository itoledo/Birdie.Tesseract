namespace tvn.cosine.ai.search.framework.problem
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 68.<para />
    /// The step cost of taking action a in state s to reach state s' is denoted by c(s, a, s').
    /// </summary>
    /// <typeparam name="S">The type used to represent states</typeparam>
    /// <typeparam name="A">The type of the actions to be used to navigate through the state space</typeparam>
    /// <param name="state"></param>
    /// <param name="action"></param>
    /// <param name="stateDelta"></param>
    /// <returns></returns>
    public delegate double StepCostFunction<S, A>(S state, A action, S stateDelta);
}
