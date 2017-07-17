using System.Text;

namespace tvn.cosine.ai.common.collections
{
    public class Queue<T> : QueueBase<T>, IQueue<T>
    {
        private System.Collections.Generic.List<T> backingList;

        public Queue()
        {
            backingList = new System.Collections.Generic.List<T>();
        }

        public Queue(IQueue<T> items)
            : this()
        {
            AddAll(items);
        }

        public Queue(T[] items)
            : this()
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public bool Add(T item)
        {
            backingList.Add(item);
            return true;
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
            backingList.Clear();
        }

        public bool Contains(T item)
        {
            return backingList.Contains(item);
        }

        public T Get(int index)
        {
            return backingList[index];
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(backingList);
        }

        public int IndexOf(T item)
        {
            return backingList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            backingList.Insert(index, item);
        }

        public bool IsEmpty()
        {
            return backingList.Count == 0;
        }

        public bool IsReadonly()
        {
            return false;
        }

        public T Peek()
        {
            return backingList[0];
        }

        public T Pop()
        {
            T item = backingList[0];
            RemoveAt(0);
            return item;
        }

        public bool Remove(T item)
        {
            return backingList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            backingList.RemoveAt(index);
        }

        public int Size()
        {
            return backingList.Count;
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

            public Enumerator(System.Collections.Generic.List<T> backingList)
            {
                this.values = backingList.ToArray();
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
