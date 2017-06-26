 namespace aima.core.environment.map;

 
 
import aima.core.search.framework.Node;
import aima.core.search.framework.problem.ActionsFunction;
import aima.core.search.framework.problem.ResultFunction;
import aima.core.search.framework.problem.StepCostFunction;
 

 
 
import java.util.function.Function;
import java.util.function.ToDoubleFunction;
 

/**
 * @author Ruediger Lunde
 * @author Ciaran O'Reilly
 */
public class MapFunctions {

    public static ActionsFunction<String, MoveToAction> createActionsFunction(Map map) {
        return new MapActionsFunction(map, false);
    }

    public static ActionsFunction<String, MoveToAction> createReverseActionsFunction(Map map) {
        return new MapActionsFunction(map, true);
    }

    public static ResultFunction<String, MoveToAction> createResultFunction() {
        return new MapResultFunction();
    }


    public static StepCostFunction<String, MoveToAction> createDistanceStepCostFunction(Map map) {
        return new DistanceStepCostFunction(map);
    }

    public static Function<Percept, String> createPerceptToStateFunction() {
        return  p -> (string) ((DynamicPercept) p).getAttribute(DynAttributeNames.PERCEPT_IN);
    }

    /** Returns a heuristic function based on straight line distance computation. */
    public static ToDoubleFunction<Node<String, MoveToAction>> createSLDHeuristicFunction(string goal, Map map) {
        return node -> getSLD(node.getState(), goal, map);
    }

    public static double getSLD(string loc1, string loc2, Map map) {
        double result = 0.0;
        Point2D pt1 = map.getPosition(loc1);
        Point2D pt2 = map.getPosition(loc2);
        if (pt1 != null && pt2 != null)
            result = pt1.distance(pt2);
        return result;
    }

    private static class MapActionsFunction : ActionsFunction<String, MoveToAction> {
        private Map map = null;
        private bool reverseMode;

        private MapActionsFunction(Map map, bool reverseMode) {
            this.map = map;
            this.reverseMode = reverseMode;
        }

        public List<MoveToAction> apply(string state) {
            List<MoveToAction> actions = new List<>();

            List<string> linkedLocations = reverseMode ? map.getPossiblePrevLocations(state)
                    : map.getPossibleNextLocations(state);
            actions.addAll(linkedLocations.stream().map(MoveToAction::new).collect(Collectors.toList()));
            return actions;
        }
    }

    private static class MapResultFunction : ResultFunction<String, MoveToAction> {

        public string apply(string s, MoveToAction a) {
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
    private static class DistanceStepCostFunction : StepCostFunction<String, MoveToAction> {
        private Map map = null;

        // Used by Uniform-cost search to ensure every step is greater than or equal
        // to some small positive constant
        private static double constantCost = 1.0;

        private DistanceStepCostFunction(Map map) {
            this.map = map;
        }

         
        public double applyAsDouble(string state, MoveToAction action, string statePrimed) {
            double distance = map.getDistance(state, statePrimed);
            if (distance == null || distance <= 0)
                return constantCost;
            return distance;
        }
    }
}
