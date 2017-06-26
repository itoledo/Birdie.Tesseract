 namespace aima.core.logic.fol.kb.data;

 
 
 

/**
 * Conjunctive Normal Form (CNF) : a conjunction of clauses, where each clause
 * is a disjunction of literals.
 * 
 * @author Ciaran O'Reilly
 * 
 */
public class CNF {

	private List<Clause> conjunctionOfClauses = new List<Clause>();

	public CNF(List<Clause> conjunctionOfClauses) {
		this.conjunctionOfClauses.addAll(conjunctionOfClauses);
	}

	public int getNumberOfClauses() {
		return conjunctionOfClauses.Count;
	}

	public List<Clause> getConjunctionOfClauses() {
		return Collections.unmodifiableList(conjunctionOfClauses);
	}

	 
	public override string ToString() {
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < conjunctionOfClauses.Count; ++i) {
			if (i > 0) {
				sb.Append(",");
			}
			sb.Append(conjunctionOfClauses.get(i).ToString());
		}

		return sb.ToString();
	}
}
