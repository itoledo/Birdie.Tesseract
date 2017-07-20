namespace tvn_cosine.ai.v2.common
{
    /// <summary>
    /// Simple predicate
    /// </summary>
    /// <typeparam name="T">The type of the input</typeparam>
    /// <param name="input">The input</param>
    /// <returns>boolean value</returns>
    public delegate bool Predicate<T>(T input);
}
