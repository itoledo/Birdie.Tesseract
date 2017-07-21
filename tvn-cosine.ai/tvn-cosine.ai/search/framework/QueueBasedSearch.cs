﻿using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;
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

        public virtual IQueue<A> findActions(Problem<S, A> p)
        {
            impl.getNodeExpander().useParentLinks(true);
            frontier.Clear();
            Node<S, A> node = impl.findNode(p, frontier);
            return SearchUtils.toActions(node);
        }

        public virtual S findState(Problem<S, A> p)
        {
            impl.getNodeExpander().useParentLinks(false);
            frontier.Clear();
            Node<S, A> node = impl.findNode(p, frontier);
            return SearchUtils.toState(node);
        }

        public virtual Metrics getMetrics()
        {
            return impl.getMetrics();
        }

        public virtual void addNodeListener(Consumer<Node<S, A>> listener)
        {
            impl.getNodeExpander().addNodeListener(listener);
        }

        public virtual bool removeNodeListener(Consumer<Node<S, A>> listener)
        {
            return impl.getNodeExpander().removeNodeListener(listener);
        }
    }
}
