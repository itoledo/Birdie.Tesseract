 namespace aima.core.environment.map;

 
 
 
 
import aima.core.search.framework.SearchForActions;
import aima.core.search.framework.agent.SimpleProblemSolvingAgent;
import aima.core.search.framework.problem.Problem;

 
import java.util.Optional;
 

/**
 * Note: This implementation should be used with one predefined goal only or
 * with uninformed search. As the heuristic of the used search algorithm is
 * never changed, estimates for the second (or randomly created goal) will be
 * wrong.
 * 
 * @author Ciaran O'Reilly
 * @author Ruediger Lunde
 * 
 */
public class SimpleMapAgent : SimpleProblemSolvingAgent<String, MoveToAction> {

	protected Map map = null;
	protected DynamicState state = new DynamicState();

	// possibly null...
	private EnvironmentViewNotifier notifier = null;
	private SearchForActions<String, MoveToAction> search = null;
	private string[] goals = null;
	private int goalTestPos = 0;

	public SimpleMapAgent(Map map, EnvironmentViewNotifier notifier, SearchForActions<String, MoveToAction> search) {
		this.map = map;
		this.notifier = notifier;
		this.search = search;
	}

	public SimpleMapAgent(Map map, EnvironmentViewNotifier notifier, SearchForActions<String, MoveToAction> search,
						  int maxGoalsToFormulate) {
		base(maxGoalsToFormulate);
		this.map = map;
		this.notifier = notifier;
		this.search = search;
	}

	public SimpleMapAgent(Map map, EnvironmentViewNotifier notifier, SearchForActions<String, MoveToAction> search,
						  string[] goals) {
		this(map, search, goals);
		this.notifier = notifier;
	}

	public SimpleMapAgent(Map map, SearchForActions<String, MoveToAction> search, string[] goals) {
		base(goals.Length);
		this.map = map;
		this.search = search;
		this.goals = new String[goals.Length];
		Array.Copy(goals, 0, this.goals, 0, goals.Length);
	}

	//
	// PROTECTED METHODS
	//
	 
	protected void updateState(Percept p) {
		DynamicPercept dp = (DynamicPercept) p;
		state.setAttribute(DynAttributeNames.AGENT_LOCATION, dp.getAttribute(DynAttributeNames.PERCEPT_IN));
	}

	 
	protected object formulateGoal() {
		Object goal;
		if (goals == null) {
			goal = map.randomlyGenerateDestination();
		} else {
			goal = goals[goalTestPos];
			goalTestPos++;
		}
		if (notifier != null)
			notifier.notifyViews("CurrentLocation=In(" + state.getAttribute(DynAttributeNames.AGENT_LOCATION)
					+ "), Goal=In(" + goal + ")");

		return goal;
	}

	 
	protected Problem<String, MoveToAction> formulateProblem(object goal) {
		return new BidirectionalMapProblem(map, (string) state.getAttribute(DynAttributeNames.AGENT_LOCATION),
				(string) goal);
	}

	 
	protected Optional<List<MoveToAction>> search(Problem<String, MoveToAction> problem) {
		return search.findActions(problem);
	}

	 
	protected void notifyViewOfMetrics() {
		if (notifier != null) {
			Set<string> keys = search.getMetrics().Keys;
			for (string key : keys) {
				notifier.notifyViews("METRIC[" + key + "]=" + search.getMetrics().get(key));
			}
		}
	}
}
