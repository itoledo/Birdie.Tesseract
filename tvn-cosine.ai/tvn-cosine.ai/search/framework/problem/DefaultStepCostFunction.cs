using tvn.cosine.ai.search.framework.problem.api;

namespace tvn.cosine.ai.search.framework.problem
{
    public class DefaultStepCostFunction<S, A> : IStepCostFunction<S, A>
    {
        public double applyAsDouble(S s, A a, S sDelta)
        {
            return 1D;
        }
    }
}
