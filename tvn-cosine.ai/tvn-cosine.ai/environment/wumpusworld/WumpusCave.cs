using System.Text;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.environment.wumpusworld
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 236.<br>
     * <br>
     * The <b>wumpus world</b> is a cave consisting of rooms connected by
     * passageways. The rooms are always organized into a grid. See Figure 7.2 for
     * an example.
     *
     * @author Ruediger Lunde
     * @author Federico Baron
     * @author Alessandro Daniele
     * @author Ciaran O'Reilly
     */
    public class WumpusCave
    {
        private int caveXDimension; // starts bottom left -> right
        private int caveYDimension; // starts bottom left ^ up

        private AgentPosition start = new AgentPosition(1, 1, AgentPosition.Orientation.FACING_NORTH);
        private Room wumpus;
        private Room gold;
        private ISet<Room> pits = Factory.CreateSet<Room>();

        private ISet<Room> allowedRooms;

        /**
         * Default Constructor. Create a Wumpus Case of default dimensions 4x4.
         */
        public WumpusCave()
            : this(4, 4)
        { }

        /**
         * Create a grid of rooms of dimensions x and y, representing the wumpus's cave.
         * 
         * @param caveXDimension
         *            the cave's x dimension.
         * @param caveYDimension
         *            the cave's y dimension.
         */
        public WumpusCave(int caveXDimension, int caveYDimension)
        {
            if (caveXDimension < 1)
                throw new IllegalArgumentException("Cave must have x dimension >= 1");
            if (caveYDimension < 1)
                throw new IllegalArgumentException("Case must have y dimension >= 1");
            this.caveXDimension = caveXDimension;
            this.caveYDimension = caveYDimension;
            allowedRooms = getAllRooms();
        }

        /**
         * Create a grid of rooms of dimensions x and y, representing the wumpus's cave.
         *
         * @param caveXDimension
         *            the cave's x dimension.
         * @param caveYDimension
         *            the cave's y dimension.
         * @param config
         *            cave specification - two character per square (unfortunately a Wumpus can reside on top of a pit),
         *            first line first, then second line etc. Mapping: S=start, W=Wumpus, G=gold, P=pit.
         */
        public WumpusCave(int caveXDimension, int caveYDimension, string config)
            : this(caveXDimension, caveYDimension)
        {

            if (config.Length != 2 * caveXDimension * caveYDimension)
                throw new IllegalStateException("Wrong configuration length.");
            for (int i = 0; i < config.Length;++i)
            {
                char c = config[i];
                Room r = new Room(i / 2 % caveXDimension + 1, caveYDimension - i / 2 / caveXDimension);
                switch (c)
                {
                    case 'S': start = new AgentPosition(r.getX(), r.getY(), AgentPosition.Orientation.FACING_NORTH); break;
                    case 'W': wumpus = r; break;
                    case 'G': gold = r; break;
                    case 'P': pits.Add(r); break;
                }
            }
        }

        /**
         * Limits possible movement within the cave (for search).
         * @param allowedRooms
         *            the set of legal rooms that can be reached within the cave.
         */
        public WumpusCave setAllowed(ISet<Room> allowedRooms)
        {
            this.allowedRooms.Clear();
            this.allowedRooms.AddAll(allowedRooms);
            return this;
        }

        public void setWumpus(Room room)
        {
            wumpus = room;
        }

        public void setGold(Room room)
        {
            gold = room;
        }

        public void setPit(Room room, bool b)
        {
            if (!b)
                pits.Remove(room);
            else if (!room.Equals(start.getRoom()) && !room.Equals(gold))
                pits.Add(room);
        }

        public int getCaveXDimension()
        {
            return caveXDimension;
        }

        public int getCaveYDimension()
        {
            return caveYDimension;
        }

        public AgentPosition getStart()
        {
            return start;
        }

        public Room getWumpus()
        {
            return wumpus;
        }

        public Room getGold()
        {
            return gold;
        }

        public bool isPit(Room room)
        {
            return pits.Contains(room);
        }

        public AgentPosition moveForward(AgentPosition position)
        {
            int x = position.getX();
            int y = position.getY();
            if (position.getOrientation().Equals(AgentPosition.Orientation.FACING_NORTH))
            {
                y++; 
            }
            else if (position.getOrientation().Equals(AgentPosition.Orientation.FACING_SOUTH))
            {
                y--; 
            }
            else if (position.getOrientation().Equals(AgentPosition.Orientation.FACING_EAST))
            {
                x++; 
            }
            else if (position.getOrientation().Equals(AgentPosition.Orientation.FACING_WEST))
            {
                x--;
            }
            Room room = new Room(x, y);
            return allowedRooms.Contains(room) ? new AgentPosition(x, y, position.getOrientation()) : position;
        }

        public AgentPosition turnLeft(AgentPosition position)
        {
            AgentPosition.Orientation orientation = null;

            if (position.getOrientation().Equals(AgentPosition.Orientation.FACING_NORTH))
            {
                orientation = AgentPosition.Orientation.FACING_WEST;
            }
            else if (position.getOrientation().Equals(AgentPosition.Orientation.FACING_SOUTH))
            {
                orientation = AgentPosition.Orientation.FACING_EAST;
            }
            else if (position.getOrientation().Equals(AgentPosition.Orientation.FACING_EAST))
            {
                orientation = AgentPosition.Orientation.FACING_NORTH;
            }
            else if (position.getOrientation().Equals(AgentPosition.Orientation.FACING_WEST))
            {
                orientation = AgentPosition.Orientation.FACING_SOUTH;
            }
            return new AgentPosition(position.getX(), position.getY(), orientation);
        }

        public AgentPosition turnRight(AgentPosition position)
        {
            AgentPosition.Orientation orientation = null;
            if (position.getOrientation().Equals(AgentPosition.Orientation.FACING_NORTH))
            {
                orientation = AgentPosition.Orientation.FACING_EAST;
            }
            else if (position.getOrientation().Equals(AgentPosition.Orientation.FACING_SOUTH))
            {
                orientation = AgentPosition.Orientation.FACING_WEST;
            }
            else if (position.getOrientation().Equals(AgentPosition.Orientation.FACING_EAST))
            {
                orientation = AgentPosition.Orientation.FACING_SOUTH;
            }
            else if (position.getOrientation().Equals(AgentPosition.Orientation.FACING_WEST))
            {
                orientation = AgentPosition.Orientation.FACING_NORTH;
            }

            return new AgentPosition(position.getX(), position.getY(), orientation);
        }

        public ISet<Room> getAllRooms()
        {
            ISet<Room> allowedRooms = Factory.CreateSet<Room>();
            for (int x = 1; x <= caveXDimension; x++)
                for (int y = 1; y <= caveYDimension; y++)
                    allowedRooms.Add(new Room(x, y));
            return allowedRooms;
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int y = caveYDimension; y >= 1; y--)
            {
                for (int x = 1; x <= caveXDimension; x++)
                {
                    Room r = new Room(x, y);
                    string txt = "";
                    if (r.Equals(start.getRoom()))
                        txt += "S";
                    if (r.Equals(gold))
                        txt += "G";
                    if (r.Equals(wumpus))
                        txt += "W";
                    if (isPit(r))
                        txt += "P";

                    if (string.IsNullOrEmpty(txt))
                        txt = ". ";
                    else if (txt.Length == 1)
                        txt += " ";
                    else if (txt.Length > 2) // cannot represent...
                        txt = txt.Substring(0, 2);
                    builder.Append(txt);
                }
                builder.Append("\n");
            }
            return builder.ToString();
        }
    }
}
