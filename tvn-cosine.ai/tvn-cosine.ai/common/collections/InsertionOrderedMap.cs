﻿using System.Text;
using tvn.cosine.ai.common.datastructures;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.common.collections
{
    public class InsertionOrderedMap<KEY, VALUE> : IMap<KEY, VALUE>, IStringable, IHashable
    {
        private readonly System.Collections.Generic.IDictionary<KEY, VALUE> backingDictionary;
        private readonly IQueue<KEY> orderedQueue = Factory.CreateQueue<KEY>();

        public InsertionOrderedMap()
        {
            backingDictionary = new System.Collections.Generic.Dictionary<KEY, VALUE>();
        }
          
        public InsertionOrderedMap(IEqualityComparer<KEY> comparer)
        {
            backingDictionary = new System.Collections.Generic.Dictionary<KEY, VALUE>(new QueueBase<KEY>.EqualityComparerAdapter(comparer));
        }

        public InsertionOrderedMap(IQueue<KeyValuePair<KEY, VALUE>> items)
            : this()
        {
            AddAll(items);
        }

        public InsertionOrderedMap(IQueue<KeyValuePair<KEY, VALUE>> items, IEqualityComparer<KEY> comparer)
            : this(comparer)
        {
            AddAll(items);
        }


        public bool Add(KeyValuePair<KEY, VALUE> item)
        {
            if (!backingDictionary.ContainsKey(item.GetKey()))
            {
                orderedQueue.Add(item.GetKey());
            }
            backingDictionary[item.GetKey()] = item.GetValue();
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
            orderedQueue.Clear();
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
            return new Enumerator(this);
        }

        public ISet<KEY> GetKeys()
        {
            return Factory.CreateReadOnlySet<KEY>(orderedQueue);
        }

        public IQueue<VALUE> GetValues()
        {
            IQueue<VALUE> obj = Factory.CreateQueue<VALUE>();
            foreach (KEY key in orderedQueue)
            {
                obj.Add(backingDictionary[key]);
            }
            return Factory.CreateReadOnlyQueue<VALUE>(obj);
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
            if (!backingDictionary.ContainsKey(key))
            {
                orderedQueue.Add(key);
            }
            backingDictionary[key] = value;
        }

        public bool Remove(KeyValuePair<KEY, VALUE> item)
        {
            if (backingDictionary.ContainsKey(item.GetKey())
             && backingDictionary[item.GetKey()].Equals(item.GetValue()))
            {
                orderedQueue.Remove(item.GetKey());
                Remove(item.GetKey());
            }
            return false;
        }

        public bool Remove(KEY key)
        {
            orderedQueue.Remove(key);
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
                if (!backingDictionary.ContainsKey(pair.GetKey()))
                {
                    orderedQueue.Add(pair.GetKey());
                }
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
                    orderedQueue.Remove(pair.GetKey());
                    backingDictionary.Remove(pair.GetKey());
                }
            }
        }

        public KeyValuePair<KEY, VALUE>[] ToArray()
        {
            KeyValuePair<KEY, VALUE>[] obj = new KeyValuePair<KEY, VALUE>[backingDictionary.Count];
            int count = 0;
            foreach (KEY key in orderedQueue)
            {
                obj[count] = new KeyValuePair<KEY, VALUE>(key, backingDictionary[key]);
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

        bool IQueue<KeyValuePair<KEY, VALUE>>.SequenceEqual(IQueue<KeyValuePair<KEY, VALUE>> queue)
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
            private readonly IQueue<KEY> orderedQueue;
            private readonly InsertionOrderedMap<KEY, VALUE> backingMap;

            private int position = -1;

            public KeyValuePair<KEY, VALUE> Current
            {
                get
                {
                    return GetCurrent();
                }
            }

            public Enumerator(InsertionOrderedMap<KEY, VALUE> backingMap)
            {
                this.backingMap = backingMap;
                this.orderedQueue = backingMap.orderedQueue;
            }

            public KeyValuePair<KEY, VALUE> GetCurrent()
            {
                KEY key = orderedQueue.Get(position);
                return new KeyValuePair<KEY, VALUE>(key, backingMap.Get(key));
            }

            public bool MoveNext()
            {
                ++position;
                return (position < orderedQueue.Size());
            }

            public void Reset()
            {
                position = -1;
            }
        }
    }
}