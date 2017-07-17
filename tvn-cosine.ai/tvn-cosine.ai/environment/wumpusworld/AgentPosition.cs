using tvn.cosine.ai.common;
using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.environment.wumpusworld
{
    /**
     * Representation of an Agent's [x,y] position and orientation [up, down, right, or left] within a Wumpus World cave.
     * 
     * @author Federico Baron
     * @author Alessandro Daniele
     * @author Ciaran O'Reilly
     */
    public class AgentPosition : IToString, IEquatable
    {
        public class Orientation
        {
            private static readonly ISet<Orientation> _values = Factory.CreateSet<Orientation>();
             
            public static readonly Orientation FACING_NORTH = new Orientation("FacingNorth");
            public static readonly Orientation FACING_SOUTH = new Orientation("FacingSouth");
            public static readonly Orientation FACING_EAST = new Orientation("FacingEast");
            public static readonly Orientation FACING_WEST = new Orientation("FacingWest");

            public string getSymbol()
            {
                return symbol;
            }

            private readonly string symbol;

            public static IQueue<Orientation> values()
            {
                return Factory.CreateReadOnlySet<Orientation>(_values);
            }

            Orientation(string sym)
            {
                symbol = sym;
                _values.Add(this);
            }
        }

        private Room room;
        private Orientation orientation;

        public AgentPosition(int x, int y, Orientation orientation)
            : this(new Room(x, y), orientation)
        { }

        public AgentPosition(Room room, Orientation orientation)
        {
            this.room = room;
            this.orientation = orientation;
        }

        public Room getRoom()
        {
            return room;
        }

        public int getX()
        {
            return room.getX();
        }

        public int getY()
        {
            return room.getY();
        }

        public Orientation getOrientation()
        {
            return orientation;
        }

        public override string ToString()
        {
            return room.toString() + "->" + orientation.getSymbol();
        }

        public override bool Equals(object obj)
        {
            if (obj != null && GetType() == obj.GetType())
            {
                AgentPosition other = (AgentPosition)obj;
                return (getX() == other.getX()) && (getY() == other.getY())
                        && (orientation == other.getOrientation());
            }
            return false;
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 37 * result + room.GetHashCode();
            result = 43 * result + orientation.GetHashCode();
            return result;
        }
    }
}
