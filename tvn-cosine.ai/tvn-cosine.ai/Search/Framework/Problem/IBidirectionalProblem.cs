namespace tvn.cosine.ai.search.framework.problem
{ 
    /// <summary>
    /// An interface describing a problem that can be tackled from both directions at once (i.e InitialState&lt;->Goal).
    /// </summary>
    /// <typeparam name="S">The type used to represent states</typeparam>
    /// <typeparam name="A">The type of the actions to be used to navigate through the state space</typeparam>
    public interface IBidirectionalProblem<S, A> : IProblem<S, A>
    {
        IProblem<S, A> getOriginalProblem();
        IProblem<S, A> getReverseProblem();
    } 
}
