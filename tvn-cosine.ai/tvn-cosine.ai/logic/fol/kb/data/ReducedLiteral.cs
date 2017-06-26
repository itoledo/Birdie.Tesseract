 namespace aima.core.logic.fol.kb.data;

import aima.core.logic.fol.parsing.ast.AtomicSentence;

/**
 * @see <a
 *      href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture13.pdf"
 *      >Reduced Literal</a>
 * 
 * @author Ciaran O'Reilly
 * 
 */
public class ReducedLiteral : Literal {
	private string strRep = null;

	public ReducedLiteral(AtomicSentence atom) {
		base(atom);
	}

	public ReducedLiteral(AtomicSentence atom, bool negated) {
		base(atom, negated);
	}

	 
	public Literal newInstance(AtomicSentence atom) {
		return new ReducedLiteral(atom, isNegativeLiteral());
	}

	 
	public override string ToString() {
		if (null == strRep) {
			StringBuilder sb = new StringBuilder();
			sb.Append("[");
			if (isNegativeLiteral()) {
				sb.Append("~");
			}
			sb.Append(getAtomicSentence().ToString());
			sb.Append("]");
			strRep = sb.ToString();
		}

		return strRep;
	}
}