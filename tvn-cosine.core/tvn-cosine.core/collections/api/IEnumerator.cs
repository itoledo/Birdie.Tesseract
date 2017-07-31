namespace tvn.cosine.collections.api
{ 
    public interface IEnumerator<T> 
    {
        T Current { get; }
        T GetCurrent();
        bool MoveNext();
        void Reset(); 
    }
}
