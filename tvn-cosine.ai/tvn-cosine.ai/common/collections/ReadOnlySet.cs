using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.common.collections
{
    public class ReadOnlySet<T> : ISet<T>
    {
        private readonly ISet<T> backingSet;

        public ReadOnlySet(ISet<T> backingSet)
        {
            this.backingSet = backingSet;
        }

        public ReadOnlySet(IQueue<T> backingQueue)
        {
            this.backingSet = new Set<T>(backingQueue); 
        }

        public bool IsReadonly()
        {
            return true;
        }

        public int Size()
        {
            return backingSet.Size();
        }

        public bool Contains(T item)
        {
            return backingSet.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return backingSet.GetEnumerator();
        }

        void IQueue<T>.RemoveAt(int index)
        {
            throw new NotSupportedException("Not supported");
        }

        T IQueue<T>.Get(int index)
        {
            throw new NotSupportedException("Not supported");
        }

        int IQueue<T>.IndexOf(T item)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<T>.Insert(int index, T item)
        {
            throw new NotSupportedException("Not supported");
        }

        T IQueue<T>.Peek()
        {
            throw new NotSupportedException("Not supported");
        }

        T IQueue<T>.Pop()
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<T>.AddAll(IQueue<T> items)
        {
            throw new NotSupportedException();
        }

        bool IQueue<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        public bool IsEmpty()
        {
            return backingSet.IsEmpty();
        }
         
        void IQueue<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool IQueue<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }
    }
}
