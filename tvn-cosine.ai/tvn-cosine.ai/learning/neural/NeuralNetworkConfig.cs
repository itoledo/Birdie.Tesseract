using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.learning.neural
{
    /// <summary>
    /// A holder for config data for neural networks and possibly for other learning systems.
    /// </summary>
    public class NeuralNetworkConfig
    {
        private readonly IMap<string, object> hash;

        public NeuralNetworkConfig(IMap<string, object> hash)
        {
            this.hash = hash;
        }

        public NeuralNetworkConfig()
        {
            this.hash = CollectionFactory.CreateInsertionOrderedMap<string, object>();
        }

        public double getParameterAsDouble(string key)
        {

            return (double)hash.Get(key);
        }

        public int getParameterAsInteger(string key)
        {

            return (int)hash.Get(key);
        }

        public void setConfig(string key, double value)
        {
            hash.Put(key, value);
        }

        public void setConfig(string key, int value)
        {
            hash.Put(key, value);
        }
    } 
}
