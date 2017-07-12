namespace tvn.cosine.ai.search.framework.problem
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 67. 
     *  
     * The goal test, which determines whether a given state is a goal state.
     *
     * @param <S> The type used to represent states
     *
     * @author Ruediger Lunde
     */
    public delegate bool GoalTest<S>(S state);
}

