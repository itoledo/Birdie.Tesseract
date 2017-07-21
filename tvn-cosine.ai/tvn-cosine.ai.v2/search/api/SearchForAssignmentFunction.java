namespace aima.core.search.api;

using java.util.function.Function;

/**
 * Description of a Search function that looks for a complete and consistent
 * assignment for a Constraint Satisfaction Problem (CSP).
 * 
 * @author Ciaran O'Reilly
 * 
 */
@FunctionalInterface
public interface SearchForAssignmentFunction extends Function<CSP, Assignment> {
	 
	Assignment apply(CSP csp);
}
