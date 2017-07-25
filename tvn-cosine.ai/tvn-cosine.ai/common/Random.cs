using tvn.cosine.ai.common.api;

namespace tvn.cosine.ai.common
{
    public class Random : System.Random, IRandom
    {
        public bool NextBoolean()
        {
            return this.Next(2) == 1;
        }
    }
}
