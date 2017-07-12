using System;
using tvn.cosine.ai.robotics.datatypes;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.robotics.impl.datatypes
{
    /**
     * This class stores an angle as a {@code double}. It is used to pass on the angle from the {@link IMclRangeReading} to the {@link IPose2D}.<br/>
     * This implementation can be used for angles in radians. The context of this unit is that {@link IPose2D} has to use radian angles.<br/>
     * Please note, that all classes that are interacting with the MCL somehow should use radian angles, too.<br/>
     * Use {@code degreeAngle()}, {@code getDegreeValue()}, {@code Math.toDegrees()} and {@code Math.toRadians()} if you need or have degree angles.
     *  
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     */
    public class Angle : IMclVector, IComparable<Angle>
    {
        /**
         * The zero angle represents {@code 0.0} radians.
         */
        public static readonly Angle ZERO_ANGLE = new Angle(0.0d);

        private readonly double value;

        /**
         * @param value the radian value of the angle.
         */
        public Angle(double value)
        {
            this.value = value;
        }

        /**
         * Creates a new angle based on a degree value.
         * @param value the degree value of the angle.
         * @return the new angle.
         */
        public static Angle degreeAngle(double value)
        {
            return new Angle(Util.ToRadians(value));
        }

        /**
         * @return the radian value of the angle.
         */
        public double getValue()
        {
            return value;
        }

        /**
         * @return the degree value of the angle.
         */
        public double getDegreeValue()
        {
            return Util.ToDegrees(value);
        }

        public int CompareTo(Angle o)
        {
            if (Util.compareDoubles(this.value, o.value)) return 0;
            if (this.value < o.value) return -1;
            return 1;
        }
    } 
}
