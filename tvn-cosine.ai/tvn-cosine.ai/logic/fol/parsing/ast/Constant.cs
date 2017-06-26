 namespace aima.core.logic.fol.parsing.ast;

 

import aima.core.logic.fol.parsing.FOLVisitor;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public class Constant : Term {
	private string value;
	private int hashCode = 0;

	public Constant(string s) {
		value = s;
	}

	public string getValue() {
		return value;
	}

	//
	// START-Term
	public string getSymbolicName() {
		return getValue();
	}

	public bool isCompound() {
		return false;
	}

	public List<Term> getArgs() {
		// Is not Compound, therefore should
		// return null for its arguments
		return null;
	}

	public object accept(FOLVisitor v, object arg) {
		return v.visitConstant(this, arg);
	}

	public Constant copy() {
		return new Constant(value);
	}

	// END-Term
	//

	 
	public override bool Equals(object o) {

		if (this == o) {
			return true;
		}
		if (!(o is Constant)) {
			return false;
		}
		Constant c = (Constant) o;
		return c.getValue() .Equals(getValue());

	}

	 
	public override int GetHashCode() {
		if (0 == hashCode) {
			hashCode = 17;
			hashCode = 37 * hashCode + value .GetHashCode();
		}
		return hashCode;
	}

	 
	public override string ToString() {
		return value;
	}
}
