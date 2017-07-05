using System.Collections.Generic;
using tvn.cosine.ai.search.framework.qsearch;

namespace tvn.cosine.ai.search.framework
{/**
 * Factory class for queues. Changes made here will affect all queue based
 * search algorithms of this library.
 *
 * @author Ruediger Lunde
 */
    public class QueueFactory
    {

        /**
         * Returns a {@link LinkedList}.
         */
        public static IQueue<E> createFifoQueue<E>()
        {
            return new FifoQueue<E>();
        }

        /**
         * Returns a {@link LinkedList} which is extended by a {@link HashSet} for efficient containment checks. Elements
         * are only added if they are not already present in the queue. Use only queue methods for access!
         */
        public static IQueue<E> createFifoQueueNoDuplicates<E>()
        {
            return new FifoQueueWithHashSet<E>();
        }

        /**
         * Returns a Last-in-first-out (Lifo) view on a {@link LinkedList}.
         */
        public static IQueue<E> createLifoQueue<E>()
        {
            return new LifoQueue<E>();
        }

        /**
         * Returns a standard java {@link PriorityQueue}. Note that the smallest
         * element comes first!
         */
        public static IQueue<E> createPriorityQueue<E>(IComparer<E> comparator)
        {
            return new PriorityQueue<E>(comparator);
        } 
    } 
}
