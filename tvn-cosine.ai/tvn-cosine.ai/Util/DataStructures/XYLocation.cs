using System;

namespace tvn.cosine.ai.Util.DataStructures
{
    /// <summary>
    /// Note: If looking at a rectangle - the coordinate(x= 0, y= 0) will be the top
    /// left hand corner.This corresponds with Java's AWT coordinate system.
    /// </summary>
    public class XYLocation : IEquatable<XYLocation>
    {
        public enum Direction
        {
            North, South, East, West
        }

        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Constructs and initializes a location at the specified (x,y) location in the coordinate space.
        /// </summary>
        /// <param name="x">the x coordinate</param>
        /// <param name="y">the y coordinate</param>
        public XYLocation(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns the location one unit left of this location.
        /// </summary>
        /// <returns>the location one unit left of this location.</returns>
        public XYLocation West()
        {
            return new XYLocation(X - 1, Y);
        }


        /// <summary>
        /// Returns the location one unit right of this location.
        /// </summary>
        /// <returns>the location one unit right of this location.</returns>
        public XYLocation East()
        {
            return new XYLocation(X + 1, Y);
        }

        /// <summary>
        /// Returns the location one unit ahead of this location.
        /// </summary>
        /// <returns>the location one unit ahead of this location.</returns>
        public XYLocation North()
        {
            return new XYLocation(X, Y - 1);
        }


        /// <summary>
        /// Returns the location one unit behind, this location.
        /// </summary>
        /// <returns>the location one unit behind, this location.</returns>
        public XYLocation South()
        {
            return new XYLocation(X, Y + 1);
        }


        /// <summary>
        /// Returns the location one unit left of this location.
        /// </summary>
        /// <returns>the location one unit left of this location.</returns>
        public XYLocation Left()
        {
            return West();
        }


        /// <summary>
        /// Returns the location one unit right of this location.
        /// </summary>
        /// <returns> the location one unit right of this location.</returns>
        public XYLocation Right()
        {
            return East();
        }


        /// <summary>
        /// Returns the location one unit above this location.
        /// </summary>
        /// <returns>the location one unit above this location.</returns>
        public XYLocation Up()
        {
            return North();
        }

        /// <summary>
        /// Returns the location one unit below this location.
        /// </summary>
        /// <returns>the location one unit below this location.</returns>
        public XYLocation Down()
        {
            return South();
        }

        /// <summary>
        /// Returns the location one unit from this location in the specified direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>the location one unit from this location in the specified direction.</returns>
        public XYLocation LocationAt(Direction direction)
        {
            if (direction.Equals(Direction.North))
            {
                return North();
            }
            if (direction.Equals(Direction.South))
            {
                return South();
            }
            if (direction.Equals(Direction.East))
            {
                return East();
            }
            if (direction.Equals(Direction.West))
            {
                return West();
            }
            else
            {
                throw new ArgumentException("Unknown direction " + direction);
            }
        }

        public override int GetHashCode()
        {
            int result = 17;
            result = 37 * result + X;
            result = 43 * result + Y;
            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as XYLocation);
        }

        public bool Equals(XYLocation other)
        {
            return null != other
                && X.Equals(other.X)
                && Y.Equals(other.Y);
        } 
    }
}
