using tvn.cosine.ai.robotics.datatypes;
using tvn.cosine.ai.robotics.impl.datatypes;
using tvn.cosine.ai.util.math.geom.shapes;

namespace tvn.cosine.ai.robotics.impl.map
{
    /**
     * This interface defines a factory for the class implementing {@link IPose2D} in the context of the {@link MclCartesianPlot2D}.
     *  
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     * @param <P> a pose implementing {@link IPose2D}.
     * @param <M> a movement (or a sequence of movements) implementing {@link IMclMove}.
     */
    public interface IPoseFactory<P, M>
        where P : IPose2D<P, M>
        where M : IMclMove<M>
    {
        /**
         * Creates a new instance of {@code <P>} for the given parameters.
         * @param point the 2D coordinates of the new pose.
         * @return the new pose.
         */
        P getPose(Point2D point);

        /**
         * Creates a new instance of {@code <P>} for the given parameters.<br/>
         * This function is used to create the result of {@link MclCartesianPlot2D}{@code .getAverage()}.
         * @param point the 2D coordinates of the new pose.
         * @param heading the heading of the pose. This heading may be invalid. Based on the given environment this can be corrected or ignored.
         * @return the new pose.
         */
        P getPose(Point2D point, double heading);

        /**
         * Checks whether the heading of a pose is valid.
         * @param pose the pose to be checked.
         * @return true if the heading is valid.
         */
        bool isHeadingValid(P pose);
    }

}
