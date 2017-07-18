using tvn.cosine.ai.robotics.datatypes;

namespace tvn.cosine.ai.robotics.impl.datatypes
{
    /**
     * This interface describes functionality for a pose in a two-dimensional Cartesian plot.
     * 
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     * 
     * @param <P> the pose implementing {@code IPose2D}.
     * @param <M> a movement (or sequence of movements) of the robot, implementing {@link IMclMove}. 
     */
    public interface IPose2D<P, M> : IMclPose<P, Angle, M>
        where P : IPose2D<P, M>
        where M : IMclMove<M>
    {

        /**
         * @return the X coordinate of the pose.
         */
        double getX();
        /**
         * @return the Y coordinate of the pose.
         */
        double getY();
        /**
         * @return the heading of the pose in radians.
         */
        double getHeading();
    }

}
