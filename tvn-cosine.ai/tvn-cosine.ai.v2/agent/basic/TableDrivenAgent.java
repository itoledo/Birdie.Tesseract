namespace aima.core.agent.basic;

using java.util.ArrayList;
using java.util.HashMap;
using java.util.List;
using java.util.Map;

using aima.core.agent.api.Agent;

/**
 * Artificial Intelligence A Modern Approach (4th Edition): Figure ??, page ??.
 * <br>
 * <br>
 *
 * <pre>
 * function TABLE-DRIVEN-AGENT(percept) returns an action
 *   persistent: percepts, a sequence, initially empty
 *               table, a table of actions, indexed by percept sequences, initially fully specified
 *
 *   append percept to end of percepts
 *   action &larr; LOOKUP(percepts, table)
 *   return action
 * </pre>
 *
 * Figure ?? The TABLE-DRIVEN-AGENT program is invoked for each new percept and
 * returns an action each time. It retains the complete percept sequence in
 * memory.
 *
 * @author Ciaran O'Reilly
 */
public class TableDrivenAgent<A, P> implements Agent<A, P> {
	// persistent: 
	private List<P> percepts = new ArrayList<>(); // a sequence, initially empty
	private IDictionary<List<P>, A> table = new HashMap<>(); // a table of actions, indexed by percept sequences, initially fully specified

	// function TABLE-DRIVEN-AGENT(percept) returns an action
	 
	public A perceive(P percept) {
		// append percept to end of percepts
		getPercepts().add(percept);
		// action <- LOOKUP(percepts, table)
		A action = lookup(getPercepts(), getTable());
		// return action
		return action;
	}

	public A lookup(List<P> percepts, IDictionary<List<P>, A> table) {
		A action = table.get(percepts);
		return action;
	}
	
	//
	// Supporting Code
	public TableDrivenAgent(Map<List<P>, A> table) {
		this.table.putAll(table);
	}

	//
	// Getters
	public List<P> getPercepts() {
		return percepts;
	}

	public IDictionary<List<P>, A> getTable() {
		return table;
	}
}
