using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.util;

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
        void setHeuristicFunction(ToDoubleFunction<Node<S, A>> h);
    } 
}
