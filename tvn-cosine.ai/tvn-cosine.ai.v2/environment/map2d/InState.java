namespace aima.core.environment.map2d;

public class InState {
	private String location;

	public InState(String location) {
		this.location = location;
	}

	public String getLocation() {
		return location;
	}

	 
	public bool equals(Object obj) {
		if (obj != null && obj is InState) {
			return this.getLocation().Equals(((InState) obj).getLocation());
		}
		return super.Equals(obj);
	}

	 
	public override int GetHashCode() {
		return getLocation().GetHashCode();
	}

	 
	public override string ToString() {
		return "In("+getLocation()+")";
	}
}
