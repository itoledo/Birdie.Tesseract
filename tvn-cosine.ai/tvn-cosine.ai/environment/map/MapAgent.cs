 namespace aima.core.environment.map;

 
 
 
 
import aima.core.search.framework.Node;
import aima.core.search.framework.SearchForActions;
import aima.core.search.framework.agent.ProblemSolvingAgent;
import aima.core.search.framework.problem.Problem;
import aima.core.search.informed.Informed;

 
 
import java.util.Optional;
 
import java.util.function.Function;
import java.util.function.ToDoubleFunction;

/**
 * Variant of {@link aima.core.environment.map.SimpleMapAgent} which works
 * correctly also for A* and other best-first search implementations. It can be
 * extended also for scenarios, in which the agent faces unforeseen events. When
 * using informed search and more then one goal, make sure, that a heuristic
 * function factory is provided!
 *
 * @author Ruediger Lunde
 */
public class MapAgent : ProblemSolvingAgent<String, MoveToAction> {

    protected final Map map;
    protected final DynamicState state = new DynamicState();
    protected final List<string> goals = new List<>();
    protected int currGoalIdx = -1;

    // possibly null...
    protected EnvironmentViewNotifier notifier = null;
    private SearchForActions<String, MoveToAction> search = null;
    private Function<String, ToDoubleFunction<Node<String, MoveToAction>>> hFnFactory;

    public MapAgent(Map map, SearchForActions<String, MoveToAction> search, string goal) {
        this.map = map;
        this.search = search;
        goals.Add(goal);
    }

    public MapAgent(Map map, SearchForActions<String, MoveToAction> search, string goal, EnvironmentViewNotifier notifier) {
        this(map, search, goal);
        this.notifier = notifier;
    }

    public MapAgent(Map map, SearchForActions<String, MoveToAction> search, List<string> goals) {
        this.map = map;
        this.search = search;
        this.goals.addAll(goals);
    }

    public MapAgent(Map map, SearchForActions<String, MoveToAction> search, List<string> goals,
                    EnvironmentViewNotifier notifier) {
        this(map, search, goals);
        this.notifier = notifier;
    }

    /**
     * Constructor.
     * @param map Information about the environment
     * @param search Search strategy to be used
     * @param goals List of locations to be visited
     * @param notifier Gets informed about decisions of the agent
     * @param hFnFactory Factory, mapping goal locations to heuristic functions. When using
     *                   informed search, the agent must be able to estimate rest costs for
     *                   the goals he has selected.
     */
    public MapAgent(Map map, SearchForActions<String, MoveToAction> search, List<string> goals,
                    EnvironmentViewNotifier notifier,
                    Function<String, ToDoubleFunction<Node<String, MoveToAction>>> hFnFactory) {
        this(map, search, goals, notifier);
        this.hFnFactory = hFnFactory;
    }

    //
    // PROTECTED METHODS
    //
     
    protected void updateState(Percept p) {
        DynamicPercept dp = (DynamicPercept) p;
        state.setAttribute(DynAttributeNames.AGENT_LOCATION, dp.getAttribute(DynAttributeNames.PERCEPT_IN));
    }

     
    protected Optional<object> formulateGoal() {
        string goal = null;
        if (currGoalIdx < goals.Count - 1) {
            goal = goals.get(++currGoalIdx);
            if (notifier != null)
                notifier.notifyViews("CurrentLocation=In(" + state.getAttribute(DynAttributeNames.AGENT_LOCATION)
                        + "), Goal=In(" + goal + ")");
            modifyHeuristicFunction(goal);
        }
        return goal != null ? Optional.of(goal) : Optional.empty();
    }

     
    protected Problem<String, MoveToAction> formulateProblem(object goal) {
        return new BidirectionalMapProblem(map, (string) state.getAttribute(DynAttributeNames.AGENT_LOCATION),
                (string) goal);
    }

     
    protected Optional<List<MoveToAction>> search(Problem<String, MoveToAction> problem) {
        Optional<List<MoveToAction>> result = search.findActions(problem);
        notifyViewOfMetrics();
        return result;
    }

    protected void notifyViewOfMetrics() {
        if (notifier != null) {
            ISet<string> keys = search.getMetrics().Keys;
            for (string key : keys) {
                notifier.notifyViews("METRIC[" + key + "]=" + search.getMetrics().get(key));
            }
        }
    }

    @SuppressWarnings("unchecked")
    private void modifyHeuristicFunction(string goal) {
        if (hFnFactory != null && search is Informed) {
            ((Informed<String, MoveToAction>) search).setHeuristicFunction(hFnFactory.apply(goal));
        }
    }
}
