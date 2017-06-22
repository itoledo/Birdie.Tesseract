using System;
using System.Collections;
using System.Collections.Generic;

namespace tvn_cosine.ai.DataStructures.Queues
{
    public class PriorityQueue<T> : IQueue<T>
    {
        protected readonly SortedList<T, T> backing_collection;

        public IComparer<T> Comparer { get; }

        public PriorityQueue()
            : this(Comparer<T>.Default)
        { }

        public PriorityQueue(IComparer<T> comparer)
        {
            backing_collection = new SortedList<T, T>(comparer);
            this.Comparer = comparer;
        }

        public T Remove()
        {
            if (!IsEmpty())
            {
                var item = backing_collection.Values[0];
                backing_collection.Remove(item);

                return item;
            }
            throw new ArgumentOutOfRangeException("The PriorityQueue<T> is empty");
        }

        public bool IsEmpty()
        {
            return backing_collection.Count == 0;
        }

        public T Peek()
        {
            if (!IsEmpty())
            {
                return backing_collection.Values[0];
            }

            throw new ArgumentOutOfRangeException("The PriorityQueue<T> is empty");
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
            backing_collection.Add(item, item);
        }

        public void Clear()
        {
            backing_collection.Clear();
        }

        public bool Contains(T item)
        {
            return backing_collection.ContainsKey(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            return backing_collection.Remove(item);
        }
        #endregion

        #region IEnumerator<T> Support
        public IEnumerator<T> GetEnumerator()
        {
            return backing_collection.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return backing_collection.GetEnumerator();
        }
        #endregion 
    } 
}
