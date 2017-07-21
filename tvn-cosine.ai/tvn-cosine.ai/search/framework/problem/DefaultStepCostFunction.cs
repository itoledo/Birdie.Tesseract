namespace tvn.cosine.ai.search.framework.problem
{
    public class DefaultStepCostFunction<S, A> : StepCostFunction<S, A>
    {
        public double applyAsDouble(S s, A a, S sDelta)
        {
            return 1D;
        }
    }
}
