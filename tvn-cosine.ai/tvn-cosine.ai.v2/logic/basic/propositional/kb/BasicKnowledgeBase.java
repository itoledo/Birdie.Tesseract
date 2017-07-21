namespace aima.core.logic.basic.propositional.kb;

using java.util.ArrayList;
using java.util.Arrays;
using java.util.Collections;
using java.util.LinkedHashSet;
using java.util.List;
using java.util.Set;

using aima.core.logic.api.propositional.KnowledgeBase;
using aima.core.logic.basic.propositional.kb.data.Clause;
using aima.core.logic.basic.propositional.kb.data.ConjunctionOfClauses;
using aima.core.logic.basic.propositional.parsing.PLParser;
using aima.core.logic.basic.propositional.parsing.ast.PropositionSymbol;
using aima.core.logic.basic.propositional.parsing.ast.Sentence;
using aima.core.logic.basic.propositional.visitors.ConvertToConjunctionOfClauses;
using aima.core.logic.basic.propositional.visitors.SymbolCollector;

/**
 * @author Ravi Mohan
 * @author Mike Stampone
 * @author Anurag Rai
 * 
 */
public class BasicKnowledgeBase implements KnowledgeBase {
	private List<Sentence>         sentences = new ArrayList<Sentence>();
	private ConjunctionOfClauses   asCNF     = new ConjunctionOfClauses(Collections.<Clause>emptySet());
	private ISet<PropositionSymbol> symbols   = new HashSet<PropositionSymbol>();
	private PLParser               parser;
	
	public BasicKnowledgeBase(PLParser plparser) {
		parser = plparser;
	}
	
	 
	public void tell(String aSentence) {
		tell((Sentence) parser.parse(aSentence));	
	}
	
	/**
	 * Adds the specified sentence to the knowledge base.
	 * 
	 * @param aSentence
	 *            a fact to be added to the knowledge base.
	 */
	public void tell(Sentence aSentence) {
		if (!(sentences.contains(aSentence))) {
			sentences.add(aSentence);
			asCNF = asCNF.extend(ConvertToConjunctionOfClauses.convert(aSentence).getClauses());
			symbols.addAll(SymbolCollector.getSymbolsFrom(aSentence));
		}
	}

	/**
	 * Each time the agent program is called, it TELLS the knowledge base what
	 * it perceives.
	 * 
	 * @param percepts
	 *            what the agent perceives
	 */
	public void tellAll(String[] percepts) {
		Arrays.stream(percepts).forEach( percept -> tell(percept) );
	}

	public Sentence asSentence() {
		return Sentence.newConjunction(sentences);
	}
	
	/**
	 * Returns the number of sentences in the knowledge base.
	 * 
	 * @return the number of sentences in the knowledge base.
	 */
	public int size() {
		return sentences.size();
	}
	
	 
	public override string ToString() {
		if (sentences.size() == 0) {
			return "";
		} else {
			return asSentence().ToString();
		}
	}


	 
	public List<Sentence> getSentences() {
		return sentences;
	}
}