namespace tvn.cosine.collections.api
{
    public interface IEqualityComparer<T> 
    {  
        bool Equals(T x, T y); 
        int GetHashCode(T obj);
    }
}
