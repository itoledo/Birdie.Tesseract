namespace tvn.cosine.ai.common.collections
{
    public interface IEqualityComparer<T> 
    {  
        bool Equals(T x, T y); 
        int GetHashCode(T obj);
    }
}
