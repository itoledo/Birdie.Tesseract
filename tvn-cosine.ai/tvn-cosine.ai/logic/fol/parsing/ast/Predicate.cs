 namespace aima.core.logic.fol.parsing.ast;

 
 
 

import aima.core.logic.fol.parsing.FOLVisitor;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public class Predicate : AtomicSentence {
	private string predicateName;
	private List<Term> terms = new List<Term>();
	private string stringRep = null;
	private int hashCode = 0;

	public Predicate(string predicateName, List<Term> terms) {
		this.predicateName = predicateName;
		this.terms.addAll(terms);
	}

	public string getPredicateName() {
		return predicateName;
	}

	public List<Term> getTerms() {
		return Collections.unmodifiableList(terms);
	}

	//
	// START-AtomicSentence
	public string getSymbolicName() {
		return getPredicateName();
	}

	public bool isCompound() {
		return true;
	}

	public List<Term> getArgs() {
		return getTerms();
	}

	public object accept(FOLVisitor v, object arg) {
		return v.visitPredicate(this, arg);
	}

	public Predicate copy() {
		List<Term> copyTerms = new List<Term>();
		for (Term t : terms) {
			copyTerms.Add(t.copy());
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
		return p.getPredicateName() .Equals(getPredicateName())
				&& p.getTerms() .Equals(getTerms());
	}

	 
	public override int GetHashCode() {
		if (0 == hashCode) {
			hashCode = 17;
			hashCode = 37 * hashCode + predicateName .GetHashCode();
			for (Term t : terms) {
				hashCode = 37 * hashCode + t .GetHashCode();
			}
		}
		return hashCode;
	}

	 
	public override string ToString() {
		if (null == stringRep) {
			StringBuilder sb = new StringBuilder();
			sb.Append(predicateName);
			sb.Append("(");

			bool first = true;
			for (Term t : terms) {
				if (first) {
					first = false;
				} else {
					sb.Append(",");
				}
				sb.Append(t.ToString());
			}

			sb.Append(")");
			stringRep = sb.ToString();
		}

		return stringRep;
	}
}