namespace tvn_cosine.ai.Search.Framework.EvaluationFunctions
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach (3rd Edition): page 78.
    /// </summary>
    public class PathCostFunction
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns>the cost, traditionally denoted by g(n), of the path from the initial state to the node, as indicated by the parent pointers.</returns>
        public double g(Node n)
        {
            return n.getPathCost();
        }
    }
}
