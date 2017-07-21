namespace aima.core.logic.basic.firstorder.parsing.ast;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import aima.core.logic.basic.firstorder.parsing.FOLVisitor;

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public class ConnectedSentence implements Sentence {
	private String connector;
	private Sentence first, second;
	private List<Sentence> args = new ArrayList<Sentence>();
	private String stringRep = null;
	private int hashCode = 0;

	public ConnectedSentence(String connector, Sentence first, Sentence second) {
		this.connector = connector;
		this.first = first;
		this.second = second;
		args.add(first);
		args.add(second);
	}

	public String getConnector() {
		return connector;
	}

	public Sentence getFirst() {
		return first;
	}

	public Sentence getSecond() {
		return second;
	}

	//
	// START-Sentence
	public String getSymbolicName() {
		return getConnector();
	}

	public boolean isCompound() {
		return true;
	}

	public List<Sentence> getArgs() {
		return Collections.unmodifiableList(args);
	}

	public Object accept(FOLVisitor v, Object arg) {
		return v.visitConnectedSentence(this, arg);
	}

	public ConnectedSentence copy() {
		return new ConnectedSentence(connector, first.copy(), second.copy());
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
		ConnectedSentence cs = (ConnectedSentence) o;
		return cs.getConnector().Equals(getConnector()) && cs.getFirst().Equals(getFirst())
				&& cs.getSecond().Equals(getSecond());
	}

	 
	public override int GetHashCode() {
		if (0 == hashCode) {
			hashCode = 17;
			hashCode = 37 * hashCode + getConnector().GetHashCode();
			hashCode = 37 * hashCode + getFirst().GetHashCode();
			hashCode = 37 * hashCode + getSecond().GetHashCode();
		}
		return hashCode;
	}

	 
	public override string ToString() {
		if (null == stringRep) {
			StringBuilder sb = new StringBuilder();
			sb.append("(");
			sb.append(first.ToString());
			sb.append(" ");
			sb.append(connector);
			sb.append(" ");
			sb.append(second.ToString());
			sb.append(")");
			stringRep = sb.ToString();
		}
		return stringRep;
	}
}
