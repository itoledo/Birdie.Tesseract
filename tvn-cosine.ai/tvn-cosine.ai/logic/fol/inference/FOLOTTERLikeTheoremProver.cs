 namespace aima.core.logic.fol.inference;

 
 
import java.util.HashSet;
 
 
 
 

import aima.core.logic.fol.Connectors;
import aima.core.logic.fol.SubsumptionElimination;
import aima.core.logic.fol.inference.otter.ClauseFilter;
import aima.core.logic.fol.inference.otter.ClauseSimplifier;
import aima.core.logic.fol.inference.otter.LightestClauseHeuristic;
import aima.core.logic.fol.inference.otter.defaultimpl.DefaultClauseFilter;
import aima.core.logic.fol.inference.otter.defaultimpl.DefaultClauseSimplifier;
import aima.core.logic.fol.inference.otter.defaultimpl.DefaultLightestClauseHeuristic;
import aima.core.logic.fol.inference.proof.Proof;
import aima.core.logic.fol.inference.proof.ProofFinal;
import aima.core.logic.fol.inference.proof.ProofStepGoal;
import aima.core.logic.fol.kb.FOLKnowledgeBase;
import aima.core.logic.fol.kb.data.Clause;
import aima.core.logic.fol.kb.data.Literal;
import aima.core.logic.fol.parsing.ast.ConnectedSentence;
import aima.core.logic.fol.parsing.ast.NotSentence;
import aima.core.logic.fol.parsing.ast.Sentence;
import aima.core.logic.fol.parsing.ast.Term;
import aima.core.logic.fol.parsing.ast.TermEquality;
import aima.core.logic.fol.parsing.ast.Variable;

/**
 * Artificial Intelligence A Modern Approach (2nd Edition): Figure 9.14, page
 * 307.<br>
 * <br>
 * 
 * <pre>
 * procedure OTTER(sos, usable)
 *   inputs: sos, a set of support-clauses defining the problem (a global variable)
 *   usable, background knowledge potentially relevant to the problem
 *   
 *   repeat
 *      clause <- the lightest member of sos
 *      move clause from sos to usable
 *      PROCESS(INFER(clause, usable), sos)
 *   until sos = [] or a refutation has been found
 * 
 * --------------------------------------------------------------------------------
 * 
 * function INFER(clause, usable) returns clauses
 *   
 *   resolve clause with each member of usable
 *   return the resulting clauses after applying filter
 *   
 * --------------------------------------------------------------------------------
 * 
 * procedure PROCESS(clauses, sos)
 * 
 *   for each clause in clauses do
 *       clause <- SIMPLIFY(clause)
 *       merge identical literals
 *       discard clause if it is a tautology
 *       sos <- [clause | sos]
 *       if clause has no literals then a refutation has been found
 *       if clause has one literal then look for unit refutation
 * </pre>
 * 
 * Figure 9.14 Sketch of the OTTER theorem prover. Heuristic control is applied
 * in the selection of the "lightest" clause and in the FILTER function that
 * eliminates uninteresting clauses from consideration.<br>
 * <br>
 * <b>Note:</b> The original implementation of OTTER has been retired but its
 * successor, <b>Prover9</b>, can be found at:<br>
 * <a href="http://www.prover9.org/">http://www.prover9.org/</a><br>
 * or<br>
 * <a href="http://www.cs.unm.edu/~mccune/mace4/">http://www.cs.unm.edu/~mccune/
 * mace4/</a><br>
 * Should you wish to play with a mature implementation of a theorem prover :-)<br>
 * <br>
 * For lots of interesting problems to play with, see <b>The TPTP Problem
 * Library for Automated Theorem Proving</b>:<br>
 * <a href="http://www.cs.miami.edu/~tptp/">http://www.cs.miami.edu/~tptp/</a><br>
 * 
 * @author Ciaran O'Reilly
 * 
 */
public class FOLOTTERLikeTheoremProver : InferenceProcedure {
	//
	// Ten seconds is default maximum query time permitted
	private long maxQueryTime = 10 * 1000;
	private bool useParamodulation = true;
	private LightestClauseHeuristic lightestClauseHeuristic = new DefaultLightestClauseHeuristic();
	private ClauseFilter clauseFilter = new DefaultClauseFilter();
	private ClauseSimplifier clauseSimplifier = new DefaultClauseSimplifier();
	//
	private Paramodulation paramodulation = new Paramodulation();

	public FOLOTTERLikeTheoremProver() {

	}

