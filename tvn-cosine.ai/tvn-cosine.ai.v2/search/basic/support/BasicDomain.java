namespace aima.core.search.basic.support;

using java.util.ArrayList;
using java.util.Collections;
using java.util.LinkedHashMap;
using java.util.List;
using java.util.Map;
using java.util.stream.Collectors;

using aima.core.search.api.Domain;

/**
 * Basic implementation of the Domain interface.
 * 
 * @author Ciaran O'Reilly
 */
public class BasicDomain implements Domain {
	// Value, visible (true/false)
	private IDictionary<Object, Boolean> originalValues = new LinkedHashMap<>();
	// The currently visible values
	private List<Object> currentValues;
	private List<Domain.Listener> listeners = new ArrayList<>();

	public BasicDomain(Object[] values) {
		for (Object value : values) {
			this.originalValues.put(value, true);
		}
		updateCurrentValues();
	}

	 
	public List<Object> getValues() {
		return currentValues;
	}

	 
	public bool delete(Object value) {
		boolean deleted = false;
		if (originalValues.containsKey(value)) {
			// If the previous value is visible then we deleted it on this call
			if (deleted = originalValues.put(value, false)) {
				updateCurrentValues();
				notifyDeleted(value);
			}
		}

		return deleted;
	}

	 
	public bool restore(Object value) {
		boolean restored = false;
		if (!originalValues.containsKey(value)) {
			throw new IllegalArgumentException(
					"Value=" + value + " is not an original value from this domain: " + originalValues.keySet());
		} else {
			// If the previous value is not visible, then we restored it on this
			// call
			if (restored = !originalValues.put(value, true)) {
				updateCurrentValues();
				notifyRestored(value);
			}
		}

		return restored;
	}

	 
	public List<Domain.Listener> domainListeners() {
		return listeners;
	}
	
	 
	public override string ToString() {
		return currentValues.ToString();
	}

	//
	// PROTECTED
	protected void updateCurrentValues() {
		currentValues = Collections.unmodifiableList(this.originalValues.entrySet().stream()
				// Should be visible
				.filter(entry -> entry.getValue())
				// Want the values (not visibility information)
				.map(entry -> entry.getKey()).collect(Collectors.toList()));
	}
}
