using System.Collections;
using System.Collections.Generic; 

namespace tvn.cosine.ai.search.framework.qsearch
{
    public class PriorityQueue<E> : IQueue<E>
    {
        private readonly SortedList<E, E> backingList;

        public PriorityQueue(IComparer<E> comparer)
        {
            Comparer = comparer;
            backingList =  new SortedList<E, E>(Comparer);
        }

        public IComparer<E> Comparer { get; }

        public int Count
        {
            get
            {
                return backingList.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(E item)
        {
            add(item);
        }

        public bool add(E e)
        {
            if (!backingList.ContainsKey(e))
            {
                backingList.Add(e, e);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            backingList.Clear();
        }

        public bool Contains(E item)
        {
            return backingList.ContainsKey(item);
        }

        public void CopyTo(E[] array, int arrayIndex)
        {
            backingList.Keys.CopyTo(array, arrayIndex);
        }

        public E element()
        {
            return backingList.Keys[0];
        }

        public IEnumerator<E> GetEnumerator()
        {
            return backingList.Keys.GetEnumerator();
        }

        public bool isEmpty()
        {
            return backingList.Keys.Count == 0;
        }

        public bool offer(E e)
        {
            backingList.Add(e, e);
            return true;
        }

        public E peek()
        {
            return backingList.Keys[0];
        }

        public E poll()
        {
            var item = peek();
            Remove(item);
            return item;
        }

        public E remove()
        {
            return poll();
        }

        public bool Remove(E item)
        {
           return backingList.Remove(item);
        }

        public int size()
        {
           return backingList.Keys.Count;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return backingList.Keys.GetEnumerator();
        }
    }
}
