 namespace aima.core.logic.fol.parsing.ast;

 
 
 

import aima.core.logic.fol.parsing.FOLVisitor;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public class Function : Term {
	private string functionName;
	private List<Term> terms = new List<Term>();
	private string stringRep = null;
	private int hashCode = 0;

	public Function(string functionName, List<Term> terms) {
		this.functionName = functionName;
		this.terms.addAll(terms);
	}

	public string getFunctionName() {
		return functionName;
	}

	public List<Term> getTerms() {
		return Collections.unmodifiableList(terms);
	}

	//
	// START-Term
	public string getSymbolicName() {
		return getFunctionName();
	}

	public bool isCompound() {
		return true;
	}

	public List<Term> getArgs() {
		return getTerms();
	}

	public object accept(FOLVisitor v, object arg) {
		return v.visitFunction(this, arg);
	}

	public Function copy() {
		List<Term> copyTerms = new List<Term>();
		for (Term t : terms) {
			copyTerms.Add(t.copy());
		}
		return new Function(functionName, copyTerms);
	}

	// END-Term
	//

	 
	public override bool Equals(object o) {

		if (this == o) {
			return true;
		}
		if (!(o is Function)) {
			return false;
		}

		Function f = (Function) o;

		return f.getFunctionName() .Equals(getFunctionName())
				&& f.getTerms() .Equals(getTerms());
	}

	 
	public override int GetHashCode() {
		if (0 == hashCode) {
			hashCode = 17;
			hashCode = 37 * hashCode + functionName .GetHashCode();
			for (Term t : terms) {
				hashCode = 37 * hashCode + t .GetHashCode();
			}
		}
		return hashCode;
	}

	 
	public override string ToString() {
		if (null == stringRep) {
			StringBuilder sb = new StringBuilder();
			sb.Append(functionName);
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