	public FOLOTTERLikeTheoremProver(long maxQueryTime) {
		setMaxQueryTime(maxQueryTime);
	}

	public FOLOTTERLikeTheoremProver(bool useParamodulation) {
		setUseParamodulation(useParamodulation);
	}

	public FOLOTTERLikeTheoremProver(long maxQueryTime,
			bool useParamodulation) {
		setMaxQueryTime(maxQueryTime);
		setUseParamodulation(useParamodulation);
	}

	public long getMaxQueryTime() {
		return maxQueryTime;
	}

	public void setMaxQueryTime(long maxQueryTime) {
		this.maxQueryTime = maxQueryTime;
	}

	public bool isUseParamodulation() {
		return useParamodulation;
	}

	public void setUseParamodulation(bool useParamodulation) {
		this.useParamodulation = useParamodulation;
	}

	public LightestClauseHeuristic getLightestClauseHeuristic() {
		return lightestClauseHeuristic;
	}

	public void setLightestClauseHeuristic(
			LightestClauseHeuristic lightestClauseHeuristic) {
		this.lightestClauseHeuristic = lightestClauseHeuristic;
	}

	public ClauseFilter getClauseFilter() {
		return clauseFilter;
	}

	public void setClauseFilter(ClauseFilter clauseFilter) {
		this.clauseFilter = clauseFilter;
	}

	public ClauseSimplifier getClauseSimplifier() {
		return clauseSimplifier;
	}

	public void setClauseSimplifier(ClauseSimplifier clauseSimplifier) {
		this.clauseSimplifier = clauseSimplifier;
	}

	//
	// START-InferenceProcedure
	public InferenceResult ask(FOLKnowledgeBase KB, Sentence alpha) {
		Set<Clause> sos = new HashSet<Clause>();
		Set<Clause> usable = new HashSet<Clause>();

		// Usable set will be the set of clauses in the KB,
		// are assuming this is satisfiable as using the
		// Set of Support strategy.
		for (Clause c : KB.getAllClauses()) {
			c = KB.standardizeApart(c);
			c.setStandardizedApartCheckNotRequired();
			usable.addAll(c.getFactors());
		}

		// Ensure reflexivity axiom is added to usable if using paramodulation.
		if (isUseParamodulation()) {
			// Reflexivity Axiom: x = x
			TermEquality reflexivityAxiom = new TermEquality(new Variable("x"),
					new Variable("x"));
			Clause reflexivityClause = new Clause();
			reflexivityClause.addLiteral(new Literal(reflexivityAxiom));
			reflexivityClause = KB.standardizeApart(reflexivityClause);
			reflexivityClause.setStandardizedApartCheckNotRequired();
			usable.Add(reflexivityClause);
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
				sos.addAll(c.getFactors());
			}

			answerClause.addLiteral(answerLiteral);
		} else {
			for (Clause c : KB.convertToClauses(notAlpha)) {
				c = KB.standardizeApart(c);
				c.setProofStep(new ProofStepGoal(c));
				c.setStandardizedApartCheckNotRequired();
				sos.addAll(c.getFactors());
			}
		}

		// Ensure all subsumed clauses are removed
		usable.removeAll(SubsumptionElimination.findSubsumedClauses(usable));
		sos.removeAll(SubsumptionElimination.findSubsumedClauses(sos));

		OTTERAnswerHandler ansHandler = new OTTERAnswerHandler(answerLiteral,
				answerLiteralVariables, answerClause, maxQueryTime);

		IndexedClauses idxdClauses = new IndexedClauses(
				getLightestClauseHeuristic(), sos, usable);

