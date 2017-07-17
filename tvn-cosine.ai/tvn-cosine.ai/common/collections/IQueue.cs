namespace tvn.cosine.ai.common.collections
{
    public interface IQueue<T> : IEnumerable<T>
    {
        T Get(int index);
        int IndexOf(T item);
        void Insert(int index, T item);
        void RemoveAt(int index);

        void AddAll(IQueue<T> items);
        bool IsReadonly();
        bool Add(T item);
        bool IsEmpty();
        int Size();
        T Pop();
        T Peek();
        void Clear();
        bool Contains(T item);
        bool Remove(T item);
    }
}
