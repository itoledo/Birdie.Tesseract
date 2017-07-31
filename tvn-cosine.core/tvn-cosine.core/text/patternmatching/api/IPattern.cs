using System;
using System.Collections.Generic;

namespace tvn.cosine.text.patternmatching.api
{
    public interface IPattern : IEnumerable<char>, 
                                IEquatable<IPattern>
    { }
}
