 namespace aima.core.nlp.parsing;

 
import java.util.Arrays;
 
 
 
 

import aima.core.nlp.parsing.grammars.Rule;

/**
 * The Lexicon object appears on pg. 891 of the text and defines a simple
 * set of words for a certain language category and their associated probabilities.
 * 
 * Defining and using a lexicon saves us from listing out a large number of rules to
 * derive terminal strings in a grammar.
 * 
 * @author Jonathon
 *
 */
public class Lexicon : Dictionary<String, List<LexWord>> {

	private static readonly long serialVersionUID = 1L;

	public List<Rule> getTerminalRules( string partOfSpeech ) {
		 List<LexWord> lexWords = this.get(partOfSpeech.toUpperCase());
		 List<Rule> rules = new List<Rule>();
		if( lexWords.Count > 0) {
			for( int i=0; i < lexWords.Count; ++i ) {
				rules.Add( new Rule( partOfSpeech.toUpperCase(), 
						   			    lexWords.get(i).word, 
						   			    lexWords.get(i).prob));
			}	
		}
		return rules;
	}
	
	public List<Rule> getAllTerminalRules() {
		 List<Rule> allRules = new List<Rule>();
		Set<string> keys = this.Keys;
		Iterator<string> it = keys.iterator();
		while( it.hasNext() ) {
			String key = (string) it.next();
			allRules.addAll( this.getTerminalRules(key));
		}
		
		return allRules;
	}
	
	public bool addEntry( string category, string word, float prob ) {
		if( this.ContainsKey(category)) {
			this.get(category).Add( new LexWord( word, prob ));
		}
		else {
			this.Add(category, new List<LexWord>( Arrays.asList(new LexWord(word,prob))));
		}
		
		return true;
	}
	
	public bool addLexWords( String... vargs ) {
		
		String key; List<LexWord> lexWords = new List<LexWord>();
		bool containsKey = false;
		// number of arguments must be key (1) + lexWord pairs ( x * 2 )
		if( vargs.Length % 2 != 1 ) {
			return false;
		}
		key = vargs[0].toUpperCase();
		if( this.ContainsKey(key)) { containsKey = true; }
			
		for( int i=1; i < vargs.Length; ++i ) {
			try {
				if( containsKey ) {
					this.get(key).Add( new LexWord( vargs[i], Float.valueOf(vargs[i+1])));
				}
				else {
					lexWords.Add( new LexWord( vargs[i], Float.valueOf(vargs[i+1])));	
				}
				i++;
			} catch( NumberFormatException e ) {
				System.err.println("Supplied args have incorrect format.");
				return false;
			}
		}
		if( !containsKey ) { this.Add(key, lexWords); }
		return true;
		
	}
	
	/**
	 * Add words to an lexicon from an existing lexicon. Using this 
	 * you can combine lexicons.
	 * @param l
	 */
	public void addLexWords( Lexicon l ) {
		Iterator<Map.Entry<String, List<LexWord>>> it = l.entrySet().iterator();
		while(it.hasNext()) {
			Map.Entry<String, List<LexWord>> pair = it.next();
			if( this.ContainsKey( pair.Key)) {
				for( int i=0; i < pair.getValue().Count; ++i ) {
					this.get(pair.Key).Add(pair.getValue().get(i));
				}
			}
			else {
				this.Add(pair.Key, pair.getValue());
			}
		}
	}
}


