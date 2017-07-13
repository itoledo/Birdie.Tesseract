using System.Collections.Generic;

namespace tvn.cosine.ai.learning.neural
{
    /**
     * A holder for config data for neural networks and possibly for other learning
     * systems.
     * 
     * @author Ravi Mohan
     * 
     */
    public class NNConfig
    {
        private readonly IDictionary<string, object> hash;

        public NNConfig(IDictionary<string, object> hash)
        {
            this.hash = hash;
        }

        public NNConfig()
        {
            this.hash = new Dictionary<string, object>();
        }

        public double getParameterAsDouble(string key)
        { 
            return (double)hash[key];
        }

        public int getParameterAsInteger(string key)
        { 
            return (int)hash[key];
        }

        public void setConfig(string key, double value)
        {
            hash[key] = value;
        }

        public void setConfig(string key, int value)
        {
            hash[key] = value;
        }
    } 
}
