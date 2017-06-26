 namespace aima.core.logic.fol.inference.trace;

 

import aima.core.logic.fol.inference.InferenceResult;
import aima.core.logic.fol.kb.data.Clause;

/**
 * @author Ciaran O'Reilly
 * 
 */
public interface FOLTFMResolutionTracer {
	void stepStartWhile(ISet<Clause> clauses, int totalNoClauses,
			int totalNoNewCandidateClauses);

	void stepOuterFor(Clause i);

	void stepInnerFor(Clause i, Clause j);

	void stepResolved(Clause iFactor, Clause jFactor, ISet<Clause> resolvents);

	void stepFinished(ISet<Clause> clauses, InferenceResult result);
}
