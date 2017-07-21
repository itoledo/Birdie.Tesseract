using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace tvn_cosine.ai.v2.common.collections.generic
{
    //TODO: Correctly implement this.
    public class LinkedHashMap<KEY, VALUE> : IDictionary<KEY, VALUE>
    {
        private readonly List<KEY> backingOrderedList = new List<KEY>();
        private readonly IDictionary<KEY, VALUE> backingDictionary = new ConcurrentDictionary<KEY, VALUE>();

        public VALUE this[KEY key]
        {
            get
            {
                return backingDictionary[key];
            }
            set
            {
                if (!backingDictionary.ContainsKey(key))
                {
                    backingOrderedList.Add(key);
                }
                backingDictionary[key] = value;
            }
        }

        public ICollection<KEY> Keys
        {
            get
            {
                return backingOrderedList.ToList().AsReadOnly();
            }
        }

        public ICollection<VALUE> Values
        {
            get
            {
                return backingDictionary.Values;
            }
        }

        public int Count
        {
            get
            {
                return backingDictionary.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(KEY key, VALUE value)
        {
            this[key] = value;
        }

        public void Add(KeyValuePair<KEY, VALUE> item)
        {
            this[item.Key] = item.Value;
        }

        public void Clear()
        {
            backingDictionary.Clear();
            backingOrderedList.Clear();
        }

        public bool Contains(KeyValuePair<KEY, VALUE> item)
        {
            return backingDictionary.Contains(item);
        }

        public bool ContainsKey(KEY key)
        {
            return backingDictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<KEY, VALUE>[] array, int arrayIndex)
        {
            backingDictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<KEY, VALUE>> GetEnumerator()
        {
            return backingDictionary.GetEnumerator();
        }

        public bool Remove(KEY key)
        {
            backingOrderedList.Remove(key);
            return backingDictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<KEY, VALUE> item)
        {
            backingOrderedList.Remove(item.Key);
            return backingDictionary.Remove(item);
        }

        public bool TryGetValue(KEY key, out VALUE value)
        {
            return backingDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
