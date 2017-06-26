 namespace aima.core.environment.map;

import aima.core.agent.impl.DynamicAction;

public class MoveToAction : DynamicAction {
	public static readonly string ATTRIBUTE_MOVE_TO_LOCATION = "location";

	public MoveToAction(string location) {
		base("moveTo");
		setAttribute(ATTRIBUTE_MOVE_TO_LOCATION, location);
	}

	public string getToLocation() {
		return (string) getAttribute(ATTRIBUTE_MOVE_TO_LOCATION);
	}
}
