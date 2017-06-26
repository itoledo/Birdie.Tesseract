 namespace aima.core.logic.fol.parsing.ast;

 

import aima.core.logic.fol.parsing.FOLVisitor;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public class Variable : Term {
	private string value;
	private int hashCode = 0;
	private int indexical = -1;

	public Variable(string s) {
		value = s.trim();
	}

	public Variable(string s, int idx) {
		value = s.trim();
		indexical = idx;
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
		return v.visitVariable(this, arg);
	}

	public Variable copy() {
		return new Variable(value, indexical);
	}

	// END-Term
	//

	public int getIndexical() {
		return indexical;
	}

	public void setIndexical(int idx) {
		indexical = idx;
		hashCode = 0;
	}

	public string getIndexedValue() {
		return value + indexical;
	}

	 
	public override bool Equals(object o) {

		if (this == o) {
			return true;
		}
		if (!(o is Variable)) {
			return false;
		}

		Variable v = (Variable) o;
		return v.getValue() .Equals(getValue())
				&& v.getIndexical() == getIndexical();
	}

	 
	public override int GetHashCode() {
		if (0 == hashCode) {
			hashCode = 17;
			hashCode += indexical;
			hashCode = 37 * hashCode + value .GetHashCode();
		}

		return hashCode;
	}

	 
	public override string ToString() {
		return value;
	}
}