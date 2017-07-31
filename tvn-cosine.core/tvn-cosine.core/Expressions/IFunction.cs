using System;
using System.Collections.Generic;

namespace tvn.cosine.expressions
{
    public interface IFunction
    {
        Action<Stack<ExpressionObject>> F { get; }
    }
}
