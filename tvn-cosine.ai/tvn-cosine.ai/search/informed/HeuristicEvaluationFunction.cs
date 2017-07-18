﻿using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.search.informed
{
    /**
     * Super class for all evaluation functions which make use of heuristics.
     * Informed search algorithms use heuristics to estimate remaining costs to
     * reach a goal state from a given node. Their evaluation functions only differ
     * in the way how they combine the estimated remaining costs with the costs of
     * the already known path to the node.
     * 
     * @author Ruediger Lunde
     *
     */
    public abstract class HeuristicEvaluationFunction<S, A> : ToDoubleFunction<Node<S, A>>
    {
        protected ToDoubleFunction<Node<S, A>> h = (node) => 0.0;

        public ToDoubleFunction<Node<S, A>> getHeuristicFunction()
        {
            return h;
        }

        public void setHeuristicFunction(ToDoubleFunction<Node<S, A>> h)
        {
            this.h = h;
        }
    }
}
