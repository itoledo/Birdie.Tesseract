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
            return new MapActionsFunction(map, false);
        }

        public static ActionsFunction<string, MoveToAction> createReverseActionsFunction(Map map)
        {
            return new MapActionsFunction(map, true);
        }

        public static ResultFunction<string, MoveToAction> createResultFunction()
        {
            return new MapResultFunction();
        }


        public static StepCostFunction<string, MoveToAction> createDistanceStepCostFunction(Map map)
        {
            return new DistanceStepCostFunction(map);
        }

        public static Function<Percept, string> createPerceptToStateFunction()
        {
            return (p) => (string)((DynamicPercept)p).getAttribute(DynAttributeNames.PERCEPT_IN);
        }

        /** Returns a heuristic function based on straight line distance computation. */
        public static ToDoubleFunction<Node<string, MoveToAction>> createSLDHeuristicFunction(string goal, Map map)
        {
            return node => getSLD(node.getState(), goal, map);
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

        private class MapActionsFunction : ActionsFunction<string, MoveToAction>
        {
            private Map map = null;
            private bool reverseMode;

            private MapActionsFunction(Map map, bool reverseMode)
            {
                this.map = map;
                this.reverseMode = reverseMode;
            }

            public IQueue<MoveToAction> apply(string state)
            {
                IQueue<MoveToAction> actions = Factory.CreateQueue<MoveToAction>();

                IQueue<string> linkedLocations = reverseMode ? map.getPossiblePrevLocations(state)
                        : map.getPossibleNextLocations(state);

                foreach (string location in linkedLocations)
                {
                    actions.Add(new MoveToAction(location));
                }
                return actions;
            }
        }

        private class MapResultFunction : ResultFunction<string, MoveToAction>
        {
            public string apply(string s, MoveToAction a)
            {
                if (a != null)
                    return a.getToLocation();
                // If the action is NoOp the result will be the current state.
                return s;
            }
        }

        /**
         * Implementation of StepCostFunction interface that uses the distance between
         * locations to calculate the cost in addition to a constant cost, so that it
         * may be used in conjunction with a Uniform-cost search.
         */
        private class DistanceStepCostFunction : StepCostFunction<string, MoveToAction>
        {
            private Map map = null;

            // Used by Uniform-cost search to ensure every step is greater than or equal
            // to some small positive constant
            private static double constantCost = 1.0;

            private DistanceStepCostFunction(Map map)
            {
                this.map = map;
            }

            public double applyAsDouble(string state, MoveToAction action, string statePrimed)
            {
                double distance = map.getDistance(state, statePrimed);
                if (distance == null || distance <= 0)
                    return constantCost;
                return distance;
            }
        }
    }
}
