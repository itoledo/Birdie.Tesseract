using tvn.cosine.ai.search.framework;

namespace tvn.cosine.ai.search.informed
{
    /**
     * Search algorithms which make use of heuristics to guide the search
     * are expected to implement this interface.
     *
     * @author Ruediger Lunde
     */
    public interface Informed<S, A>
    {
        HeuristicEvaluationFunction<Node<S, A>> h { get; set; }
    }
}
