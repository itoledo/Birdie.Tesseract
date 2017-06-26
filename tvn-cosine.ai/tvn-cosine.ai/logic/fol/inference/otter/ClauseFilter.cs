 namespace aima.core.logic.fol.inference.otter;

 

import aima.core.logic.fol.kb.data.Clause;

/**
 * @author Ciaran O'Reilly
 * 
 */
public interface ClauseFilter {
	Set<Clause> filter(ISet<Clause> clauses);
}
