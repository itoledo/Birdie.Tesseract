using System.Collections.Generic;
using tvn.cosine.ai.agent;

namespace tvn.cosine.ai.probability.mdp
{
    /**
     * An interface for MDP action functions.
     * 
     * @param <S>
     *            the state type.
     * @param <A>
     *            the action type.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public interface ActionsFunction<S, A>
        where A : IAction
    {
        /**
         * Get the set of actions for state s.
         * 
         * @param s
         *            the state.
         * @return the set of actions for state s.
         */
        ISet<A> actions(S s);
    }
}
