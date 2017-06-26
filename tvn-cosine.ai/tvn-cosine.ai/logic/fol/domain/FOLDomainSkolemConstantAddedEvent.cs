 namespace aima.core.logic.fol.domain;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class FOLDomainSkolemConstantAddedEvent : FOLDomainEvent {

	private static readonly long serialVersionUID = 1L;

	private string skolemConstantName;

	public FOLDomainSkolemConstantAddedEvent(object source,
			String skolemConstantName) {
		base(source);

		this.skolemConstantName = skolemConstantName;
	}

	public string getSkolemConstantName() {
		return skolemConstantName;
	}

	 
	public void notifyListener(FOLDomainListener listener) {
		listener.skolemConstantAdded(this);
	}
}
