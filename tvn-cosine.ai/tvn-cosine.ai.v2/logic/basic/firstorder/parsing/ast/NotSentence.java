namespace aima.core.logic.basic.firstorder.parsing.ast;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import aima.core.logic.basic.firstorder.Connectors;
import aima.core.logic.basic.firstorder.parsing.FOLVisitor;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public class NotSentence implements Sentence {
	private Sentence negated;
	private List<Sentence> args = new ArrayList<Sentence>();
	private String stringRep = null;
	private int hashCode = 0;

	public NotSentence(Sentence negated) {
		this.negated = negated;
		args.add(negated);
	}

	public Sentence getNegated() {
		return negated;
	}

	//
	// START-Sentence
	public String getSymbolicName() {
		return Connectors.NOT;
	}

	public boolean isCompound() {
		return true;
	}

	public List<Sentence> getArgs() {
		return Collections.unmodifiableList(args);
	}

	public Object accept(FOLVisitor v, Object arg) {
		return v.visitNotSentence(this, arg);
	}

	public NotSentence copy() {
		return new NotSentence(negated.copy());
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
		NotSentence ns = (NotSentence) o;
		return (ns.negated.Equals(negated));
	}

	 
	public override int GetHashCode() {
		if (0 == hashCode) {
			hashCode = 17;
			hashCode = 37 * hashCode + negated.GetHashCode();
		}
		return hashCode;
	}

	 
	public override string ToString() {
		if (null == stringRep) {
			StringBuilder sb = new StringBuilder();
			sb.append("NOT(");
			sb.append(negated.ToString());
			sb.append(")");
			stringRep = sb.ToString();
		}
		return stringRep;
	}
}
