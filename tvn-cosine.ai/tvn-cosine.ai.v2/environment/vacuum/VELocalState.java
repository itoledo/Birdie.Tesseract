namespace aima.core.environment.vacuum;

/**
 * @author Ciaran O'Reilly
 */
public class VELocalState {
	public final String location;
	public final VacuumEnvironment.Status status;

	public VELocalState(String location, VacuumEnvironment.Status state) {
		this.location = location;
		this.status = state;
	}

	 
	public bool equals(Object obj) {
		boolean result = false;
		if (obj != null && this.getClass() == obj.getClass()) {
			VELocalState other = (VELocalState) obj;
			result = this.status == other.status && this.location.Equals(other.location);
		}
		return result;
	}

	 
	public override int GetHashCode() {
		return location.GetHashCode() + status.GetHashCode();
	}
	
	 
	public override string ToString() {
		return "VE("+location+", "+status+")";
	}
}
