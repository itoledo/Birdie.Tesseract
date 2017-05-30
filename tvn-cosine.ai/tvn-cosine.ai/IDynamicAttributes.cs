using System.Collections.Generic;

namespace tvn_cosine.ai
{ 
    public interface IDynamicAttributes<KEY, VALUE>
    {
        IDictionary<KEY, VALUE> Attributes { get; }
    }

    public interface IDynamicAttributes : IDynamicAttributes<object, object>
    { }
}
