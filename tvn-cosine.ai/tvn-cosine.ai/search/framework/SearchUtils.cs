using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.search.framework
{
    /**
     * Provides several useful static methods for implementing search.
     *
     * @author Ruediger Lunde
     */
    public class SearchUtils
    {
        /**
         * Returns the path from the root node to this node.
         *
         * @return the path from the root node to this node.
         */
        public static IQueue<Node<S, A>> getPathFromRoot<S, A>(Node<S, A> node)
        {
            IQueue<Node<S, A>> path = Factory.CreateQueue<Node<S, A>>();
            while (!node.isRootNode())
            {
                path.Insert(0, node);
                node = node.getParent();
            }
            // ensure the root node is added
            path.Insert(0, node);
            return path;
        }

        /**
         * Returns the list of actions which corresponds to the complete path to the
         * given node. The list is empty, if the node is the root node of the search
         * tree.
         */
        public static IQueue<A> getSequenceOfActions<S, A>(Node<S, A> node)
        {
            IQueue<A> actions = Factory.CreateQueue<A>();
            while (node != null && !node.isRootNode())
            {
                actions.Insert(0, node.getAction());
                node = node.getParent();
            }
            return actions;
        }

        public static IQueue<A> toActions<S, A>(Node<S, A> node)
        {
            return getSequenceOfActions(node);
        }


        public static S toState<S, A>(Node<S, A> node)
        {
            return node == null ? default(S) : node.getState();
        }
    }
}
