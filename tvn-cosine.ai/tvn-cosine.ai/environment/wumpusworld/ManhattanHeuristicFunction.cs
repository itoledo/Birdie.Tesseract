 namespace aima.core.environment.wumpusworld;

 
import aima.core.search.framework.Node;

 
 
 
import java.util.function.Function;
import java.util.function.ToDoubleFunction;

/**
 * Heuristic for calculating the Manhattan distance between two rooms within a Wumpus World cave.
 * 
 * @author Federico Baron
 * @author Alessandro Daniele
 * @author Ciaran O'Reilly
 */
public class ManhattanHeuristicFunction : ToDoubleFunction<Node<AgentPosition, Action>> {
	
	List<Room> goals = new List<>();
	
	public ManhattanHeuristicFunction(ISet<Room> goals) {
		this.goals.addAll(goals);
	}
	
	 
	public double applyAsDouble(Node<AgentPosition, Action> node) {
		AgentPosition pos = node.getState();
		int nearestGoalDist = Integer.MAX_VALUE;
		for (Room g : goals) {
			int tmp = evaluateManhattanDistanceOf(pos.getX(), pos.getY(), g.getX(), g.getY());
			
			if (tmp < nearestGoalDist) {
				nearestGoalDist = tmp;
			}
		}
		
		return (double) nearestGoalDist;
	}

	//
	// PRIVATE
	//
	private int evaluateManhattanDistanceOf(int x1, int y1, int x2, int y2) {		
		return Math.Abs(x1-x2) + Math.Abs(y1-y2); 
	}
}
