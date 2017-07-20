using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.common.collections
{
    public class FifoQueueNoDuplicates<T> : QueueBase<T>, IQueue<T>
    {
        private readonly System.Collections.Generic.HashSet<T> elements;
        private readonly System.Collections.Generic.Queue<T> backingQueue;

        public FifoQueueNoDuplicates()
        {
            elements = new System.Collections.Generic.HashSet<T>();
            backingQueue = new System.Collections.Generic.Queue<T>();
        }

        public FifoQueueNoDuplicates(IQueue<T> items)
            : this()
        {
            AddAll(items);
        }

        public bool Add(T item)
        {
            if (!elements.Contains(item))
            {
                elements.Add(item);
                backingQueue.Enqueue(item);
                return true;
            }
            return false;
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
            backingQueue.Clear();
            elements.Clear();
        }

        public bool Contains(T item)
        {
            return elements.Contains(item);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(backingQueue);
        }

        public bool IsEmpty()
        {
            return backingQueue.Count == 0;
        }

        public bool IsReadonly()
        {
            return false;
        }

        public T Peek()
        {
            return backingQueue.Peek();
        }

        public T Pop()
        {
            T obj = backingQueue.Dequeue();
            elements.Remove(obj);
            return obj;
        }

        public int Size()
        {
            return backingQueue.Count;
        }

        void IQueue<T>.Sort(IComparer<T> comparer)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<T>.RemoveAt(int index)
        {
            throw new NotSupportedException("Not supported");
        }

        bool IQueue<T>.Remove(T item)
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

        public bool ContainsAll(IQueue<T> other)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAll(IQueue<T> items)
        {
            throw new System.NotImplementedException();
        }

        public T[] ToArray()
        {
            throw new System.NotImplementedException();
        }

        public void Reverse()
        {
            throw new System.NotImplementedException();
        }

        public IQueue<T> subList(int startPos, int endPos)
        {
            throw new System.NotImplementedException();
        }

        public void Set(int position, T item)
        {
            throw new System.NotImplementedException();
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

            public Enumerator(System.Collections.Generic.Queue<T> backingQueue)
            {
                this.values = backingQueue.ToArray();
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
