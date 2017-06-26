 namespace aima.core.nlp.parsing;

public class LexWord {
	String word;
	Float  prob;
	
	public LexWord( string word, Float prob ) {
		this.word = word;
		this.prob = prob;
	}
	
	public string getWord() { return word; }
	public Float getProb() { return prob; }
}
