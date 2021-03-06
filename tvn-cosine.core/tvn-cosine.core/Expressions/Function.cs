﻿using System;
using System.Collections.Generic;

namespace tvn.cosine.expressions
{
    public abstract class Function : ExpressionObject, IFunction
    {
        public Function(string description)
            : base(description)
        { }

        public Action<Stack<ExpressionObject>> F { get; }
    }
}
