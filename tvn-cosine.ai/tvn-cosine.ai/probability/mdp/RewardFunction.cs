namespace tvn.cosine.ai.probability.mdp
{
    /// <summary>
    /// Get the reward associated with being in state s.
    /// </summary>
    /// <typeparam name="S"> the state type.</typeparam>
    /// <param name="s">the state whose award is sought.</param>
    /// <returns>the reward associated with being in state s.</returns>
    public delegate double RewardFunction<S>(S s); 
}
