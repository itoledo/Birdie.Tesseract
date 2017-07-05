using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.util.datastructure 
{
    /// <summary>
    /// Provides a hash map which is indexed by two keys.In fact this is just a hash
    /// map which is indexed by a pair containing the two keys. The provided two-key
    /// access methods try to increase code readability.
    /// </summary>
    /// <typeparam name="K1">First key</typeparam>
    /// <typeparam name="K2">Second key</typeparam>
    /// <typeparam name="V">Result value</typeparam>
    public class TwoKeyDictionary<K1, K2, V> : Dictionary<Pair<K1, K2>, V>
        where K1 : IEquatable<K1>
        where K2 : IEquatable<K2>
    {
        /// <summary>
        /// Associates the specified value with the specified key pair in this map. 
        /// If the map previously contained a mapping for this key pair, the old
        /// value is replaced.
        /// </summary>
        /// <param name="key1">the first key of the key pair, with which the specified value is to be associated.</param>
        /// <param name="key2">the second key of the key pair, with which the specified value is to be associated.</param>
        /// <param name="value">the value to be associated with the key pair.</param>
        public void Add(K1 key1, K2 key2, V value)
        {
            base.Add(new Pair<K1, K2>(key1, key2), value);
        }

        /// <summary>
        /// Returns the value to which the specified key pair is mapped in this two
        /// key hash map, or null if the map contains no mapping for
        /// this key pair.A return value of null does not necessarily 
        /// indicate that the map contains no mapping for the key; it is also
        /// possible that the map explicitly maps the key to null. The
        /// containsKey method may be used to distinguish these two
        /// cases.
        /// </summary>
        /// <param name="key1">the first key of the key pair, whose associated value is to be returned.</param>
        /// <param name="key2">the second key of the key pair, whose associated value is to be returned.</param>
        /// <returns>the value to which this map maps the specified key pair, or null if the map contains no mapping for this key pair.</returns>
        public V this[K1 key1, K2 key2]
        {
            get
            {
                return base[new Pair<K1, K2>(key1, key2)];
            }
        }

        /// <summary>
        /// Returns true if this map contains a mapping for the specified key pair.
        /// </summary>
        /// <param name="key1">the first key of the key pair whose presence in this map is to be tested.</param>
        /// <param name="key2">the second key of the key pair whose presence in this map is to be tested.</param>
        /// <returns>true if this map contains a mapping for the specified key.</returns>
        public bool ContainsKey(K1 key1, K2 key2)
        {
            return ContainsKey(new Pair<K1, K2>(key1, key2));
        }

        /// <summary>
        /// Removes the mapping for this key pair from this map if present.
        /// </summary>
        /// <param name="key1">the first key of the key pair, whose mapping is to be removed from the map.</param>
        /// <param name="key2">the second key of the key pair, whose mapping is to be removed from the map.</param>
        /// <returns>
        /// the previous value associated with the specified key pair, or null if there was no mapping for the key pair. 
        /// A null return can also indicate that the map previously associated null with the specified key pair.
        /// </returns>
        public V RemoveKey(K1 key1, K2 key2)
        {
            var pair = new Pair<K1, K2>(key1, key2);
            var v = base[pair];
            base.Remove(pair);
            return v;
        }
    }
}
