 namespace aima.core.probability.domain;

public abstract class AbstractContinuousDomain : ContinuousDomain {

	//
	// START-Domain
	 
	public bool isFinite() {
		return false;
	}

	 
	public bool isInfinite() {
		return true;
	}

	 
	public int size() {
		throw new IllegalStateException(
				"You cannot determine the size of an infinite domain");
	}

	 
	public abstract bool isOrdered();
	// END-Domain
	//
}
