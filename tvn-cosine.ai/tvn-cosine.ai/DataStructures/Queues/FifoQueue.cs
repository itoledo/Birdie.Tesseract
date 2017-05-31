using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace tvn_cosine.ai.DataStructures.Queues
{
    public class FifoQueue<T> : IQueue<T>
    {
        protected readonly Queue<T> backing_collection;

        public FifoQueue()
        {
            backing_collection = new Queue<T>();
        }

        public T Remove()
        {
            if (!IsEmpty())
            {
                return backing_collection.Dequeue();
            }
            throw new ArgumentOutOfRangeException("The FifoQueue<T> is empty");
        }

        public bool IsEmpty()
        {
            return backing_collection.Count == 0;
        }

        public T Peek()
        {
            if (!IsEmpty())
            {
                return backing_collection.ElementAt(0);
            }

            throw new ArgumentOutOfRangeException("The FifoQueue<T> is empty");
        }

        #region ICollection<T> Support
        public int Count
        {
            get
            {
                return backing_collection.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(T item)
        {
            backing_collection.Enqueue(item);
        }

        public void Clear()
        {
            backing_collection.Clear();
        }

        public bool Contains(T item)
        {
            return backing_collection.Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            backing_collection.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IEnumerator<T> Support
        public IEnumerator<T> GetEnumerator()
        {
            return backing_collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return backing_collection.GetEnumerator();
        }
        #endregion 
    }

    public class FifoQueue : FifoQueue<object>, IQueue
    { } 
}
