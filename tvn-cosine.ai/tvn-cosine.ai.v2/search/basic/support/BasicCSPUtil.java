namespace aima.core.search.basic.support;

using java.util.ArrayList;
using java.util.List;
using java.util.Map;
using java.util.Set;
using java.util.concurrent.atomic.AtomicInteger;

using aima.core.search.api.Assignment;
using aima.core.search.api.CSP;
using aima.core.search.api.Constraint;
using aima.core.util.Util;

/**
 * Some basic utility routines for CSPs.
 * 
 * @author Ciaran O'Reilly
 *
 */
public class BasicCSPUtil {
	public static int getNumberNeigboringConflicts(String variable, Object value, CSP csp,
			Assignment currentAssignment) {
		AtomicInteger nconflicts = new AtomicInteger(0);

		// Use a local assignment and update to have the currently set value for
		// the input variable
		Assignment assignment = new BasicAssignment(currentAssignment);
		assignment.add(variable, value);

		// Get all of the neighboring variables along with the input variable
		// So we can identify constraints that are covered by their scope
		Set<String> neighborVariables = csp.getNeighbors(variable);

		// Determine the constraints covered by the neighboring set of variables
		List<Constraint> neighboringConstraints = csp.getNeighboringConstraints(variable);

		// Based on the assignment and neighboring values, determine the set of
		// allowed assignments for each variable.
		Map<String, List<Object>> allowedAssignments = assignment.getAllowedAssignments(csp, neighborVariables);

		// Determine the # of conflicts
		List<List<Object>> possibleValues = new ArrayList<>();
		for (Constraint constraint : neighboringConstraints) {
			// Collect the possible values for each variable in
			// the contraint's scope
			possibleValues.clear();
			constraint.getScope().forEach(scopeVar -> {
				possibleValues.add(allowedAssignments.get(scopeVar));
			});
			// For each permutation of possible arguments, count
			// the # that are not a member of the constraint's
			// relation (i.e. the # of possible conflicts).
			Util.permuteArguments(Object.class, possibleValues, (Object[] args) -> {
				if (!constraint.getRelation().isMember(args)) {
					nconflicts.incrementAndGet();
				}
			});
		}

		return nconflicts.get();
	}
}
