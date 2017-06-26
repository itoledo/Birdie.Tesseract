 namespace aima.core.logic.fol.domain;

 
import java.util.HashSet;
 
 

/**
 * @author Ravi Mohan
 * @author Ciaran O'Reilly
 */
public class FOLDomain {
	private ISet<string> constants, functions, predicates;
	private int skolemConstantIndexical = 0;
	private int skolemFunctionIndexical = 0;
	private int answerLiteralIndexical = 0;
	private List<FOLDomainListener> listeners = new List<FOLDomainListener>();

	public FOLDomain() {
		this.constants = new HashSet<string>();
		this.functions = new HashSet<string>();
		this.predicates = new HashSet<string>();
	}

	public FOLDomain(FOLDomain toCopy) {
		this(toCopy.getConstants(), toCopy.getFunctions(), toCopy
				.getPredicates());
	}

	public FOLDomain(ISet<string> constants, ISet<string> functions,
			Set<string> predicates) {
		this.constants = new HashSet<string>(constants);
		this.functions = new HashSet<string>(functions);
		this.predicates = new HashSet<string>(predicates);
	}

	public ISet<string> getConstants() {
		return constants;
	}

	public ISet<string> getFunctions() {
		return functions;
	}

	public ISet<string> getPredicates() {
		return predicates;
	}

	public void addConstant(string constant) {
		constants.Add(constant);
	}

	public string addSkolemConstant() {

		String sc = null;
		do {
			sc = "SC" + (skolemConstantIndexical++);
		} while (constants.Contains(sc) || functions.Contains(sc)
				|| predicates.Contains(sc));

		addConstant(sc);
		notifyFOLDomainListeners(new FOLDomainSkolemConstantAddedEvent(this, sc));

		return sc;
	}

	public void addFunction(string function) {
		functions.Add(function);
	}

	public string addSkolemFunction() {
		String sf = null;
		do {
			sf = "SF" + (skolemFunctionIndexical++);
		} while (constants.Contains(sf) || functions.Contains(sf)
				|| predicates.Contains(sf));

		addFunction(sf);
		notifyFOLDomainListeners(new FOLDomainSkolemFunctionAddedEvent(this, sf));

		return sf;
	}

	public void addPredicate(string predicate) {
		predicates.Add(predicate);
	}

	public string addAnswerLiteral() {
		String al = null;
		do {
			al = "Answer" + (answerLiteralIndexical++);
		} while (constants.Contains(al) || functions.Contains(al)
				|| predicates.Contains(al));

		addPredicate(al);
		notifyFOLDomainListeners(new FOLDomainAnswerLiteralAddedEvent(this, al));

		return al;
	}

	public void addFOLDomainListener(FOLDomainListener listener) {
		synchronized (listeners) {
			if (!listeners.Contains(listener)) {
				listeners.Add(listener);
			}
		}
	}

	public void removeFOLDomainListener(FOLDomainListener listener) {
		synchronized (listeners) {
			listeners.Remove(listener);
		}
	}

	//
	// PRIVATE METHODS
	//
	private void notifyFOLDomainListeners(FOLDomainEvent event) {
		synchronized (listeners) {
			for (FOLDomainListener l : listeners) {
				event.notifyListener(l);
			}
		}
	}
}