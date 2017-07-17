using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.common.collections
{
    public class LifoQueue<T> : QueueBase<T>, IQueue<T>
    {
        private readonly System.Collections.Generic.Stack<T> backingStack;

        public LifoQueue()
        {
            backingStack = new System.Collections.Generic.Stack<T>();
        }

        public LifoQueue(IQueue<T> items)
            : this()
        {
            AddAll(items);
        }

        public bool Add(T item)
        {
            backingStack.Push(item);
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
            backingStack.Clear();
        }

        public bool Contains(T item)
        {
            return backingStack.Contains(item);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(backingStack);
        }

        public bool IsEmpty()
        {
            return backingStack.Count == 0;
        }

        public bool IsReadonly()
        {
            return false;
        }

        public T Peek()
        {
            return backingStack.Peek();
        }

        public T Pop()
        {
            return backingStack.Pop();
        }

        public int Size()
        {
            return backingStack.Count;
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

            public Enumerator(System.Collections.Generic.Stack<T> backingStack)
            {
                this.values = backingStack.ToArray();
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
