using System.Collections.Generic;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.probability.mdp.impl
{
    public class ActionsFunction<S, A> : mdp.ActionsFunction<S, A>
        where A : IAction
    {
        private System.Func<S, ISet<A>> func;

        public ActionsFunction(System.Func<S, ISet<A>> func)
        {
            this.func = func;
        }

        public ISet<A> actions(S s)
        {
            return func(s);
        }
    }
}
