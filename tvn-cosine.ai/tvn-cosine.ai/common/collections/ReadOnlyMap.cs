using tvn.cosine.ai.common.datastructures;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.common.collections
{
    public class ReadOnlyMap<KEY, VALUE> : IMap<KEY, VALUE>
    {
        private readonly IMap<KEY, VALUE> backingMap;

        public ReadOnlyMap(IMap<KEY, VALUE> backingMap)
        {
            this.backingMap = backingMap;
        }

        public int Size()
        {
            return backingMap.Size();
        }

        public bool Contains(KeyValuePair<KEY, VALUE> item)
        {
            return backingMap.Contains(item);
        }

        public bool ContainsKey(KEY key)
        {
            return backingMap.ContainsKey(key);
        }

        public bool Equals(IMap<KEY, VALUE> other)
        {
            return backingMap.Equals(other);
        }

        public VALUE Get(KEY key)
        {
            return backingMap.Get(key);
        }

        public IEnumerator<KeyValuePair<KEY, VALUE>> GetEnumerator()
        {
            return backingMap.GetEnumerator();
        }

        public IQueue<KEY> GetKeys()
        {
            return backingMap.GetKeys();
        }

        public IQueue<VALUE> GetValues()
        {
            return backingMap.GetValues();
        }

        public bool IsEmpty()
        {
            return backingMap.IsEmpty();
        }

        public bool IsReadonly()
        {
            return true;
        }

        void IMap<KEY, VALUE>.Put(KEY key, VALUE value)
        {
            throw new NotSupportedException("Not supported");
        }

        bool IQueue<KeyValuePair<KEY, VALUE>>.Remove(KeyValuePair<KEY, VALUE> item)
        {
            throw new NotSupportedException("Not supported");
        }

        bool IMap<KEY, VALUE>.Remove(KEY key)
        {
            throw new NotSupportedException("Not supported");
        }

        bool IQueue<KeyValuePair<KEY, VALUE>>.Add(KeyValuePair<KEY, VALUE> item)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<KeyValuePair<KEY, VALUE>>.AddAll(IQueue<KeyValuePair<KEY, VALUE>> items)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<KeyValuePair<KEY, VALUE>>.Clear()
        {
            throw new NotSupportedException("Not supported");
        }

        KeyValuePair<KEY, VALUE> IQueue<KeyValuePair<KEY, VALUE>>.Get(int index)
        {
            throw new NotSupportedException("Not supported");
        }

        int IQueue<KeyValuePair<KEY, VALUE>>.IndexOf(KeyValuePair<KEY, VALUE> item)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<KeyValuePair<KEY, VALUE>>.Insert(int index, KeyValuePair<KEY, VALUE> item)
        {
            throw new NotSupportedException("Not supported");
        }

        KeyValuePair<KEY, VALUE> IQueue<KeyValuePair<KEY, VALUE>>.Peek()
        {
            throw new NotSupportedException("Not supported");
        }

        KeyValuePair<KEY, VALUE> IQueue<KeyValuePair<KEY, VALUE>>.Pop()
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<KeyValuePair<KEY, VALUE>>.RemoveAt(int index)
        {
            throw new NotSupportedException("Not supported");
        }
    }
}
