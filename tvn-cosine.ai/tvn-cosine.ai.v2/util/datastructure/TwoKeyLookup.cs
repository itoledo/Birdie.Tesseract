using System.Collections.Generic;
using tvn_cosine.ai.v2.common.collections.generic;

namespace aima.core.util.datastructure
{/**
 * Provides a lookup which is indexed by two keys.
 * 
 * @param <K1>
 *            First key
 * @param <K2>
 *            Second key
 * @param <V>
 *            Result value
 * 
 * @author Ruediger Lunde
 * @author Ciaran O'Reilly
 * @author Mike Stampone
 */
    public class TwoKeyLookup<K1, K2, V>
    {
        private IDictionary<K1, IDictionary<K2, V>> k1Map = new LinkedHashMap<K1, IDictionary<K2, V>>();

        /**
         * Associates the specified value with the specified key pair in this map.
         * If the map previously contained a mapping for this key pair, the old
         * value is replaced.
         * 
         * @param key1
         *            the first key of the key pair, with which the specified value
         *            is to be associated.
         * @param key2
         *            the second key of the key pair, with which the specified value
         *            is to be associated.
         * @param value
         *            the value to be associated with the key pair.
         * 
         */
        public void put(K1 key1, K2 key2, V value)
        {
            if (!k1Map.ContainsKey(key1))
            {
                k1Map[key1] = new LinkedHashMap<K2, V>();
            }
            k1Map[key1][key2] = value;
        }

        /**
         * Returns the value to which the specified key pair is mapped in this two
         * key hash map, or <code>null</code> if the map contains no mapping for
         * this key pair. A return value of null does not <em>necessarily</em>
         * indicate that the map contains no mapping for the key; it is also
         * possible that the map explicitly maps the key to <code>null</code>. The
         * <code>containsKey</code> method may be used to distinguish these two
         * cases.
         * 
         * @param key1
         *            the first key of the key pair, whose associated value is to be
         *            returned.
         * @param key2
         *            the second key of the key pair, whose associated value is to
         *            be returned.
         * 
         * @return the value to which this map maps the specified key pair, or
         *         <code>null</code> if the map contains no mapping for this key
         *         pair.
         */
        public V get(K1 key1, K2 key2)
        {
            if (containsKey(key1, key2))
            {
                return k1Map[key1][key2];
            }
            return default(V);
        }

        /**
         * Returns <code>true</code> if this map contains a mapping for the
         * specified key pair.
         * 
         * @param key1
         *            the first key of the key pair whose presence in this map is to
         *            be tested.
         * @param key2
         *            the second key of the key pair whose presence in this map is
         *            to be tested.
         * 
         * @return <code>true</code> if this map contains a mapping for the
         *         specified key.
         */
        public bool containsKey(K1 key1, K2 key2)
        {
            return k1Map.ContainsKey(key1)
                && k1Map[key1].ContainsKey(key2);
        }

        /**
         * Removes the mapping for this key pair from this map if present.
         * 
         * @param key1
         *            the first key of the key pair, whose mapping is to be removed
         *            from the map.
         * @param key2
         *            the second key of the key pair, whose mapping is to be removed
         *            from the map.
         * 
         * @return the previous value associated with the specified key pair, or
         *         <code>null</code> if there was no mapping for the key pair. A
         *         <code>null</code> return can also indicate that the map
         *         previously associated <code>null</code> with the specified key
         *         pair.
         */
        public V removeKey(K1 key1, K2 key2)
        {
            V value = get(key1, key2);
            if (k1Map.ContainsKey(key1))
            {
                if (k1Map[key1].ContainsKey(key2))
                {
                    k1Map[key1].Remove(key2);
                }
                k1Map.Remove(key1);
            }
            return value;
        }

        /**
         * Get the entry set associated with the first key of the lookup.
         * 
         * @param key1
         *            the first key of the lookup.
         * @return the entry set of K2 and V pairs contained within K1.
         */
        public ISet<KeyValuePair<K2, V>> getEntrySetForK1(K1 key1)
        {
            if (k1Map.ContainsKey(key1))
            {
                return new HashSet<KeyValuePair<K2, V>>(k1Map[key1]);
            }
            return null;
        }
    }
}