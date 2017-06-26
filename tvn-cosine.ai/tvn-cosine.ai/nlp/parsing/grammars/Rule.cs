 namespace aima.core.nlp.parsing.grammars;

 
import java.util.Arrays;
 
import java.util.Objects;

/**
 * A derivation rule that is contained within a grammar. This rule is probabilistic, in that it 
 * has an associated probability representing the likelihood that the RHS follows from the LHS, given 
 * the presence of the LHS.
 * @author Jonathon (thundergolfer)
 *
 */
public class Rule {
	
	public final float PROB;
	public final List<string> lhs; // Left hand side of derivation rule
	public final List<string> rhs; // Right hand side of derivation rule
	
	// Basic constructor
	public Rule( List<string> lhs, List<string> rhs, float probability ) {
		this.lhs = lhs;
		this.rhs = rhs;
		this.PROB = validateProb( probability );
	}
	
	// null RHS rule constructor
	public Rule( List<string> lhs, float probability ) {
		this.lhs = lhs;
		this.rhs = null;
		this.PROB = validateProb( probability );
	}
	
	// string split constructor
	public Rule( string lhs, string rhs, float probability) {
		if( lhs .Equals("")) {
			this.lhs = new List<string>();
		} else {
			this.lhs = new List<string>(Arrays.asList(lhs.split("\\s*,\\s*")));
		}
		if( rhs .Equals("")) {
			this.rhs = new List<string>();
		} else {
			this.rhs = new List<string>(Arrays.asList(rhs.split("\\s*,\\s*")));
		}
		this.PROB = validateProb( probability );
		
	}
	
	/**
	 * Currently a hack to ensure rule has a valid probablity value.
	 * Don't really want to throw an exception.
	 */
	private float validateProb(float prob) {
		if( prob >= 0.0 && prob <= 1.0 ) {
			return prob;
		}
		else {
			return (float) 0.5; // probably should throw exception
		}
	}
	
	public bool derives( List<string> sentForm ) {
		if( this.rhs.Count != sentForm.Count ) {
			return false;
		}
		for( int i=0; i < sentForm.Count; ++i ) {
			if(!Objects .Equals(this.rhs.get(i), sentForm.get(i))) {
				return false;
			}
		}
		return true;
	}
	
	public bool derives( string terminal ) {
		if( this.rhs.Count == 1 && this.rhs.get(0) .Equals(terminal)) {
			return true;
		}
		return false;
	}
	
	  
	public override string ToString() {
		StringBuilder output = new StringBuilder();

		for (string lh : lhs) {
			output.Append(lh);
		}

		output.Append(" -> ");

		for (string rh : rhs) {
			output.Append(rh);
		}

		output.Append(" ").Append(String.valueOf(PROB));

		return output.ToString();
	}
}
