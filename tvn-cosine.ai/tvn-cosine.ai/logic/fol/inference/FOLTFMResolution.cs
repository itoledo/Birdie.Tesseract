 namespace aima.core.logic.fol.inference;

 
 
 
 
 
 

import aima.core.logic.fol.Connectors;
import aima.core.logic.fol.inference.proof.Proof;
import aima.core.logic.fol.inference.proof.ProofFinal;
import aima.core.logic.fol.inference.proof.ProofStepGoal;
import aima.core.logic.fol.inference.trace.FOLTFMResolutionTracer;
import aima.core.logic.fol.kb.FOLKnowledgeBase;
import aima.core.logic.fol.kb.data.Clause;
import aima.core.logic.fol.kb.data.Literal;
import aima.core.logic.fol.parsing.ast.ConnectedSentence;
import aima.core.logic.fol.parsing.ast.NotSentence;
import aima.core.logic.fol.parsing.ast.Sentence;
import aima.core.logic.fol.parsing.ast.Term;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * Artificial Intelligence A Modern Approach (3rd Edition): page 347.<br>
 * <br>
 * The algorithmic approach is identical to the propositional case, described in
 * Figure 7.12.<br>
 * <br>
 * However, this implementation will use the T)wo F)inger M)ethod for looking
 * for resolvents between clauses, which is very inefficient.<br>
 * <br>
 * see:<br>
 * <a
 * href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture04.pdf">
 * http://logic.stanford.edu/classes/cs157/2008/lectures/lecture04.pdf</a>,
 * slide 21 for the propositional case. In addition, an Answer literal will be
 * used so that queries with Variables may be answered (see pg. 350 of AIMA3e).
 * 
 * @author Ciaran O'Reilly
 * 
 */
public class FOLTFMResolution : InferenceProcedure {

	private long maxQueryTime = 10 * 1000;

	private FOLTFMResolutionTracer tracer = null;

	public FOLTFMResolution() {

	}

	public FOLTFMResolution(long maxQueryTime) {
		setMaxQueryTime(maxQueryTime);
	}

	public FOLTFMResolution(FOLTFMResolutionTracer tracer) {
		setTracer(tracer);
	}

	public long getMaxQueryTime() {
		return maxQueryTime;
	}

	public void setMaxQueryTime(long maxQueryTime) {
		this.maxQueryTime = maxQueryTime;
	}

	public FOLTFMResolutionTracer getTracer() {
		return tracer;
	}

	public void setTracer(FOLTFMResolutionTracer tracer) {
		this.tracer = tracer;
	}

	//
	// START-InferenceProcedure
	public InferenceResult ask(FOLKnowledgeBase KB, Sentence alpha) {

		// clauses <- the set of clauses in CNF representation of KB ^ ~alpha
		Set<Clause> clauses = new HashSet<Clause>();
		for (Clause c : KB.getAllClauses()) {
			c = KB.standardizeApart(c);
			c.setStandardizedApartCheckNotRequired();
			clauses.addAll(c.getFactors());
		}
		Sentence notAlpha = new NotSentence(alpha);
		// Want to use an answer literal to pull
		// query variables where necessary
		Literal answerLiteral = KB.createAnswerLiteral(notAlpha);
		Set<Variable> answerLiteralVariables = KB
				.collectAllVariables(answerLiteral.getAtomicSentence());
		Clause answerClause = new Clause();

		if (answerLiteralVariables.Count > 0) {
			Sentence notAlphaWithAnswer = new ConnectedSentence(Connectors.OR,
					notAlpha, answerLiteral.getAtomicSentence());
			for (Clause c : KB.convertToClauses(notAlphaWithAnswer)) {
				c = KB.standardizeApart(c);
				c.setProofStep(new ProofStepGoal(c));
				c.setStandardizedApartCheckNotRequired();
				clauses.addAll(c.getFactors());
			}

			answerClause.addLiteral(answerLiteral);
		} else {
			for (Clause c : KB.convertToClauses(notAlpha)) {
				c = KB.standardizeApart(c);
				c.setProofStep(new ProofStepGoal(c));
				c.setStandardizedApartCheckNotRequired();
				clauses.addAll(c.getFactors());
			}
		}

		TFMAnswerHandler ansHandler = new TFMAnswerHandler(answerLiteral,
				answerLiteralVariables, answerClause, maxQueryTime);

		// new <- {}
		Set<Clause> newClauses = new HashSet<Clause>();
		Set<Clause> toAdd = new HashSet<Clause>();
		// loop do
		int noOfPrevClauses = clauses.Count;
		do {
			if (null != tracer) {
				tracer.stepStartWhile(clauses, clauses.Count,
						newClauses.Count);
			}

			newClauses.Clear();

			// for each Ci, Cj in clauses do
			Clause[] clausesA = new Clause[clauses.Count];
			clauses.toArray(clausesA);
			// Basically, using the simple T)wo F)inger M)ethod here.
			for (int i = 0; i < clausesA.Length; ++i) {
				Clause cI = clausesA[i];
				if (null != tracer) {
					tracer.stepOuterFor(cI);
				}
				for (int j = i; j < clausesA.Length; j++) {
					Clause cJ = clausesA[j];

					if (null != tracer) {
						tracer.stepInnerFor(cI, cJ);
					}

					// resolvent <- FOL-RESOLVE(Ci, Cj)
					Set<Clause> resolvents = cI.binaryResolvents(cJ);

					if (resolvents.Count > 0) {
						toAdd.Clear();
						// new <- new <UNION> resolvent
						for (Clause rc : resolvents) {
							toAdd.addAll(rc.getFactors());
						}

						if (null != tracer) {
							tracer.stepResolved(cI, cJ, toAdd);
						}

						ansHandler.checkForPossibleAnswers(toAdd);

						if (ansHandler.isComplete()) {
							break;
						}

						newClauses.addAll(toAdd);
					}

					if (ansHandler.isComplete()) {
						break;
					}
				}
				if (ansHandler.isComplete()) {
					break;
				}
			}

			noOfPrevClauses = clauses.Count;

			// clauses <- clauses <UNION> new
			clauses.addAll(newClauses);

			if (ansHandler.isComplete()) {
				break;
			}

			// if new is a <SUBSET> of clauses then finished
			// searching for an answer
			// (i.e. when they were added the # clauses
			// did not increase).
		} while (noOfPrevClauses < clauses.Count);

		if (null != tracer) {
			tracer.stepFinished(clauses, ansHandler);
		}

		return ansHandler;
	}

