using System; 

namespace tvn.cosine.ai.common
{
    public class DefaultRandom : Random, IRandom
    {
        public bool NextBoolean()
        {
            return this.Next(2) == 1;
        }
    }
}
