using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.probability.mdp.impl
{
    class TransitionProbabilityFunction<S, A> : mdp.TransitionProbabilityFunction<S, A>
        where A : IAction
    {
        private System.Func<S, S, A, double> func;

        public TransitionProbabilityFunction(System.Func<S, S, A, double> func)
        {
            this.func = func;
        }

        public double probability(S sDelta, S s, A a)
        {
            return func(sDelta, s, a);
        }
    }
}
