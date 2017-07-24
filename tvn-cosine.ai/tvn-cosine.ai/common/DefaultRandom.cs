using System;
using tvn.cosine.ai.common.api;

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
