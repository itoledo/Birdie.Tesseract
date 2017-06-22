using tvn_cosine.ai.Agents;

namespace tvn_cosine.ai.Search.Framework.EvaluationFunctions
{
    /// <summary>
    /// A heuristic function factory creates a heuristic function for a given goal.
    /// Autonomously acting problem solving agents can profit from this kind of
    /// factories after selecting a new goal.
    /// </summary>
    public interface IHeuristicFunctionFactory
    { 
        IHeuristicFunction createHeuristicFunction(IState goal); 
    }
}
