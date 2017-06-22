namespace tvn_cosine.ai.Search.Framework.Problems
{
    /// <summary>
    /// An interface describing a problem that can be tackled from both directions at once (i.e InitialState&lt;->Goal).
    /// </summary>
    public interface BidirectionalProblem
    {
         Problem getOriginalProblem(); 
         Problem getReverseProblem();
    }
}
