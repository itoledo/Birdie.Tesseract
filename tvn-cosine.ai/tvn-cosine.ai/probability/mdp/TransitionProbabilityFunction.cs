using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.probability.mdp
{
    /// <summary>
    /// Return the probability of going from state s using action a to s' based
    /// on the underlying transition model P(s' | s, a).
    /// </summary>
    /// <typeparam name="S">the state type.</typeparam>
    /// <typeparam name="A">the action type.</typeparam>
    /// <param name="sDelta">the state s' being transitioned to.</param>
    /// <param name="s">the state s being transitions from.</param>
    /// <param name="a">the action used to move from state s to s'.</param>
    /// <returns>the probability of going from state s using action a to s'.</returns>
    public delegate double TransitionProbabilityFunction<S, A>(S sDelta, S s, A a) where A : IAction;

}
