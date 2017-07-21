namespace aima.core.logic.basic.firstorder.parsing.ast;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import aima.core.logic.basic.firstorder.parsing.FOLVisitor;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public class QuantifiedSentence implements Sentence {
	private String quantifier;
	private List<Variable> variables = new ArrayList<Variable>();
	private Sentence quantified;
	private List<FOLNode> args = new ArrayList<FOLNode>();
	private String stringRep = null;
	private int hashCode = 0;

	public QuantifiedSentence(String quantifier, List<Variable> variables,
			Sentence quantified) {
		this.quantifier = quantifier;
		this.variables.addAll(variables);
		this.quantified = quantified;
		this.args.addAll(variables);
		this.args.add(quantified);
	}

	public String getQuantifier() {
		return quantifier;
	}

	public List<Variable> getVariables() {
		return Collections.unmodifiableList(variables);
	}

	public Sentence getQuantified() {
		return quantified;
	}

	//
	// START-Sentence
	public String getSymbolicName() {
		return getQuantifier();
	}

	public boolean isCompound() {
		return true;
	}

	public List<FOLNode> getArgs() {
		return Collections.unmodifiableList(args);
	}

	public Object accept(FOLVisitor v, Object arg) {
		return v.visitQuantifiedSentence(this, arg);
	}

	public QuantifiedSentence copy() {
		List<Variable> copyVars = new ArrayList<Variable>();
		for (Variable v : variables) {
			copyVars.add(v.copy());
		}
		return new QuantifiedSentence(quantifier, copyVars, quantified.copy());
	}

	// END-Sentence
	//

	 
	public override bool Equals(object o) {

		if (this == o) {
			return true;
		}
		if ((o == null) || (this.getClass() != o.getClass())) {
			return false;
		}
		QuantifiedSentence cs = (QuantifiedSentence) o;
		return cs.quantifier.Equals(quantifier)
				&& cs.variables.Equals(variables)
				&& cs.quantified.Equals(quantified);
	}

	 
	public override int GetHashCode() {
		if (0 == hashCode) {
			hashCode = 17;
			hashCode = 37 * hashCode + quantifier.GetHashCode();
			for (Variable v : variables) {
				hashCode = 37 * hashCode + v.GetHashCode();
			}
			hashCode = hashCode * 37 + quantified.GetHashCode();
		}
		return hashCode;
	}

	 
	public override string ToString() {
		if (null == stringRep) {
			StringBuilder sb = new StringBuilder();
			sb.append(quantifier);
			sb.append(" ");
			for (Variable v : variables) {
				sb.append(v.ToString());
				sb.append(" ");
			}
			sb.append(quantified.ToString());
			stringRep = sb.ToString();
		}
		return stringRep;
	}
}