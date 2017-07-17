﻿namespace tvn.cosine.ai.common.collections
{
    public static class Factory
    {
        public static IMap<KEY, VALUE> CreateMap<KEY, VALUE>()
        {
            return new Map<KEY, VALUE>();
        }

        public static IQueue<T> CreateQueue<T>()
        {
            return new Queue<T>();
        }

        public static ISet<T> CreateSet<T>()
        {
            return new Set<T>();
        }

        public static IQueue<T> CreateLifoQueue<T>()
        {
            return new LifoQueue<T>();
        }

        public static IQueue<T> CreateFifoQueue<T>()
        {
            return new FifoQueue<T>();
        }

        public static IQueue<T> CreateQueue<T>(IQueue<T> collection)
        {
            return new Queue<T>(collection);
        }

        public static IQueue<T> CreateFifoQueue<T>(IQueue<T> collection)
        {
            return new FifoQueue<T>(collection);
        }

        public static IQueue<T> CreateLifoQueue<T>(IQueue<T> collection)
        {
            return new LifoQueue<T>(collection);
        }

        public static IMap<KEY, VALUE> CreateReadOnlyMap<KEY, VALUE>(IMap<KEY, VALUE> collection)
        {
            return new ReadOnlyMap<KEY, VALUE>(collection);
        }

        public static ISet<T> CreateReadOnlySet<T>(ISet<T> collection)
        {
            return new ReadOnlySet<T>(collection);
        }

        public static ISet<T> CreateReadOnlySet<T>(IQueue<T> collection)
        {
            return new ReadOnlySet<T>(collection);
        }

        public static IQueue<T> CreateReadOnlyQueue<T>(IQueue<T> collection)
        {
            return new ReadOnlyQueue<T>(collection);
        }
    }
}
