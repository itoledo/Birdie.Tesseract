namespace tvn.cosine.ai.Search.Framework.Problem
{
    public delegate double StepCostFunction<S, A>(S state, A action, S stateDelta);
}
