using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.qsearch;

namespace tvn.cosine.ai.search.uninformed
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 85.<br>
     * <br>
     * Depth-first search always expands the deepest node in the current frontier of
     * the search tree. <br>
     * <br>
     * <b>Note:</b> Supports TreeSearch, GraphSearch, and BidirectionalSearch. Just
     * provide an instance of the desired QueueSearch implementation to the
     * constructor!
     *
     * @author Ruediger Lunde
     * @author Ravi Mohan
     * 
     */
    public class DepthFirstSearch<S, A> : QueueBasedSearch<S, A>
    {
        public DepthFirstSearch(QueueSearch<S, A> impl)
                : base(impl, Factory.CreateLifoQueue<Node<S, A>>())
        {

        }
    }
}
