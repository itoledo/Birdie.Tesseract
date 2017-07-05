using System.Collections.Generic;

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
        public static List<Node<S, A>> getPathFromRoot<S, A>(Node<S, A> node)
        {
            List<Node<S, A>> path = new List<Node<S, A>>();
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
        public static List<A> getSequenceOfActions<S, A>(Node<S, A> node)
        {
            List<A> actions = new List<A>();
            while (!node.isRootNode())
            {
                actions.Insert(0, node.getAction());
                node = node.getParent();
            }
            return actions;
        }

        public static List<A> toActions<S, A>(Node<S, A> node)
        {
            return node != null ? getSequenceOfActions(node) : null;
        }

        public static S toState<S, A>(Node<S, A> node)
        {
            return node != null ? node.getState() : default(S);
        }
    }
}