	// END-InferenceProcedure
	//

	//
	// PRIVATE METHODS
	//
	class TFMAnswerHandler : InferenceResult {
		private Literal answerLiteral = null;
		private ISet<Variable> answerLiteralVariables = null;
		private Clause answerClause = null;
		private long finishTime = 0L;
		private bool complete = false;
		private List<Proof> proofs = new List<Proof>();
		private bool timedOut = false;

		public TFMAnswerHandler(Literal answerLiteral,
				Set<Variable> answerLiteralVariables, Clause answerClause,
				long maxQueryTime) {
			this.answerLiteral = answerLiteral;
			this.answerLiteralVariables = answerLiteralVariables;
			this.answerClause = answerClause;
			//
			this.finishTime = System.currentTimeMillis() + maxQueryTime;
		}

		//
		// START-InferenceResult
		public bool isPossiblyFalse() {
			return !timedOut && proofs.Count == 0;
		}

		public bool isTrue() {
			return proofs.Count > 0;
		}

		public bool isUnknownDueToTimeout() {
			return timedOut && proofs.Count == 0;
		}

		public bool isPartialResultDueToTimeout() {
			return timedOut && proofs.Count > 0;
		}

		public List<Proof> getProofs() {
			return proofs;
		}

		// END-InferenceResult
		//

		public bool isComplete() {
			return complete;
		}

		private void checkForPossibleAnswers(ISet<Clause> resolvents) {
			// If no bindings being looked for, then
			// is just a true false query.
			for (Clause aClause : resolvents) {
				if (answerClause.isEmpty()) {
					if (aClause.isEmpty()) {
						proofs.Add(new ProofFinal(aClause.getProofStep(),
								new Dictionary<Variable, Term>()));
						complete = true;
					}
				} else {
					if (aClause.isEmpty()) {
						// This should not happen
						// as added an answer literal, which
						// implies the database (i.e. premises) are
						// unsatisfiable to begin with.
						throw new IllegalStateException(
								"Generated an empty clause while looking for an answer, implies original KB is unsatisfiable");
					}

					if (aClause.isUnitClause()
							&& aClause.isDefiniteClause()
							&& aClause
									.getPositiveLiterals()
									.get(0)
									.getAtomicSentence()
									.getSymbolicName()
									 .Equals(answerLiteral.getAtomicSentence()
											.getSymbolicName())) {
						IDictionary<Variable, Term> answerBindings = new Dictionary<Variable, Term>();
						List<Term> answerTerms = aClause.getPositiveLiterals()
								.get(0).getAtomicSentence().getArgs();
						int idx = 0;
						for (Variable v : answerLiteralVariables) {
							answerBindings.Add(v, answerTerms.get(idx));
							idx++;
						}
						bool addNewAnswer = true;
						for (Proof p : proofs) {
							if (p.getAnswerBindings() .Equals(answerBindings)) {
								addNewAnswer = false;
								break;
							}
						}
						if (addNewAnswer) {
							proofs.Add(new ProofFinal(aClause.getProofStep(),
									answerBindings));
						}
					}
				}

				if (System.currentTimeMillis() > finishTime) {
					complete = true;
					// Indicate that I have run out of query time
					timedOut = true;
				}
			}
		}

		 
		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb.Append("isComplete=" + complete);
			sb.Append("\n");
			sb.Append("result=" + proofs);
			return sb.ToString();
		}
	}
}
