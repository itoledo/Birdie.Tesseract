using tvn_cosine.ai.Agents;

namespace tvn_cosine.ai.Search.Framework.EvaluationFunctions
{
    /// <summary>
    /// Artificial Intelligence A Modern Approach(3rd Edition): page 92.<br>
    ///
    /// a heuristic function, denoted h(n):<br>
    /// h(n) = estimated cost of the cheapest path from the state at node n to a goal
    /// state.<br>
    ///  
    /// Notice that h(n) takes a node as input, but, unlike g(n) it depends only on
    /// the state at that node.
    /// </summary>
    public interface IHeuristicFunction
    {
        double h(IState state);
    }
}
