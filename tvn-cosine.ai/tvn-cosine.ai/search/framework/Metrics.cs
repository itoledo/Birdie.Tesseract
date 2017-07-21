using System.Globalization;
using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.search.framework
{
    /**
     * Stores key-value pairs for efficiency analysis. 
     */
    public class Metrics : IStringable
    {
        private IMap<string, string> hash;

        public Metrics()
        {
            this.hash = Factory.CreateInsertionOrderedMap<string, string>();
        }

        public void set(string name, int i)
        {
            hash.Put(name, i.ToString());
        }

        public void set(string name, double d)
        {
            hash.Put(name, d.ToString());
        }

        public void incrementInt(string name)
        {
            set(name, getInt(name) + 1);
        }

        public void set(string name, long l)
        {
            hash.Put(name, l.ToString());
        }

        public int getInt(string name)
        {
            return hash.ContainsKey(name) ? int.Parse(hash.Get(name), NumberStyles.Any, CultureInfo.InvariantCulture) : 0;
        }

        public double getDouble(string name)
        {
            return hash.ContainsKey(name) ? double.Parse(hash.Get(name), NumberStyles.Any, CultureInfo.InvariantCulture) : double.NaN;
        }

        public long getLong(string name)
        { 
            return hash.ContainsKey(name) ? long.Parse(hash.Get(name), NumberStyles.Any, CultureInfo.InvariantCulture) : 0L;
        }

        public string get(string name)
        {
            return hash.Get(name);
        }

        public ISet<string> keySet()
        {
            return Factory.CreateSet<string>(hash.GetKeys());
        }

        /** Sorts the key-value pairs by key names and formats them as equations. */
        public override string ToString()
        {
            IMap<string, string> map = Factory.CreateTreeMap<string, string>(hash);
            return map.ToString();
        }
    }
}
