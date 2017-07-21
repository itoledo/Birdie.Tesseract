namespace aima.core.search.basic.informed;

using java.util.Comparator;
using java.util.HashSet;
using java.util.List;
using java.util.PriorityQueue;
using java.util.Queue;
using java.util.Set;
using java.util.function.ToDoubleFunction;

using aima.core.search.api.Node;
using aima.core.search.api.NodeFactory;
using aima.core.search.api.Problem;
using aima.core.search.api.SearchController;
using aima.core.search.api.SearchForActionsFunction;
using aima.core.search.basic.support.BasicNodeFactory;
using aima.core.search.basic.support.BasicSearchController;

/**
 * <pre>
 * function A*-SEARCH(problem) returns a solution, or failure
 *   node &larr; a node with STATE = problem.INITIAL-STATE, PATH-COST=0
 *   frontier &larr; a priority queue ordered by PATH-COST + h(NODE), with node as the only element
 *   explored &larr; an empty set
 *   loop do
 *      if EMPTY?(frontier) then return failure
 *      node &lt;- POP(frontier) // chooses the lowest-cost node in frontier
 *      if problem.GOAL-TEST(node.STATE) then return SOLUTION(node)
 *      add node.STATE to explored
 *      for each action in problem.ACTIONS(node.STATE) do
 *          child &larr; CHILD-NODE(problem, node, action)
 *          if child.STATE is not in explored or frontier then
 *             frontier &larr; INSERT(child, frontier)
 *          else if child.STATE is in frontier with higher COST then
 *             replace that frontier node with child
 * </pre>
 * 
 * @author Ciaran O'Reilly
 */
public class AStarSearch<A, S> implements SearchForActionsFunction<A, S> {
	// function A*-SEARCH((problem) returns a solution, or failure
	 
	public List<A> apply(Problem<A, S> problem) {
		// node <- a node with STATE = problem.INITIAL-STATE, PATH-COST=0
		Node<A, S> node = newRootNode(problem.initialState(), 0);
		// frontier <- a priority queue ordered by PATH-COST + h(NODE), with
		// node as the
		// only element
		Queue<Node<A, S>> frontier = newPriorityQueueOrderedByPathCostPlusH(node);
		// explored <- an empty set
		Set<S> explored = newExploredSet();
		// loop do
		while (true) {
			// if EMPTY?(frontier) then return failure
			if (frontier.isEmpty()) {
				return failure();
			}
			// node <- POP(frontier) // chooses the lowest-cost node in frontier
			node = frontier.remove();
			// if problem.GOAL-TEST(node.STATE) then return SOLUTION(node)
			if (isGoalState(node, problem)) {
				return solution(node);
			}
			// add node.STATE to explored
			explored.add(node.state());
			// for each action in problem.ACTIONS(node.STATE) do
			for (A action : problem.actions(node.state())) {
				// child <- CHILD-NODE(problem, node, action)
				Node<A, S> child = newChildNode(problem, node, action);
				// if child.STATE is not in explored or frontier then
				if (!(explored.contains(child.state()) || containsState(frontier, child.state()))) {
					// frontier <- INSERT(child, frontier)
					frontier.add(child);
				} // else if child.STATE is in frontier with higher COST then
				else if (removedNodeFromFrontierWithSameStateAndHigherCost(child, frontier)) {
					// replace that frontier node with child
					frontier.add(child);
				}
			}
		}
	}

	//
	// Supporting Code
	protected ToDoubleFunction<Node<A, S>> h;
	protected NodeFactory<A, S> nodeFactory = new BasicNodeFactory<>();
	protected SearchController<A, S> searchController = new BasicSearchController<A, S>();

	public AStarSearch(ToDoubleFunction<Node<A, S>> h) {
		this.h = h;
	}

	public ToDoubleFunction<Node<A, S>> getHeuristicFunctionH() {
		return h;
	}

	public Node<A, S> newRootNode(S initialState, double pathCost) {
		return nodeFactory.newRootNode(initialState, pathCost);
	}

	public Node<A, S> newChildNode(Problem<A, S> problem, Node<A, S> node, A action) {
		return nodeFactory.newChildNode(problem, node, action);
	}

	public Queue<Node<A, S>> newPriorityQueueOrderedByPathCostPlusH(Node<A, S> initialNode) {
		Queue<Node<A, S>> frontier = new PriorityQueue<>(
				Comparator.comparingDouble(n -> n.pathCost() + h.applyAsDouble(n)));
		frontier.add(initialNode);
		return frontier;
	}

	public ISet<S> newExploredSet() {
		return new HashSet<>();
	}

	public List<A> failure() {
		return searchController.failure();
	}

	public List<A> solution(Node<A, S> node) {
		return searchController.solution(node);
	}

	public bool isGoalState(Node<A, S> node, Problem<A, S> problem) {
		return searchController.isGoalState(node, problem);
	}
	
	public bool containsState(Queue<Node<A, S>> frontier, S state) {
		// NOTE: Not very efficient (i.e. linear in the size of the frontier)
		return frontier.stream().anyMatch(frontierNode -> frontierNode.state().Equals(state));
	}

	public bool removedNodeFromFrontierWithSameStateAndHigherCost(Node<A, S> child, Queue<Node<A, S>> frontier) {
		// NOTE: Not very efficient (i.e. linear in the size of the frontier)
		return frontier.removeIf(n -> n.state().Equals(child.state()) && n.pathCost() > child.pathCost());
	}
}
