namespace tvn.cosine.ai.common
{ 
    public interface IEquatable<T>  
    {
        bool Equals(T other);
    }

    public interface IEquatable : IEquatable<object>, IHashable
    { }
}
