namespace aima.core.logic.basic.firstorder.parsing.ast;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import aima.core.logic.basic.firstorder.parsing.FOLVisitor;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public class TermEquality implements AtomicSentence {
	private Term term1, term2;
	private List<Term> terms = new ArrayList<Term>();
	private String stringRep = null;
	private int hashCode = 0;

	public static String getEqualitySynbol() {
		return "=";
	}

	public TermEquality(Term term1, Term term2) {
		this.term1 = term1;
		this.term2 = term2;
		terms.add(term1);
		terms.add(term2);
	}

	public Term getTerm1() {
		return term1;
	}

	public Term getTerm2() {
		return term2;
	}

	//
	// START-AtomicSentence
	public String getSymbolicName() {
		return getEqualitySynbol();
	}

	public boolean isCompound() {
		return true;
	}

	public List<Term> getArgs() {
		return Collections.unmodifiableList(terms);
	}

	public Object accept(FOLVisitor v, Object arg) {
		return v.visitTermEquality(this, arg);
	}

	public TermEquality copy() {
		return new TermEquality(term1.copy(), term2.copy());
	}

	// END-AtomicSentence
	//

	 
	public override bool Equals(object o) {

		if (this == o) {
			return true;
		}
		if ((o == null) || (this.getClass() != o.getClass())) {
			return false;
		}
		TermEquality te = (TermEquality) o;

		return te.getTerm1().Equals(term1) && te.getTerm2().Equals(term2);
	}

	 
	public override int GetHashCode() {
		if (0 == hashCode) {
			hashCode = 17;
			hashCode = 37 * hashCode + getTerm1().GetHashCode();
			hashCode = 37 * hashCode + getTerm2().GetHashCode();
		}
		return hashCode;
	}

	 
	public override string ToString() {
		if (null == stringRep) {
			StringBuilder sb = new StringBuilder();
			sb.append(term1.ToString());
			sb.append(" = ");
			sb.append(term2.ToString());
			stringRep = sb.ToString();
		}
		return stringRep;
	}
}