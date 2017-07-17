using System.Linq;
using System.Text;
using tvn.cosine.ai.common.datastructures;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.common.collections
{
    public class Map<KEY, VALUE> : IMap<KEY, VALUE> , IToString, IHashable 
    {
        private readonly System.Collections.Generic.IDictionary<KEY, VALUE> backingMap;

        public Map()
        {
            backingMap = new System.Collections.Generic.Dictionary<KEY, VALUE>();
        }

        public Map(IQueue<KeyValuePair<KEY, VALUE>> items)
            : this()
        {
            AddAll(items);
        }

        public bool Add(KeyValuePair<KEY, VALUE> item)
        {
            backingMap.Add(item.GetKey(), item.GetValue());
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
            backingMap.Clear();
        }

        public bool Contains(KeyValuePair<KEY, VALUE> item)
        {
            return backingMap.ContainsKey(item.GetKey())
                && backingMap[item.GetKey()].Equals(item.GetValue());
        }

        public bool ContainsKey(KEY key)
        {
            return backingMap.ContainsKey(key);
        }

        public bool Equals(IMap<KEY, VALUE> other)
        {
            if (null == other) return false;
            if (Size() != other.Size()) return false;

            foreach (var item in other)
            {
                if (!Contains(item))
                {
                    return false;
                }
            }

            return true;
        }


        public VALUE Get(KEY key)
        {
            if (backingMap.ContainsKey(key))
                return backingMap[key];
            else
                return default(VALUE);
        }

        public IEnumerator<KeyValuePair<KEY, VALUE>> GetEnumerator()
        {
            return new Enumerator(backingMap);
        }

        public IQueue<KEY> GetKeys()
        {
            IQueue<KEY> obj = Factory.CreateSet<KEY>();
            foreach (KEY key in backingMap.Keys)
            {
                obj.Add(key);
            }
            return obj;
        }

        public IQueue<VALUE> GetValues()
        {
            IQueue<VALUE> obj = Factory.CreateFifoQueue<VALUE>();
            foreach (VALUE value in backingMap.Values)
            {
                obj.Add(value);
            }
            return obj;
        }

        public bool IsEmpty()
        {
            return backingMap.Count == 0;
        }

        public bool IsReadonly()
        {
            return false;
        }

        public void Put(KEY key, VALUE value)
        {
            backingMap[key] = value;
        }

        public bool Remove(KeyValuePair<KEY, VALUE> item)
        {
            if (backingMap.ContainsKey(item.GetKey())
             && backingMap[item.GetKey()].Equals(item.GetValue()))
            {
                Remove(item.GetKey());
            }
            return false;
        }

        public bool Remove(KEY key)
        {
            return backingMap.Remove(key);
        }

        public int Size()
        {
            return backingMap.Count;
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
