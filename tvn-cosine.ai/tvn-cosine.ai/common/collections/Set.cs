using System.Linq;
using System.Text;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.common.collections
{
    public class Set<T> : ISet<T>
    {
        protected readonly System.Collections.Generic.ISet<T> backingSet;

        public Set()
        {
            backingSet = new System.Collections.Generic.HashSet<T>();
        }

        public Set(params T[] items)
            : this()
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public Set(IQueue<T> items)
            : this()
        {
            AddAll(items);
        }

        void IQueue<T>.Sort(IComparer<T> comparer)
        {
            throw new NotSupportedException("Not supported");
        }

        public bool Add(T item)
        {
            return backingSet.Add(item);
        }

        public void AddAll(IQueue<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            backingSet.Clear();
        }

        public bool Contains(T item)
        {
            return backingSet.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(backingSet);
        }

        public bool IsEmpty()
        {
            return backingSet.Count == 0;
        }

        public bool IsReadonly()
        {
            return false;
        }

        public bool Remove(T item)
        {
            return backingSet.Remove(item);
        }

        public int Size()
        {
            return backingSet.Count;
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

        public void RemoveAll(IQueue<T> items)
        {
            foreach (T item in items)
            {
                backingSet.Remove(item);
            }
        }

        public T[] ToArray()
        {
            return backingSet.ToArray();
        }

        void IQueue<T>.RemoveAt(int index)
        {
            throw new NotSupportedException("Not supported");
        }

        T IQueue<T>.Get(int index)
        {
            if (index > backingSet.Count
             || index < 0)
            {
                throw new NotSupportedException("Not supported");
            }

            int counter = 0;

            foreach (T item in backingSet)
            {
                if (counter == index)
                {
                    return item;
                }
                ++counter;
            }

            return default(T);
        }

        public bool SequenceEqual(IQueue<T> other)
        {
            if (null == other
             || other.Size() != Size())
            {
                return false;
            }
             
            foreach (T item in backingSet)
            {
                if (!other.Contains(item))
                {
                    return false;
                } 
            }
            return true;
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

        class Enumerator : IEnumerator<T>
        {
            private readonly T[] values;

            private int position = -1;

            public T Current
            {
                get
                {
                    return GetCurrent();
                }
            }

            public Enumerator(System.Collections.Generic.ISet<T> backingSet)
            {
                this.values = backingSet.ToArray();
            }

            public T GetCurrent()
            {
                return values[position];
            }

            public void Dispose()
            { }

            public bool MoveNext()
            {
                ++position;
                return (position < values.Length);
            }

            public void Reset()
            {
                position = -1;
            }
        }
    }
}
