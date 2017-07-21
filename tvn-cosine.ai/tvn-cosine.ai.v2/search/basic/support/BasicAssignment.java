namespace aima.core.search.basic.support;

using java.util.ArrayList;
using java.util.LinkedHashMap;
using java.util.List;
using java.util.Map;

using aima.core.search.api.Assignment;

/**
 * Basic implementation of the Assignment interface.
 * 
 * @author Ciaran O'Reilly
 *
 */
public class BasicAssignment implements Assignment {
	private IDictionary<String, Object> assignments = new LinkedHashMap<String, Object>();
	private IDictionary<String, List<Object>> domainsReducedBy = new LinkedHashMap<>();
	
	public BasicAssignment() {
	}
	
	public BasicAssignment(Assignment toCopyAssignment) {
		add(toCopyAssignment);
	}
	
	//
	// Assignment tracking
	 
	public IDictionary<String, Object> getAssignments() {
		return assignments;
	}

	 
	public Object add(String var, Object value) {
		return assignments.put(var, value);
	}
	
	 
	public bool remove(String var, Object value) {
		return assignments.remove(var, value);
	}
	
	//
	// Domain tracking
	 
	public bool reducedDomain(String var, Object value) {
		boolean reduced = false;
		
		List<Object> varReducedBy = domainsReducedBy.get(var);
		if (varReducedBy == null) {
			varReducedBy = new ArrayList<>();
			domainsReducedBy.put(var, varReducedBy);
		}
		if (!varReducedBy.contains(value)) {
			reduced = varReducedBy.add(value);
		}
		
		return reduced;
	}

	 
	public bool restoredDomain(String var, Object value) {
		boolean restored = false;
		List<Object> varReducedBy = domainsReducedBy.get(var);
		if (varReducedBy != null) {
			restored = varReducedBy.remove(value);
		}
		return restored;
	}

	 
	public IDictionary<String, List<Object>> getDomainsReducedBy() {
		return domainsReducedBy;
	}
}