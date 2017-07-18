﻿using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public interface Term : FOLNode
    {
        IQueue<Term> getArgs();

        new Term copy();
    }
}
