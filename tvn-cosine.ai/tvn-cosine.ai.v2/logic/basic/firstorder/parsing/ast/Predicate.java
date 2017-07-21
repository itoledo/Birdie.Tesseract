namespace aima.core.logic.basic.firstorder.parsing.ast;

using java.util.ArrayList;
using java.util.Collections;
using java.util.List;
using java.util.StringJoiner;

using aima.core.logic.basic.firstorder.parsing.FOLVisitor;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public class Predicate implements AtomicSentence {
	private String predicateName;
	private List<Term> terms = new ArrayList<Term>();
	private String stringRep = null;
	private int hashCode = 0;

	public Predicate(String predicateName, List<Term> terms) {
		this.predicateName = predicateName;
		this.terms.addAll(terms);
	}

	public String getPredicateName() {
		return predicateName;
	}

	public List<Term> getTerms() {
		return Collections.unmodifiableList(terms);
	}

	//
	// START-AtomicSentence
	public String getSymbolicName() {
		return getPredicateName();
	}

	public bool isCompound() {
		return true;
	}

	public List<Term> getArgs() {
		return getTerms();
	}

	public Object accept(FOLVisitor v, Object arg) {
		return v.visitPredicate(this, arg);
	}

	public Predicate copy() {
		List<Term> copyTerms = new ArrayList<Term>();
		for (Term t : terms) {
			copyTerms.add(t.copy());
		}
		return new Predicate(predicateName, copyTerms);
	}

	// END-AtomicSentence
	//

	 
	public override bool Equals(object o) {

		if (this == o) {
			return true;
		}
		if (!(o is Predicate)) {
			return false;
		}
		Predicate p = (Predicate) o;
		return p.getPredicateName().Equals(getPredicateName())
				&& p.getTerms().Equals(getTerms());
	}

	 
	public override int GetHashCode() {
		if (0 == hashCode) {
			hashCode = 17;
			hashCode = 37 * hashCode + predicateName.GetHashCode();
			for (Term t : terms) {
				hashCode = 37 * hashCode + t.GetHashCode();
			}
		}
		return hashCode;
	}

	 
	public override string ToString() {
		if (null == stringRep) {
			StringBuilder sb = new StringBuilder();
			sb.append(predicateName);
			StringJoiner sj = new StringJoiner(",", "(", ")");
			for (Term t : terms) {
				sj.add(t.ToString());
			}
			stringRep = sb.ToString() + sj.ToString();
			return stringRep;
		}

		return stringRep;
	}
}