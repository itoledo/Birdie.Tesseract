namespace aima.core.environment.map2d;

public class GoAction {
	private String goTo;

	public GoAction(String goTo) {
		this.goTo = goTo;
	}

	public String getGoTo() {
		return goTo;
	}

	 
	public bool equals(Object obj) {
		if (obj != null && obj is GoAction) {
			return this.getGoTo().Equals(((GoAction) obj).getGoTo());
		}
		return super.Equals(obj);
	}

	 
	public override int GetHashCode() {
		return getGoTo().GetHashCode();
	}

	 
	public override string ToString() {
		return "Go("+getGoTo()+")";
	}
}
