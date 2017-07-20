using System.Linq;
using System.Text;
using tvn.cosine.ai.common.datastructures;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.common.collections
{
    public class Map<KEY, VALUE> : IMap<KEY, VALUE>, IStringable, IHashable
    {
        private readonly System.Collections.Generic.IDictionary<KEY, VALUE> backingDictionary;

        public Map()
        {
            backingDictionary = new System.Collections.Generic.Dictionary<KEY, VALUE>();
        }

        public Map(IEqualityComparer<KEY> comparer)
        {
            backingDictionary = new System.Collections.Generic.Dictionary<KEY, VALUE>(new QueueBase<KEY>.EqualityComparerAdapter(comparer));
        }

        public Map(IQueue<KeyValuePair<KEY, VALUE>> items)
            : this()
        {
            AddAll(items);
        }

        public Map(IQueue<KeyValuePair<KEY, VALUE>> items, IEqualityComparer<KEY> comparer)
            : this(comparer)
        {
            AddAll(items);
        }

        public bool Add(KeyValuePair<KEY, VALUE> item)
        {
            backingDictionary.Add(item.GetKey(), item.GetValue());
            return true;
        }

        public void AddAll(IQueue<KeyValuePair<KEY, VALUE>> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Clear()
        {
            backingDictionary.Clear();
        }

        public bool Contains(KeyValuePair<KEY, VALUE> item)
        {
            return backingDictionary.ContainsKey(item.GetKey())
                && backingDictionary[item.GetKey()].Equals(item.GetValue());
        }

        public bool ContainsKey(KEY key)
        {
            return backingDictionary.ContainsKey(key);
        }
         
        public VALUE Get(KEY key)
        {
            if (backingDictionary.ContainsKey(key))
                return backingDictionary[key];
            else
                return default(VALUE);
        }

        public IEnumerator<KeyValuePair<KEY, VALUE>> GetEnumerator()
        {
            return new Enumerator(backingDictionary);
        }

        public IQueue<KEY> GetKeys()
        {
            IQueue<KEY> obj = Factory.CreateSet<KEY>();
            foreach (KEY key in backingDictionary.Keys)
            {
                obj.Add(key);
            }
            return obj;
        }

        public IQueue<VALUE> GetValues()
        {
            IQueue<VALUE> obj = Factory.CreateFifoQueue<VALUE>();
            foreach (VALUE value in backingDictionary.Values)
            {
                obj.Add(value);
            }
            return obj;
        }

        public bool IsEmpty()
        {
            return backingDictionary.Count == 0;
        }

        public bool IsReadonly()
        {
            return false;
        }

        public void Put(KEY key, VALUE value)
        {
            backingDictionary[key] = value;
        }

        public bool Remove(KeyValuePair<KEY, VALUE> item)
        {
            if (backingDictionary.ContainsKey(item.GetKey())
             && backingDictionary[item.GetKey()].Equals(item.GetValue()))
            {
                Remove(item.GetKey());
            }
            return false;
        }

        public bool Remove(KEY key)
        {
            return backingDictionary.Remove(key);
        }

        public int Size()
        {
            return backingDictionary.Count;
        }

        public void PutAll(IMap<KEY, VALUE> map)
        {
            foreach (KeyValuePair<KEY, VALUE> pair in map)
            {
                backingDictionary[pair.GetKey()] = pair.GetValue();
            }
        }

        public bool ContainsAll(IQueue<KeyValuePair<KEY, VALUE>> other)
        {
            foreach (KeyValuePair<KEY, VALUE> pair in other)
            {
                if (!(backingDictionary.ContainsKey(pair.GetKey())
                    && backingDictionary[pair.GetKey()].Equals(pair.GetValue())))
                {
                    return false;
                }
            }

            return true;
        }

        public void RemoveAll(IQueue<KeyValuePair<KEY, VALUE>> items)
        {
            foreach (KeyValuePair<KEY, VALUE> pair in items)
            {
                if (backingDictionary.ContainsKey(pair.GetKey())
                    && backingDictionary[pair.GetKey()].Equals(pair.GetValue()))
                {
                    backingDictionary.Remove(pair.GetKey());
                }
            }
        }

        public KeyValuePair<KEY, VALUE>[] ToArray()
        {
            KeyValuePair<KEY, VALUE>[] obj = new KeyValuePair<KEY, VALUE>[backingDictionary.Count];
            int count = 0;
            foreach (KeyValuePair<KEY, VALUE> pair in this)
            {
                obj[count] = pair;
                ++count;
            }

            return obj;
        }

        IQueue<KeyValuePair<KEY, VALUE>> IQueue<KeyValuePair<KEY, VALUE>>.subList(int startPos, int endPos)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<KeyValuePair<KEY, VALUE>>.Set(int position, KeyValuePair<KEY, VALUE> item)
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<KeyValuePair<KEY, VALUE>>.Reverse()
        {
            throw new NotSupportedException("Not supported");
        }

        void IQueue<KeyValuePair<KEY, VALUE>>.Sort(IComparer<KeyValuePair<KEY, VALUE>> comparer)
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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            sb.Append('[');
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
                sb.Append('[');
                sb.Append(item.GetKey().ToString());
                sb.Append(", ");
                sb.Append(item.GetValue().ToString());
                sb.Append(']');
            }

            sb.Append(']');
            return sb.ToString();
        }

        class Enumerator : IEnumerator<KeyValuePair<KEY, VALUE>>
        {
            private readonly System.Collections.Generic.KeyValuePair<KEY, VALUE>[] keyValuePairs;

            private int position = -1;

            public KeyValuePair<KEY, VALUE> Current
            {
                get
                {
                    return GetCurrent();
                }
            }

            public Enumerator(System.Collections.Generic.IDictionary<KEY, VALUE> backingMap)
            {
                this.keyValuePairs = backingMap.ToArray();
            }

            public KeyValuePair<KEY, VALUE> GetCurrent()
            {
                System.Collections.Generic.KeyValuePair<KEY, VALUE> current = keyValuePairs[position];
                return new KeyValuePair<KEY, VALUE>(current.Key, current.Value);
            }

            public bool MoveNext()
            {
                ++position;
                return (position < keyValuePairs.Length);
            }

            public void Reset()
            {
                position = -1;
            }
        }
    }
}
