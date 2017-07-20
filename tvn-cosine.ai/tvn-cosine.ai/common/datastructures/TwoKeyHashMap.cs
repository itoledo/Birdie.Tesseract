using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.common.datastructures
{
    /**
     * Provides a hash map which is indexed by two keys. In fact this is just a hash
     * map which is indexed by a pair containing the two keys. The provided two-key
     * access methods try to increase code readability.
     * 
     * @param <K1>
     *            First key
     * @param <K2>
     *            Second key
     * @param <V>
     *            Result value
     * 
     * @author Ruediger Lunde
     * @author Mike Stampone
     */
    public class TwoKeyHashMap<K1, K2, V> : IMap<Pair<K1, K2>, V>
    {
        private readonly IMap<Pair<K1, K2>, V> backingMap = Factory.CreateMap<Pair<K1, K2>, V>();

        public bool IsReadonly()
        {
            return backingMap.IsReadonly();
        }

        public V Get(K1 key1, K2 key2)
        {
            return backingMap.Get(new Pair<K1, K2>(key1, key2));
        }

        public V Get(Pair<K1, K2> key)
        {
            return backingMap.Get(key);
        }

        public IQueue<Pair<K1, K2>> GetKeys()
        {
            return backingMap.GetKeys();
        }

        public IQueue<V> GetValues()
        {
            return backingMap.GetValues();
        }

        public void Put(K1 key1, K2 key2, V value)
        {
            backingMap.Put(new Pair<K1, K2>(key1, key2), value);
        }

        public void Put(Pair<K1, K2> key, V value)
        {
            backingMap.GetValues();
        }

        public bool ContainsKey(K1 key1, K2 key2)
        {
            return backingMap.ContainsKey(new Pair<K1, K2>(key1, key2));
        }

        public bool ContainsKey(Pair<K1, K2> key)
        {
            return backingMap.ContainsKey(key);
        }

        public bool Remove(K1 key1, K2 key2)
        {
            return backingMap.Remove(new Pair<K1, K2>(key1, key2));
        }

        public bool Remove(Pair<K1, K2> key)
        {
            return backingMap.Remove(key);
        }

        public KeyValuePair<Pair<K1, K2>, V> Get(int index)
        {
            return backingMap.Get(index);
        }

        public int IndexOf(KeyValuePair<Pair<K1, K2>, V> item)
        {
            return backingMap.IndexOf(item);
        }

        public void Insert(int index, KeyValuePair<Pair<K1, K2>, V> item)
        {
            backingMap.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            backingMap.RemoveAt(index);
        }

        public void AddAll(IQueue<KeyValuePair<Pair<K1, K2>, V>> items)
        {
            backingMap.AddAll(items);
        }

        public bool Add(KeyValuePair<Pair<K1, K2>, V> item)
        {
            return backingMap.Add(item);
        }

        public bool IsEmpty()
        {
            return backingMap.IsEmpty();
        }

        public int Size()
        {
            return backingMap.Size();
        }

        public KeyValuePair<Pair<K1, K2>, V> Pop()
        {
            return backingMap.Pop();
        }

        public KeyValuePair<Pair<K1, K2>, V> Peek()
        {
            return backingMap.Peek();
        }


        public void Clear()
        {
            backingMap.Clear();
        }

        public bool Contains(KeyValuePair<Pair<K1, K2>, V> item)
        {
            return backingMap.Contains(item);
        }

        public bool Remove(KeyValuePair<Pair<K1, K2>, V> item)
        {
            return backingMap.Remove(item);
        }

        public IEnumerator<KeyValuePair<Pair<K1, K2>, V>> GetEnumerator()
        {
            return backingMap.GetEnumerator();
        }

        public bool Equals(IMap<Pair<K1, K2>, V> other)
        {
            return backingMap.Equals(other);
        }

        public bool SequenceEqual(IQueue<KeyValuePair<Pair<K1, K2>, V>> queue)
        {
            return backingMap.SequenceEqual(queue);
        }

        void IMap<Pair<K1, K2>, V>.PutAll(IMap<Pair<K1, K2>, V> map)
        {
            throw new NotSupportedException("Not supported");
        }

        bool IQueue<KeyValuePair<Pair<K1, K2>, V>>.ContainsAll(IQueue<KeyValuePair<Pair<K1, K2>, V>> other)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<KeyValuePair<Pair<K1, K2>, V>>.RemoveAll(IQueue<KeyValuePair<Pair<K1, K2>, V>> items)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<KeyValuePair<Pair<K1, K2>, V>>.Sort(IComparer<KeyValuePair<Pair<K1, K2>, V>> comparer)
        {
            throw new NotSupportedException("Not supported");
        }

        public KeyValuePair<Pair<K1, K2>, V>[] ToArray()
        {
            return backingMap.ToArray();
        }

        void IQueue<KeyValuePair<Pair<K1, K2>, V>>.Reverse()
        {
            throw new NotSupportedException("Not supported");
        }

        IQueue<KeyValuePair<Pair<K1, K2>, V>> IQueue<KeyValuePair<Pair<K1, K2>, V>>.subList(int startPos, int endPos)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<KeyValuePair<Pair<K1, K2>, V>>.Set(int position, KeyValuePair<Pair<K1, K2>, V> item)
        {
            throw new NotSupportedException("Not supported");
        } 
    }
}
