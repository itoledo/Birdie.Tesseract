namespace tvn_cosine.ai.Search.Framework.EvaluationFunctions
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach(3rd Edition): page 92.<br>
    ///
    /// The evaluation function is construed as a cost estimate, so the node with the
    /// lowest evaluation is expanded first.
    /// </summary>
    public interface IEvaluationFunction
    {
        double f(Node n);
    } 
}
