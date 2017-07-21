namespace aima.core.logic.api.propositional;

using java.util.Set;

using aima.core.logic.basic.propositional.kb.data.Clause;
using aima.core.logic.basic.propositional.kb.data.Model;

/**
 * Basic interface to a SAT Solver.
 * 
 * @author Ciaran O'Reilly
 *
 */
public interface SATSolver {
	/**
	 * Solve a given problem in CNF format.
	 * 
	 * @param cnf
	 *        a CNF representation of the problem to be solved.
	 * @return a satisfiable model or null if it cannot be satisfied.
	 */
	Model solve(Set<Clause> cnf);
}
