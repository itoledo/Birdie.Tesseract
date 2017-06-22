using tvn_cosine.ai.Agents;

namespace tvn_cosine.ai.Search.Framework.Problems
{
    /// <summary>
    /// Returns one for every action.
    /// </summary>
    public class DefaultStepCostFunction : IStepCostFunction
    { 
        public double c(IState stateFrom, IAction action, IState stateTo)
        {
            return 1;
        }
    }
}
