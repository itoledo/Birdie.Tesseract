 namespace aima.core.logic.propositional.inference;

 

import aima.core.logic.propositional.kb.data.Clause;
import aima.core.logic.propositional.kb.data.Model;

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
	Model solve(ISet<Clause> cnf);
}
