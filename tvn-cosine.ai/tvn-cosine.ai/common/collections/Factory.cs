using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.common.collections
{
    public static class Factory
    {
        public static IMap<KEY, VALUE> CreateMap<KEY, VALUE>()
        {
            throw new NotImplementedException("tvn.cosine.ai.common.collections.Factory.CreateMap<KEY, VALUE>()");
        }

        public static ISet<T> CreateSet<T>()
        {
            throw new NotImplementedException("tvn.cosine.ai.common.collections.Factory.CreateSet<T>()");
        }

        public static ISet<T> CreateFifoQueue<T>()
        {
            throw new NotImplementedException("tvn.cosine.ai.common.collections.Factory.CreateFifoQueue<T>()");
        }

        public static ISet<T> CreateFifoQueue<T>(IQueue<T> collection)
        {
            throw new NotImplementedException("tvn.cosine.ai.common.collections.Factory.CreateFifoQueue<T>(IQueue<T> collection)");
        }

        public static ISet<T> CreateLifoQueue<T>()
        {
            throw new NotImplementedException("tvn.cosine.ai.common.collections.Factory.CreateFifoQueue<T>()");
        }

        public static ISet<T> CreateReadOnlySet<T>(IQueue<T> collection)
        {
            throw new NotImplementedException("tvn.cosine.ai.common.collections.Factory.CreateSet<T>(IQueue<T> collection)");
        }

        public static IQueue<T> CreateReadOnlyQueue<T>(IQueue<T> collection)
        {
            throw new NotImplementedException("tvn.cosine.ai.common.collections.Factory.CreateReadOnlyQueue<T>(IQueue<T> collection)");
        }
    }
}
