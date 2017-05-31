using System.Collections.Generic;

namespace tvn_cosine.ai.DataStructures.Queues
{
    public interface IQueue<T> : ICollection<T>
    {
        bool IsEmpty();
        T Peek();
        T Remove();
    }

    public interface IQueue : IQueue<object>
    { } 
}
