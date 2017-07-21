namespace aima.core.search.basic.uninformed;

using java.util.HashSet;
using java.util.LinkedList;
using java.util.List;
using java.util.Queue;
using java.util.Set;

using aima.core.search.api.Node;
using aima.core.search.api.NodeFactory;
using aima.core.search.api.Problem;
using aima.core.search.api.SearchController;
using aima.core.search.api.SearchForActionsFunction;
using aima.core.search.basic.support.BasicNodeFactory;
using aima.core.search.basic.support.BasicSearchController;

/**
 * Artificial Intelligence A Modern Approach (4th Edition): Figure ??, page ??.
 * <br>
 * <br>
 *
 * <pre>
 * function GRAPH-SEARCH(problem) returns a solution, or failure
 *   initialize the frontier using the initial state of problem
 *   initialize the explored set to be empty
 *   loop do
 *     if the frontier is empty then return failure
 *     choose a leaf node and remove it from the frontier
 *     if the node contains a goal state then return the corresponding solution
 *     add the node to the explored set
 *     expand the chosen node, adding the resulting nodes to the frontier
 *       only if not in the frontier or explored set
 * </pre>
 *
 * Figure ?? An informal description of the general graph-search algorithm.
 *
 *
 * @author Ciaran O'Reilly
 */
public class GraphSearch<A, S> implements SearchForActionsFunction<A, S> {

	// function GRAPH-SEARCH(problem) returns a solution, or failure
	 
	public List<A> apply(Problem<A, S> problem) {
		// initialize the frontier using the initial state of problem
		Queue<Node<A, S>> frontier = newFrontier(problem.initialState());
		// initialize the explored set to be empty
		Set<S> explored = newExploredSet();
		// loop do
		while (true) {
			// if the frontier is empty then return failure
			if (frontier.isEmpty()) {
				return failure();
			}
			// choose a leaf node and remove it from the frontier
			Node<A, S> node = frontier.remove();
			// if the node contains a goal state then return the corresponding
			// solution
			if (isGoalState(node, problem)) {
				return solution(node);
			}
			// add the node to the explored set
			explored.add(node.state());
			// expand the chosen node, adding the resulting nodes to the
			// frontier
			for (A action : problem.actions(node.state())) {
				Node<A, S> child = newChildNode(problem, node, action);
				// only if not in the frontier or explored set
				if (!(containsState(frontier, child) || explored.contains(child.state()))) {
					frontier.add(child);
				}
			}
		}
	}

	//
	// Supporting Code
	protected NodeFactory<A, S> nodeFactory = new BasicNodeFactory<>();
	protected SearchController<A, S> searchController = new BasicSearchController<A, S>();

	public GraphSearch() {
	}

	public Node<A, S> newChildNode(Problem<A, S> problem, Node<A, S> node, A action) {
		return nodeFactory.newChildNode(problem, node, action);
	}

	public Queue<Node<A, S>> newFrontier(S initialState) {
		Queue<Node<A, S>> frontier = new LinkedList<>();
		frontier.add(nodeFactory.newRootNode(initialState));
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

	public bool containsState(Queue<Node<A, S>> frontier, Node<A, S> child) {
		// NOTE: Not very efficient (i.e. linear in the size of the frontier)
		return frontier.stream().anyMatch(frontierNode -> frontierNode.state().Equals(child.state()));
	}
}
