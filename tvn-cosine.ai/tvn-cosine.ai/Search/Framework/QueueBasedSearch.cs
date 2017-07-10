using System;
using System.Collections.Generic;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.search.framework.qsearch;

namespace tvn.cosine.ai.search.framework
{
    /**
 * Base class for all search algorithms which use a queue to manage not yet
 * explored nodes. Subclasses are responsible for node prioritization. They
 * define the concrete queue to be used as frontier in their constructor.
 * Search space exploration control is always delegated to some
 * <code>QueueSearch</code> implementation.
 *
 * @param <S> The type used to represent states
 * @param <A> The type of the actions to be used to navigate through the state space
 *
 * @author Ruediger Lunde
 */
    public abstract class QueueBasedSearch<S, A> : SearchForActions<S, A>, SearchForStates<S, A>
    {
        protected readonly QueueSearch<S, A> impl;
        private readonly IQueue<Node<S, A>> frontier;

        protected QueueBasedSearch(QueueSearch<S, A> impl, IQueue<Node<S, A>> queue)
        {
            this.impl = impl;
            this.frontier = queue;
        }


        public IList<A> findActions(IProblem<S, A> p)
        {
            impl.getNodeExpander().UseParentLinks(true);
            frontier.Clear();
            Node<S, A> node = impl.findNode(p, frontier);
            return SearchUtils.toActions(node);
        }


        public S findState(IProblem<S, A> p)
        {
            impl.getNodeExpander().UseParentLinks(false);
            frontier.Clear();
            Node<S, A> node = impl.findNode(p, frontier);
            return SearchUtils.toState(node);
        }


        public IDictionary<string, double> getMetrics()
        {
            return impl.getMetrics();
        }


        public void addNodeListener(Action<Node<S, A>> listener)
        {
            impl.getNodeExpander().addNodeListener(listener);
        }


        public bool removeNodeListener(Action<Node<S, A>> listener)
        {
            return impl.getNodeExpander().removeNodeListener(listener);
        }
    }
}
