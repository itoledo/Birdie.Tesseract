 namespace aima.core.logic.fol.domain;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class FOLDomainSkolemFunctionAddedEvent : FOLDomainEvent {

	private static readonly long serialVersionUID = 1L;

	private string skolemFunctionName;

	public FOLDomainSkolemFunctionAddedEvent(object source,
			String skolemFunctionName) {
		base(source);

		this.skolemFunctionName = skolemFunctionName;
	}

	public string getSkolemConstantName() {
		return skolemFunctionName;
	}

	 
	public void notifyListener(FOLDomainListener listener) {
		listener.skolemFunctionAdded(this);
	}
}
