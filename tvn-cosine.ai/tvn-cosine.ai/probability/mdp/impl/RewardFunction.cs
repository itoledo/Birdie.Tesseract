using System;

namespace tvn.cosine.ai.probability.mdp.impl
{
    public class RewardFunction<S> : mdp.RewardFunction<S>
    {
        private Func<S, double> func;

        public RewardFunction(Func<S, double> func)
        {
            this.func = func;
        }

        public double reward(S s)
        {
            return func(s);
        }
    }
}
