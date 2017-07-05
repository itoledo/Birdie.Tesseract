using System.Collections.Generic;

namespace tvn.cosine.ai.search.framework.qsearch
{
    public class FifoQueueWithHashSet<E> : List<E>, IQueue<E>
    {
        private HashSet<E> elements = new HashSet<E>();


        public bool add(E e)
        {
            if (!elements.Contains(e))
            {
                elements.Add(e);
                Add(e);
                return true;
            }
            return false;
        }


        public bool offer(E e)
        {
            if (!elements.Contains(e))
            {
                elements.Add(e);
                Add(e);
                return true;
            }
            return false;
        }


        public E remove()
        {
            E result = this[0];
            elements.Remove(result);
            return result;
        }


        public E poll()
        {
            return remove();
        }


        public bool contains(E e)
        {
            return elements.Contains(e);
        }

        public bool isEmpty()
        {
            return this.Count == 0;
        }

        public int size()
        {
            return this.Count;
        }

        public E element()
        {
            return this[0];
        }

        public E peek()
        {
            return this[0];
        }
    }
}