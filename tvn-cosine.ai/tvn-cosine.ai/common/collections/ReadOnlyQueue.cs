using tvn.cosine.ai.common.exceptions;

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

        public bool SequenceEqual(IQueue<T> other)
        {
            return backingQueue.SequenceEqual(other);
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

        public T[] ToArray()
        {
            return backingQueue.ToArray();
        }

        public bool ContainsAll(IQueue<T> other)
        {
            return backingQueue.ContainsAll(other);
        }

        public IQueue<T> subList(int startPos, int endPos)
        {
            return backingQueue.subList(startPos, endPos);
        }

        void IQueue<T>.Sort(IComparer<T> comparer)
        {
            throw new NotSupportedException("Not supported");
        }

        T IQueue<T>.Pop()
        {
            throw new NotSupportedException("Not supported");
        }

        bool IQueue<T>.Remove(T item)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<T>.RemoveAt(int index)
        {
            throw new NotSupportedException("Not supported");
        }

        bool IQueue<T>.Add(T item)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<T>.AddAll(IQueue<T> items)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<T>.Clear()
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<T>.Insert(int index, T item)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<T>.RemoveAll(IQueue<T> items)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<T>.Reverse()
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<T>.Set(int position, T item)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
