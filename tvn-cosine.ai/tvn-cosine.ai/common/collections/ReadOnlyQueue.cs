using System;

namespace tvn.cosine.ai.common.collections
{
    public class ReadOnlyQueue<T> : IQueue<T>
    {
        private readonly IQueue<T> backingQueue;

        public ReadOnlyQueue(IQueue<T> backingQueue)
        {
            this.backingQueue = backingQueue;
        }
         
        public bool Contains(T item)
        {
            return backingQueue.Contains(item);
        }

        public T Get(int index)
        {
            return backingQueue.Get(index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return backingQueue.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return backingQueue.IndexOf(item);
        }
         
        public bool IsEmpty()
        {
            return backingQueue.IsEmpty();
        }

        public bool IsReadonly()
        {
            return true;
        }

        public T Peek()
        {
            return backingQueue.Peek();
        }

        public int Size()
        {
            return backingQueue.Size();
        }

        T IQueue<T>.Pop()
        {
            throw new NotSupportedException();
        }
         
        bool IQueue<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        void IQueue<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        bool IQueue<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        void IQueue<T>.AddAll(IQueue<T> items)
        {
            throw new NotSupportedException();
        }

        void IQueue<T>.Clear()
        {
            throw new NotSupportedException();
        }

        void IQueue<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }
    }
}
