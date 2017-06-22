using tvn_cosine.ai.Search.Framework.EvaluationFunctions;

namespace tvn_cosine.ai.Search.Framework
{
    /// <summary>
    /// Search algorithms which make use of heuristics to guide the search are expected to implement this interface.
    /// </summary>
    public interface IInformed
    {
        void setHeuristicFunction(IHeuristicFunction hf);
    }
}