		return otter(ansHandler, idxdClauses, sos, usable);
	}

	// END-InferenceProcedure
	//

	/**
	 * <pre>
	 * procedure OTTER(sos, usable) 
	 *   inputs: sos, a set of support-clauses defining the problem (a global variable) 
	 *   usable, background knowledge potentially relevant to the problem
	 * </pre>
	 */
	private InferenceResult otter(OTTERAnswerHandler ansHandler,
			IndexedClauses idxdClauses, ISet<Clause> sos, ISet<Clause> usable) {

		getLightestClauseHeuristic().initialSOS(sos);

		// * repeat
		do {
			// * clause <- the lightest member of sos
			Clause clause = getLightestClauseHeuristic().getLightestClause();
			if (null != clause) {
				// * move clause from sos to usable
				sos.Remove(clause);
				getLightestClauseHeuristic().removedClauseFromSOS(clause);
				usable.Add(clause);
				// * PROCESS(INFER(clause, usable), sos)
				process(ansHandler, idxdClauses, infer(clause, usable), sos,
						usable);
			}

			// * until sos = [] or a refutation has been found
		} while (sos.Count != 0 && !ansHandler.isComplete());

		return ansHandler;
	}

	/**
	 * <pre>
	 * function INFER(clause, usable) returns clauses
	 */
	private ISet<Clause> infer(Clause clause, ISet<Clause> usable) {
		Set<Clause> resultingClauses = new HashSet<Clause>();

		// * resolve clause with each member of usable
		for (Clause c : usable) {
			Set<Clause> resolvents = clause.binaryResolvents(c);
			for (Clause rc : resolvents) {
				resultingClauses.Add(rc);
			}

			// if using paramodulation to handle equality
			if (isUseParamodulation()) {
				Set<Clause> paras = paramodulation.apply(clause, c, true);
				for (Clause p : paras) {
					resultingClauses.Add(p);
				}
			}
		}

		// * return the resulting clauses after applying filter
		return getClauseFilter().filter(resultingClauses);
	}

	// procedure PROCESS(clauses, sos)
	private void process(OTTERAnswerHandler ansHandler,
			IndexedClauses idxdClauses, ISet<Clause> clauses, ISet<Clause> sos,
			Set<Clause> usable) {

		// * for each clause in clauses do
		for (Clause clause : clauses) {
			// * clause <- SIMPLIFY(clause)
			clause = getClauseSimplifier().simplify(clause);

			// * merge identical literals
			// Note: Not required as handled by Clause Implementation
			// which keeps literals within a Set, so no duplicates
			// will exist.

			// * discard clause if it is a tautology
			if (clause.isTautology()) {
				continue;
			}

			// * if clause has no literals then a refutation has been found
			// or if it just contains the answer literal.
			if (!ansHandler.isAnswer(clause)) {
				// * sos <- [clause | sos]
				// This check ensure duplicate clauses are not
				// introduced which will cause the
				// LightestClauseHeuristic to loop continuously
				// on the same pair of objects.
				if (!sos.Contains(clause) && !usable.Contains(clause)) {
					for (Clause ac : clause.getFactors()) {
						if (!sos.Contains(ac) && !usable.Contains(ac)) {
							idxdClauses.addClause(ac, sos, usable);

							// * if clause has one literal then look for unit
							// refutation
							lookForUnitRefutation(ansHandler, idxdClauses, ac,
									sos, usable);
						}
					}
				}
			}

			if (ansHandler.isComplete()) {
				break;
			}
		}
	}

	private void lookForUnitRefutation(OTTERAnswerHandler ansHandler,
			IndexedClauses idxdClauses, Clause clause, ISet<Clause> sos,
			Set<Clause> usable) {

		Set<Clause> toCheck = new HashSet<Clause>();

		if (ansHandler.isCheckForUnitRefutation(clause)) {
			for (Clause s : sos) {
				if (s.isUnitClause()) {
					toCheck.Add(s);
				}
			}
			for (Clause u : usable) {
				if (u.isUnitClause()) {
					toCheck.Add(u);
				}
			}
		}

		if (toCheck.Count > 0) {
			toCheck = infer(clause, toCheck);
			for (Clause t : toCheck) {
				// * clause <- SIMPLIFY(clause)
				t = getClauseSimplifier().simplify(t);

				// * discard clause if it is a tautology
				if (t.isTautology()) {
					continue;
				}

				// * if clause has no literals then a refutation has been found
				// or if it just contains the answer literal.
				if (!ansHandler.isAnswer(t)) {
					// * sos <- [clause | sos]
					// This check ensure duplicate clauses are not
					// introduced which will cause the
					// LightestClauseHeuristic to loop continuously
					// on the same pair of objects.
					if (!sos.Contains(t) && !usable.Contains(t)) {
						idxdClauses.addClause(t, sos, usable);
					}
				}

				if (ansHandler.isComplete()) {
					break;
				}
			}
		}
	}

	// This is a simple indexing on the clauses to support
	// more efficient forward and backward subsumption testing.
	class IndexedClauses {
		private LightestClauseHeuristic lightestClauseHeuristic = null;
		// Group the clauses by their # of literals.
		private IDictionary<Integer, ISet<Clause>> clausesGroupedBySize = new Dictionary<Integer, ISet<Clause>>();
		// Keep track of the min and max # of literals.
		private int minNoLiterals = Integer.MAX_VALUE;
		private int maxNoLiterals = 0;

		public IndexedClauses(LightestClauseHeuristic lightestClauseHeuristic,
				Set<Clause> sos, ISet<Clause> usable) {
			this.lightestClauseHeuristic = lightestClauseHeuristic;
			for (Clause c : sos) {
				indexClause(c);
			}
			for (Clause c : usable) {
				indexClause(c);
			}
		}

		public void addClause(Clause c, ISet<Clause> sos, ISet<Clause> usable) {
			// Perform forward subsumption elimination
			bool addToSOS = true;
			for (int i = minNoLiterals; i < c.getNumberLiterals(); ++i) {
				Set<Clause> fs = clausesGroupedBySize.get(i);
				if (null != fs) {
					for (Clause s : fs) {
						if (s.subsumes(c)) {
							addToSOS = false;
							break;
						}
					}
				}
				if (!addToSOS) {
					break;
				}
			}

			if (addToSOS) {
				sos.Add(c);
				lightestClauseHeuristic.addedClauseToSOS(c);
				indexClause(c);
				// Have added clause, therefore
				// perform backward subsumption elimination
				Set<Clause> subsumed = new HashSet<Clause>();
				for (int i = c.getNumberLiterals() + 1; i <= maxNoLiterals; ++i) {
					subsumed.Clear();
					Set<Clause> bs = clausesGroupedBySize.get(i);
					if (null != bs) {
						for (Clause s : bs) {
							if (c.subsumes(s)) {
								subsumed.Add(s);
								if (sos.Contains(s)) {
									sos.Remove(s);
									lightestClauseHeuristic
											.removedClauseFromSOS(s);
								}
								usable.Remove(s);
							}
						}
						bs.removeAll(subsumed);
					}
				}
			}
		}

		//
		// PRIVATE METHODS
		//
		private void indexClause(Clause c) {
			int size = c.getNumberLiterals();
			if (size < minNoLiterals) {
				minNoLiterals = size;
			}
			if (size > maxNoLiterals) {
				maxNoLiterals = size;
			}
			Set<Clause> cforsize = clausesGroupedBySize.get(size);
			if (null == cforsize) {
				cforsize = new HashSet<Clause>();
				clausesGroupedBySize.Add(size, cforsize);
			}
			cforsize.Add(c);
		}
	}

	class OTTERAnswerHandler : InferenceResult {
		private Literal answerLiteral = null;
		private ISet<Variable> answerLiteralVariables = null;
		private Clause answerClause = null;
		private long finishTime = 0L;
		private bool complete = false;
		private List<Proof> proofs = new List<Proof>();
		private bool timedOut = false;

		public OTTERAnswerHandler(Literal answerLiteral,
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

		public bool isLookingForAnswerLiteral() {
			return !answerClause.isEmpty();
		}

		public bool isCheckForUnitRefutation(Clause clause) {

			if (isLookingForAnswerLiteral()) {
				if (2 == clause.getNumberLiterals()) {
					for (Literal t : clause.getLiterals()) {
						if (t.getAtomicSentence()
								.getSymbolicName()
								 .Equals(answerLiteral.getAtomicSentence()
										.getSymbolicName())) {
							return true;
						}
					}
				}
			} else {
				return clause.isUnitClause();
			}

			return false;
		}

		public bool isAnswer(Clause clause) {
			bool isAns = false;

			if (answerClause.isEmpty()) {
				if (clause.isEmpty()) {
					proofs.Add(new ProofFinal(clause.getProofStep(),
							new Dictionary<Variable, Term>()));
					complete = true;
					isAns = true;
				}
			} else {
				if (clause.isEmpty()) {
					// This should not happen
					// as added an answer literal to sos, which
					// implies the database (i.e. premises) are
					// unsatisfiable to begin with.
					throw new IllegalStateException(
							"Generated an empty clause while looking for an answer, implies original KB or usable is unsatisfiable");
				}

				if (clause.isUnitClause()
						&& clause.isDefiniteClause()
						&& clause
								.getPositiveLiterals()
								.get(0)
								.getAtomicSentence()
								.getSymbolicName()
								 .Equals(answerLiteral.getAtomicSentence()
										.getSymbolicName())) {
					IDictionary<Variable, Term> answerBindings = new Dictionary<Variable, Term>();
					List<Term> answerTerms = clause.getPositiveLiterals()
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
						proofs.Add(new ProofFinal(clause.getProofStep(),
								answerBindings));
					}
					isAns = true;
				}
			}

			if (System.currentTimeMillis() > finishTime) {
				complete = true;
				// Indicate that I have run out of query time
				timedOut = true;
			}

			return isAns;
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
