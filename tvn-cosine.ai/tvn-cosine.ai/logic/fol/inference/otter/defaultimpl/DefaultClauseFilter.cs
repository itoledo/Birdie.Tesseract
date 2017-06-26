 namespace aima.core.logic.fol.inference.otter.defaultimpl;

 

import aima.core.logic.fol.inference.otter.ClauseFilter;
import aima.core.logic.fol.kb.data.Clause;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class DefaultClauseFilter : ClauseFilter {
	public DefaultClauseFilter() {

	}

	//
	// START-ClauseFilter
	public ISet<Clause> filter(ISet<Clause> clauses) {
		return clauses;
	}

	// END-ClauseFilter
	//
}
