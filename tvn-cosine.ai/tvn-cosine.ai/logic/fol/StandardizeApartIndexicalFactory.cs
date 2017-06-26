 namespace aima.core.logic.fol;

 
 

/**
 * This class ensures unique standardize apart indexicals are created.
 * 
 * @author Ciaran O'Reilly
 * 
 */
public class StandardizeApartIndexicalFactory {
	private static IDictionary<Character, int> _assignedIndexicals = new Dictionary<Character, int>();

	// For use in test cases, where predictable behavior is expected.
	public static void flush() {
		synchronized (_assignedIndexicals) {
			_assignedIndexicals.Clear();
		}
	}

	public static StandardizeApartIndexical newStandardizeApartIndexical(
			Character preferredPrefix) {
		char ch = preferredPrefix.charValue();
		if (!(Character.isLetter(ch) && Character.isLowerCase(ch))) {
			throw new ArgumentException("Preferred prefix :"
					+ preferredPrefix + " must be a valid a lower case letter.");
		}

		StringBuilder sb = new StringBuilder();
		synchronized (_assignedIndexicals) {
			Integer currentPrefixCnt = _assignedIndexicals.get(preferredPrefix);
			if (null == currentPrefixCnt) {
				currentPrefixCnt = 0;
			} else {
				currentPrefixCnt += 1;
			}
			_assignedIndexicals.Add(preferredPrefix, currentPrefixCnt);
			sb.Append(preferredPrefix);
			for (int i = 0; i < currentPrefixCnt; ++i) {
				sb.Append(preferredPrefix);
			}
		}

		return new StandardizeApartIndexicalImpl(sb.ToString());
	}
}

class StandardizeApartIndexicalImpl : StandardizeApartIndexical {
	private string prefix = null;
	private int index = 0;

	public StandardizeApartIndexicalImpl(string prefix) {
		this.prefix = prefix;
	}

	//
	// START-StandardizeApartIndexical
	public string getPrefix() {
		return prefix;
	}

	public int getNextIndex() {
		return index++;
	}
	// END-StandardizeApartIndexical
	//
}