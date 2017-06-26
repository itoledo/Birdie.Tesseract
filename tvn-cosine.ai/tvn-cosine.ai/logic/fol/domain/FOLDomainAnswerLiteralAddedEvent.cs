 namespace aima.core.logic.fol.domain;

/**
 * @author Ciaran O'Reilly
 * 
 */
public class FOLDomainAnswerLiteralAddedEvent : FOLDomainEvent {

	private static readonly long serialVersionUID = 1L;

	private string answerLiteralName;

	public FOLDomainAnswerLiteralAddedEvent(object source,
			String answerLiteralName) {
		base(source);

		this.answerLiteralName = answerLiteralName;
	}

	public string getAnswerLiteralNameName() {
		return answerLiteralName;
	}

	 
	public void notifyListener(FOLDomainListener listener) {
		listener.answerLiteralNameAdded(this);
	}
}
