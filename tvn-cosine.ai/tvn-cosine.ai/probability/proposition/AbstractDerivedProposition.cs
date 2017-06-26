 namespace aima.core.probability.proposition;

public abstract class AbstractDerivedProposition : AbstractProposition
		implements DerivedProposition {

	private string name = null;

	public AbstractDerivedProposition(string name) {
		this.name = name;
	}

	//
	// START-DerivedProposition
	public string getDerivedName() {
		return name;
	}

	// END-DerivedProposition
	//
}