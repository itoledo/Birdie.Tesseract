namespace tvn.cosine.ai.common.collections
{ 
    public interface IEnumerator<T> : IDisposable
    {
        T Current { get; }
        bool MoveNext();
        void Reset();
    }
}
