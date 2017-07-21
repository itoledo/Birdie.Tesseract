namespace aima.core.environment.wumpusworld;

/**
 * Representation of an Agent's [x,y] position and orientation [up, down, right,
 * or left] within a Wumpus World cave.
 * 
 * @author Federico Baron
 * @author Alessandro Daniele
 * @author Ciaran O'Reilly
 * @author Anurag Rai
 */
public class AgentPosition {

	public enum Orientation {
		FACING_NORTH("FacingNorth"), FACING_SOUTH("FacingSouth"), FACING_EAST("FacingEast"), FACING_WEST("FacingWest");

		 
		public override string ToString() {
			return name;
		}

		private readonly String name;

		private Orientation(String name) {
			this.name = name;
		}
	}

	private Room room;
	private Orientation orientation;

	public AgentPosition(int x, int y, Orientation orientation) {
		this(new Room(x, y), orientation);
	}

	public AgentPosition(Room room, Orientation orientation) {
		this.room = room;
		this.orientation = orientation;
	}

	public Room getRoom() {
		return room;
	}

	public int getX() {
		return room.getX();
	}

	public int getY() {
		return room.getY();
	}

	public Orientation getOrientation() {
		return orientation;
	}

	 
	public override string ToString() {
		return room.ToString() + "->" + orientation;
	}

	 
	public bool equals(Object obj) {
		if (obj != null && obj is AgentPosition) {
			AgentPosition othAgent = (AgentPosition) obj;
			if ((getX() == othAgent.getX()) && (getY() == othAgent.getY())
					&& (orientation == othAgent.getOrientation())) {
				return true;
			} else {
				return false;
			}
		}
		return false;
	}

	 
	public override int GetHashCode() {
		int result = 17;
		result = 37 * result + room.GetHashCode();
		result = 43 * result + orientation.GetHashCode();
		return result;
	}
}
