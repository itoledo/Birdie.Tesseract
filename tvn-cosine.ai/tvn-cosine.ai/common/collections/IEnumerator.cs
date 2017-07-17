namespace tvn.cosine.ai.common.collections
{ 
    public interface IEnumerator<T> 
    {
        T Current { get; }
        T GetCurrent();
        bool MoveNext();
        void Reset();
    }
}
