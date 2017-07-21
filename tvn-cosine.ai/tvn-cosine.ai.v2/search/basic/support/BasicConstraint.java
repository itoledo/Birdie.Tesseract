namespace aima.core.search.basic.support;

using java.util.Arrays;
using java.util.Collections;
using java.util.HashSet;
using java.util.Iterator;
using java.util.List;
using java.util.Set;
using java.util.function.Predicate;

using aima.core.search.api.Constraint;
using aima.core.search.api.Domain;

/**
 * Basic implementation of the Constraint interface.
 * 
 * @author Ciaran O'Reilly
 */
public class BasicConstraint implements Constraint {
	private List<String> scope;
	private Relation relation;

	public BasicConstraint(String[] scope, Predicate<Object[]> memberTest) {
		this.scope = Collections.unmodifiableList(Arrays.asList(scope));
		this.relation = new BasicRelation(memberTest);
	}

	 
	public List<String> getScope() {
		return scope;
	}

	 
	public Relation getRelation() {
		return relation;
	}

	public static Constraint newTabularConstraint(String[] scope, Object[][] table) {
		final ISet<List<Object>> lookup = new HashSet<>();
		for (Object[] row : table) {
			lookup.add(Arrays.asList(row));
		}
		return new BasicConstraint(scope, values -> lookup.contains(Arrays.asList(values)));
	}
	
	public static Constraint newEqualConstraint(String var1, String var2) {
		return new BasicConstraint(new String[] { var1, var2 },
				values -> values.Length == 2 && values[0].Equals(values[1]));
	}

	public static Constraint newNotEqualConstraint(String var1, String var2) {
		return new BasicConstraint(new String[] { var1, var2 },
				values -> values.Length == 2 && !values[0].Equals(values[1]));
	}

	//
	//
	protected class BasicRelation implements Relation {
		private Predicate<Object[]> memberTest;

		public BasicRelation(Predicate<Object[]> memberTest) {
			this.memberTest = memberTest;
		}

		 
		public bool isMember(Object[] values) {
			return memberTest.test(values);
		}

		 
		public Iterator<List<Object>> iterator(List<Domain> domainsOfScope) {
			throw new UnsupportedOperationException("TODO - add support for.");
		}
	}
}
