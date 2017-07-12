namespace tvn.cosine.ai.search.local
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 127. <para />
    ///  
    /// Each state is rated by the objective function, or (in Genetic Algorithm
    /// terminology) the fitness function. A fitness function should return higher
    /// values for better states.
    ///  
    /// Here, we assume that all values are greater or equal to zero.<para />
    /// </summary>
    /// <typeparam name="A">
    /// the type of the alphabet used in the representation of the
    /// individuals in the population (this is to provide flexibility in
    /// terms of how a problem can be encoded).</typeparam>
    /// <param name="individual">the individual whose fitness is to be accessed.</param>
    /// <returns>the individual's fitness value (the higher the better).</returns>
    public delegate double FitnessFunction<A>(Individual<A> individual);
}
