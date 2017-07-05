namespace tvn.cosine.ai.search.framework.problem
{
    public delegate double StepCostFunction<S, A>(S state, A action, S stateDelta);
}
