using System;
using System.Collections.Generic;
using System.Linq;
using tvn.cosine.ai.agent;
using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.search.framework;
using tvn.cosine.ai.search.framework.problem;
using tvn.cosine.ai.util.math.geom.shapes;

namespace tvn.cosine.ai.environment.map
{
    /**
     * @author Ruediger Lunde
     * @author Ciaran O'Reilly
     */
    public class MapFunctions
    {

        public static ActionsFunction<string, MoveToAction> createActionsFunction(Map map)
        {
            return (state) =>
            {
                List<MoveToAction> actions = new List<MoveToAction>();

                IList<string> linkedLocations = map.getPossibleNextLocations(state);
                actions.AddRange(linkedLocations.Select(x => new MoveToAction(x)).ToList());
                return actions;
            };
        }

        public static ActionsFunction<string, MoveToAction> createReverseActionsFunction(Map map)
        {
            return (state) =>
            {
                List<MoveToAction> actions = new List<MoveToAction>();

                IList<string> linkedLocations = map.getPossiblePrevLocations(state);
                actions.AddRange(linkedLocations.Select(x => new MoveToAction(x)).ToList());
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


        public static StepCostFunction<string, MoveToAction> createDistanceStepCostFunction(Map map)
        {
            double constantCost = 1.0;

            return (state, action, statePrimed) =>
            {
                double distance = map.getDistance(state, statePrimed);
                if (distance <= 0)
                    return constantCost;
                return distance;
            };
        }

        public static Func<IPercept, string> createPerceptToStateFunction()
        {
            return p => (string)((DynamicPercept)p).getAttribute(DynAttributeNames.PERCEPT_IN);
        }

        /** Returns a heuristic function based on straight line distance computation. */
        public static Func<Node<string, MoveToAction>, double> createSLDHeuristicFunction(string goal, Map map)
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
    } 
}
