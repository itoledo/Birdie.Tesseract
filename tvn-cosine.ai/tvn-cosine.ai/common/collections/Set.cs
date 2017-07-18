using System.Linq;
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
