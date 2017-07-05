using System;
using System.Collections.Generic;

namespace tvn.cosine.ai.search.framework.qsearch
{
    public class LifoQueue<E> : Stack<E>, IQueue<E>
    {
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(E item)
        {
            this.Push(item);
        }

        public bool add(E e)
        {
            Add(e);
            return true;
        }

        public E element()
        {
            return this.Peek();
        }

        public bool isEmpty()
        {
            return Count == 0;
        }

        public bool offer(E e)
        {
            return add(e);
        }

        public E peek()
        {
            return element();
        }

        public E poll()
        {
            return remove();
        }

        public E remove()
        {
            return Pop();
        }

        public bool Remove(E item)
        {
            throw new NotImplementedException();
        }

        public int size()
        {
            return this.Count;
        }
    }
}
