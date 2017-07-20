using System;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.util;
using tvn.cosine.ai.util.math.geom.shapes;

namespace tvn.cosine.ai.environment.map
{
    public class MapFunctions
    {
        public static ActionsFunction<string, MoveToAction> createActionsFunction(Map map)
        {
            return (state) =>
            {
                IQueue<MoveToAction> actions = Factory.CreateQueue<MoveToAction>();

                IQueue<string> linkedLocations = map.getPossibleNextLocations(state);

                foreach (string location in linkedLocations)
                {
                    actions.Add(new MoveToAction(location));
                }
                return actions;
            };
        }

        public static ActionsFunction<string, MoveToAction> createReverseActionsFunction(Map map)
        {
            return (state) =>
            {
                IQueue<MoveToAction> actions = Factory.CreateQueue<MoveToAction>();

                IQueue<string> linkedLocations = map.getPossiblePrevLocations(state);

                foreach (string location in linkedLocations)
                {
                    actions.Add(new MoveToAction(location));
                }
                return actions;
            };
        }

        public static ResultFunction<string, MoveToAction> createResultFunction()
        {
            return (s, a) =>
            {
                if (a != null)
                    return a.getToLocation();
                // If the action is NoOp the result will be the current state.
                return s;
            };
        }
         
        /**
         * Implementation of StepCostFunction interface that uses the distance between
         * locations to calculate the cost in addition to a constant cost, so that it
         * may be used in conjunction with a Uniform-cost search.
         */
        public static StepCostFunction<string, MoveToAction> createDistanceStepCostFunction(Map map)
        {
            // Used by Uniform-cost search to ensure every step is greater than or equal
            // to some small positive constant
            double constantCost = 1.0;
            return (state, action, statePrimed) =>
            {
                double distance = map.getDistance(state, statePrimed);
                if (distance <= 0)
                    return constantCost;
                return distance;
            };
        }

        public static Function<IPercept, string> createPerceptToStateFunction()
        {
            return (p) => (string)((DynamicPercept)p).getAttribute(DynAttributeNames.PERCEPT_IN);
        }

        /** Returns a heuristic function based on straight line distance computation. */
        public static ToDoubleFunction<Node<string, MoveToAction>> createSLDHeuristicFunction(string goal, Map map)
        {
            return new SLDHeuristicFunction(goal,map);
        }

        public static double getSLD(string loc1, string loc2, Map map)
        {
            double result = 0.0;
            Point2D pt1 = map.getPosition(loc1);
            Point2D pt2 = map.getPosition(loc2);
            if (pt1 != null && pt2 != null)
                result = pt1.distance(pt2);
            return result;
        }

        class SLDHeuristicFunction : ToDoubleFunction<Node<string, MoveToAction>>
        {
            private string goal;
            private Map map;

            public SLDHeuristicFunction(string goal, Map map)
            {
                this.goal = goal;
                this.map = map;
            }

            public double applyAsDouble(Node<string, MoveToAction> node)
            {
                return getSLD(node.getState(), goal, map); 
            }
        }
    }
}
