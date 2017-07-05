namespace tvn.cosine.ai.search.informed
{
    /**
     * Super class for all evaluation functions which make use of heuristics.
     * Informed search algorithms use heuristics to estimate remaining costs to
     * reach a goal state from a given node. Their evaluation functions only differ
     * in the way how they combine the estimated remaining costs with the costs of
     * the already known path to the node.
     * 
     * @author Ruediger Lunde
     *
     */
    public delegate double HeuristicEvaluationFunction<T>(T item); 
}
