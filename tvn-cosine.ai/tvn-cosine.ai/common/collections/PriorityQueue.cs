using System.Linq;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.common.collections
{
    public class PriorityQueue<T> : QueueBase<T>, IQueue<T>
    {
        private readonly System.Collections.Generic.SortedList<T, T> backingSortedList;
        private readonly IComparer<T> comparer;

        public PriorityQueue(IComparer<T> comparer)
        {
            this.comparer = comparer;
            backingSortedList = new System.Collections.Generic.SortedList<T, T>(new ComparerAdaptor(comparer));
        }

        public bool Add(T item)
        {
            if (!backingSortedList.ContainsKey(item))
            {
                backingSortedList.Add(item, item);
                return true;
            }
            return false;
        }

        public void AddAll(IQueue<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public IComparer<T> GetComparer()
        {
            return comparer;
        }

        public void Clear()
        {
            backingSortedList.Clear();
        }

        public bool Contains(T item)
        {
            return backingSortedList.ContainsKey(item);
        }

        public T Get(int index)
        {
            return backingSortedList.Keys[index];
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(backingSortedList);
        }

        public int IndexOf(T item)
        {
            return backingSortedList.IndexOfKey(item);
        }

        public bool IsEmpty()
        {
            return backingSortedList.Count == 0;
        }

        public bool IsReadonly()
        {
            return false;
        }

        public T Peek()
        {
            return backingSortedList.Keys[0];
        }

        public T Pop()
        {
            T obj = backingSortedList.Keys[0];
            backingSortedList.Remove(obj);
            return obj;
        }

        public bool Remove(T item)
        {
            return backingSortedList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            backingSortedList.RemoveAt(index);
        }

        public int Size()
        {
            return backingSortedList.Count();
        }

        void IQueue<T>.Sort(IComparer<T> comparer)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<T>.Insert(int index, T item)
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

            public Enumerator(System.Collections.Generic.SortedList<T, T> backingSortedList)
            {
                this.values = backingSortedList.Keys.ToArray();
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
