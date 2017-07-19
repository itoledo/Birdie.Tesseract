using tvn.cosine.ai.common.datastructures;

namespace tvn.cosine.ai.common.collections
{
    public interface IMap<KEY, VALUE> : IQueue<KeyValuePair<KEY, VALUE>>, 
        IEnumerable<KeyValuePair<KEY, VALUE>>, IHashable, 
        IToString, IEquatable, IEquatable<IMap<KEY, VALUE>>
    {
        VALUE Get(KEY key);
        IQueue<KEY> GetKeys();
        IQueue<VALUE> GetValues();
       
        void Put(KEY key, VALUE value);
        void PutAll(IMap<KEY, VALUE> map);
        bool ContainsKey(KEY key); 
        bool Remove(KEY key);  
    }
}
