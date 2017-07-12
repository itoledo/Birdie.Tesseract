using tvn.cosine.ai.agent.impl;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.environment.nqueens
{
    /**
     * Queens can be placed, removed, and moved. For movements, a vertical direction
     * is assumed. Therefore, only the end point needs to be specified.
     * 
     * @author Ravi Mohan
     * @author Ruediger Lunde
     */
    public class QueenAction : DynamicAction
    {
        public const string PLACE_QUEEN = "placeQueenAt";
        public const string REMOVE_QUEEN = "removeQueenAt";
        public const string MOVE_QUEEN = "moveQueenTo";

        public const string ATTRIBUTE_QUEEN_LOC = "location";

        /**
         * Creates a queen action. Supported values of type are {@link #PLACE_QUEEN}
         * , {@link #REMOVE_QUEEN}, or {@link #MOVE_QUEEN}.
         */
        public QueenAction(string type, XYLocation loc)
          : base(type)
        {
            SetAttribute(ATTRIBUTE_QUEEN_LOC, loc);
        }

        public XYLocation getLocation()
        {
            return (XYLocation)GetAttribute(ATTRIBUTE_QUEEN_LOC);
        }

        public int getX()
        {
            return getLocation().X;
        }

        public int getY()
        {
            return getLocation().Y;
        }
    }

}
