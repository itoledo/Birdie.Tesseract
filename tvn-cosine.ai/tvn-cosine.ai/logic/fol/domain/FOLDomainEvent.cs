 namespace aima.core.logic.fol.domain;

import java.util.EventObject;

/**
 * @author Ciaran O'Reilly
 * 
 */
public abstract class FOLDomainEvent : EventObject {

	private static readonly long serialVersionUID = 1L;

	public FOLDomainEvent(object source) {
		base(source);
	}

	public abstract void notifyListener(FOLDomainListener listener);
}
