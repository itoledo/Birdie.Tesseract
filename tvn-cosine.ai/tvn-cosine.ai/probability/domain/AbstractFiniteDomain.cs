 namespace aima.core.probability.domain;

 
 
 

public abstract class AbstractFiniteDomain : FiniteDomain {

	private string toString = null;
	private IDictionary<Object, int> valueToIdx = new Dictionary<Object, int>();
	private IDictionary<Integer, Object> idxToValue = new Dictionary<Integer, Object>();

	public AbstractFiniteDomain() {

	}

	//
	// START-Domain
	 
	public bool isFinite() {
		return true;
	}

	 
	public bool isInfinite() {
		return false;
	}

	 
	public abstract int size();

	 
	public abstract bool isOrdered();

	// END-Domain
	//

	//
	// START-FiniteDomain
	 
	public abstract ISet<? : Object> getPossibleValues();

	 
	public int getOffset(object value) {
		Integer idx = valueToIdx.get(value);
		if (null == idx) {
			throw new ArgumentException("Value [" + value
					+ "] is not a possible value of this domain.");
		}
		return idx.intValue();
	}

	 
	public object getValueAt(int offset) {
		return idxToValue.get(offset);
	}

	// END-FiniteDomain
	//

	 
	public override string ToString() {
		if (null == toString) {
			toString = getPossibleValues().ToString();
		}
		return toString;
	}

	//
	// PROTECTED METHODS
	//
	protected void indexPossibleValues(ISet<? : Object> possibleValues) {
		int idx = 0;
		for (object value : possibleValues) {
			valueToIdx.Add(value, idx);
			idxToValue.Add(idx, value);
			idx++;
		}
	}
}
