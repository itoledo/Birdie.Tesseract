using System.Text;
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

        public T[] ToArray()
        {
            return backingSet.ToArray();
        }

        public bool ContainsAll(IQueue<T> other)
        {
            foreach (T item in other)
            {
                if (!backingSet.Contains(item))
                {
                    return false;
                }
            }

            return true;
        }

        public bool SequenceEqual(IQueue<T> other)
        {
            return backingSet.SequenceEqual(other);
        }
         
        public T Get(int index)
        {
            return backingSet.Get(index);
        }

        public int IndexOf(T item)
        {
            return backingSet.IndexOf(item);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            bool first = true;
            foreach (var item in this)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(", ");
                }
                sb.Append(item.ToString());
            }
            sb.Append(']');
            return sb.ToString();
        }

        void IQueue<T>.RemoveAt(int index)
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

        void IQueue<T>.Sort(IComparer<T> comparer)
        {
            throw new NotSupportedException("Not supported");
        }

        bool IQueue<T>.Add(T item)
        {
            throw new NotSupportedException("Not supported");
        }

        public bool IsEmpty()
        {
            return backingSet.IsEmpty();
        }

        void IQueue<T>.Clear()
        {
            throw new NotSupportedException("Not supported");
        }

        bool IQueue<T>.Remove(T item)
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

        IQueue<T> IQueue<T>.subList(int startPos, int endPos)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<T>.Set(int position, T item)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
