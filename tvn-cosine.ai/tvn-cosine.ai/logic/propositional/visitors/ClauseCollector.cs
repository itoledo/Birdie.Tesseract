 namespace aima.core.logic.propositional.visitors;

 
 
 
 

import aima.core.logic.propositional.kb.data.Clause;
import aima.core.logic.propositional.kb.data.Literal;
import aima.core.logic.propositional.parsing.ast.ComplexSentence;
import aima.core.logic.propositional.parsing.ast.PropositionSymbol;
import aima.core.logic.propositional.parsing.ast.Sentence;

/**
 * Utility class for collecting clauses from CNF Sentences.
 * 
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public class ClauseCollector : BasicGatherer<Clause> {

	/**
	 * Collect a set of clauses from a list of given sentences.
	 * 
	 * @param cnfSentences
	 *            a list of CNF sentences from which to collect clauses.
	 * @return a set of all contained clauses.
	 * @throws IllegalArgumentException
	 *             if any of the given sentences are not in CNF.
	 */
	public static ISet<Clause> getClausesFrom(Sentence... cnfSentences) {
		Set<Clause> result = new HashSet<Clause>();

		ClauseCollector clauseCollector = new ClauseCollector();
		for (Sentence cnfSentence : cnfSentences) {			
			result = cnfSentence.accept(clauseCollector, result);
		}	

		return result;
	}
	
	 
	public ISet<Clause> visitPropositionSymbol(PropositionSymbol s, ISet<Clause> arg) {
		// a positive unit clause
		Literal positiveLiteral = new Literal(s);
		arg.Add(new Clause(positiveLiteral));
		
		return arg;
	}
	
	 
	public ISet<Clause> visitUnarySentence(ComplexSentence s, ISet<Clause> arg) {
		
		if (!s.getSimplerSentence(0).isPropositionSymbol()) {
			throw new IllegalStateException("Sentence is not in CNF: "+s);
		}
		
		// a negative unit clause
		Literal negativeLiteral = new Literal((PropositionSymbol)s.getSimplerSentence(0), false);
		arg.Add(new Clause(negativeLiteral));
		
		return arg;
	}
	
	 
	public ISet<Clause> visitBinarySentence(ComplexSentence s, ISet<Clause> arg) {
		
		if (s.isAndSentence()) {
			s.getSimplerSentence(0).accept(this, arg);
			s.getSimplerSentence(1).accept(this, arg);			
		} else if (s.isOrSentence()) {
			List<Literal> literals = new List<Literal>(LiteralCollector.getLiterals(s));
			arg.Add(new Clause(literals));			
		} else {
			throw new ArgumentException("Sentence is not in CNF: "+s);
		}
		
		return arg;
	}

	//
	// PRIVATE
	//
	private static class LiteralCollector : BasicGatherer<Literal> {
		
		private static ISet<Literal> getLiterals(Sentence disjunctiveSentence) {
			Set<Literal> result = new HashSet<Literal>();
			
			LiteralCollector literalCollector = new LiteralCollector();
			result = disjunctiveSentence.accept(literalCollector, result);
			
			return result;
		}
		
		 
		public ISet<Literal> visitPropositionSymbol(PropositionSymbol s, ISet<Literal> arg) {
			// a positive literal
			Literal positiveLiteral = new Literal(s);
			arg.Add(positiveLiteral);
			
			return arg;
		}
		
		 
		public ISet<Literal> visitUnarySentence(ComplexSentence s, ISet<Literal> arg) {
			
			if (!s.getSimplerSentence(0).isPropositionSymbol()) {
				throw new IllegalStateException("Sentence is not in CNF: "+s);
			}
			
			// a negative literal
			Literal negativeLiteral = new Literal((PropositionSymbol)s.getSimplerSentence(0), false);

			arg.Add(negativeLiteral);
			
			return arg;
		}
		
		 
		public ISet<Literal> visitBinarySentence(ComplexSentence s, ISet<Literal> arg) {
			if (s.isOrSentence()) {
				s.getSimplerSentence(0).accept(this, arg);
				s.getSimplerSentence(1).accept(this, arg);
			} else {
				throw new ArgumentException("Sentence is not in CNF: "+s);
			}
			return arg;
		}
	}
}
