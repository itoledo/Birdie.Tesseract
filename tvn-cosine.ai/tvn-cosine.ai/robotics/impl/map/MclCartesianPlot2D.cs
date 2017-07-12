using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using tvn.cosine.ai.robotics.datatypes;
using tvn.cosine.ai.robotics.impl.datatypes;
using tvn.cosine.ai.util.math.geom;
using tvn.cosine.ai.util.math.geom.shapes;

namespace tvn.cosine.ai.robotics.impl.map
{
    /**
     * This class implements the interface {@link IMclMap} using the classes {@link Angle} and {@link AbstractRangeReading}.<br/>
     * It uses a parser that generates two sets of {@link IGeometric2D}.<br/>
     * The first set describes obstacles that can be measured by the range sensor. Thus only this group is considered for the {@code rayCast} function.<br/>
     * The second group specifies areas on the map. If a position is in one of these areas it is a valid position.<br/>
     * This functionality is implemented by {@code isPoseValid} which in addition tests whether the heading of that pose is valid and the position is inside an obstacle which makes it an invalid position.
     * 
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     * 
     * @param <P> a pose implementing {@link IPose2D}.
     * @param <M> a movement (or sequence of movements) of the robot, implementing {@link IMclMove}.
     * @param <R> a range reading extending {@link AbstractRangeReading}.
     */
    public class MclCartesianPlot2D<P, M, R> : IMclMap<P, Angle, M, AbstractRangeReading>
        where P : IPose2D<P, M>
        where M : IMclMove<M>
        where R : AbstractRangeReading
    {
        /**
         * This is the identifier that is used to find a group of obstacles in the map file.
         */
        public const string OBSTACLE_ID = "obstacles";
        /**
         * This is the identifier that is used to find a group of areas in the map file.
         */
        public const string AREA_ID = "validMovementArea";

        private IPoseFactory<P, M> poseFactory;
        private IRangeReadingFactory<R> rangeReadingFactory;

        private CartesianPlot2D obstacles;
        private CartesianPlot2D areas;

        private Exception obstaclesException;
        private Exception areasException;

        /**
         * @param obstaclesParser a map parser implementing {@link IGroupParser}. This parser is used to load a map file for the obstacles.
         * @param areasParser a map parser implementing {@link IGroupParser}. This parser is used to load a map file for the areas. It should be a different object than obstaclesParser or implemented thread-safe.
         * @param poseFactory a pose factory implementing {@link IPoseFactory}.
         * @param rangeReadingFactory a range reading factory implementing {@link IRangeReadingFactory}.
         */
        public MclCartesianPlot2D(IGroupParser obstaclesParser, IGroupParser areasParser, IPoseFactory<P, M> poseFactory, IRangeReadingFactory<R> rangeReadingFactory)
        {
            this.poseFactory = poseFactory;
            this.rangeReadingFactory = rangeReadingFactory;
            obstacles = new CartesianPlot2D(obstaclesParser);
            areas = new CartesianPlot2D(areasParser);
        }

        /**
         * Sets the sensor range.
         * @param sensorRange the maximum range that the sensor can reliably measure. This parameter is used to speed up {@code rayCast}.
         */
        public void setSensorRange(double sensorRange)
        {
            obstacles.setRayRange(sensorRange);
            areas.setRayRange(sensorRange);
        }

        /**
         * Calculate the maximum distance between all samples and compare it to {@code maxDistance}.
         * If it is smaller or equals to {@code maxDistance} the mean is returned. {@code null} otherwise.
         * @param samples the set of samples to be checked against.
         * @param maxDistance the maxDistance that the cloud should have to return a mean.
         * @return the mean of the samples or {@code null}.
         */
        public P checkDistanceOfPoses(ISet<P> samples, double maxDistance)
        {
            double maxDistanceSamples = 0.0d;
            foreach (P first in samples)
            {
                foreach (P second in samples)
                {
                    double distance = first.distanceTo(second);
                    maxDistanceSamples = distance > maxDistanceSamples ? distance : maxDistanceSamples;
                }
            }
            if (maxDistanceSamples <= maxDistance)
            {
                double averageX = 0.0d;
                double averageY = 0.0d;
                double averageHeading = 0.0d;
                foreach (P sample in samples)
                {
                    averageX += sample.getX() / samples.Count;
                    averageY += sample.getY() / samples.Count;
                    averageHeading += sample.getHeading() / samples.Count;
                }
                return poseFactory.getPose(new Point2D(averageX, averageY), averageHeading);
            }
            return default(P);
        }

        /**
         * This function loads a map input stream into this Cartesian plot. The two streams have to be two different instances to be thread safe.
         * @param obstacleInput the stream containing the obstacles.
         * @param areaInput the stream containing the areas
         * @throws Exception thrown by the implementing class of {@link IGroupParser} when calling {@code loadMap}.
         */
        public void loadMap(StreamReader obstacleInput, StreamReader areaInput)
        {
            obstaclesException = null;
            areasException = null;

            Task.Run(() => obstacles.loadMap(obstacleInput, OBSTACLE_ID));
            Task.Run(() => areas.loadMap(areaInput, AREA_ID));

            if (obstaclesException != null) throw obstaclesException;
            if (areasException != null) throw areasException;
        }

        /**
         * Returns an iterator over the obstacle polygons.
         * @return an iterator over the obstacle polygons.
         */
        public IEnumerator<IGeometric2D> getObstacles()
        {
            return obstacles.getShapes();
        }

        /**
         * Returns an iterator over the boundaries of the obstacle polygons.
         * @return an iterator over the boundaries of the obstacle polygons.
         */
        public IEnumerator<Rect2D> getObstacleBoundaries()
        {
            return obstacles.getBoundaries();
        }

        /**
         * Returns an iterator over the area polygons.
         * @return an iterator over the area polygons.
         */
        public IEnumerator<IGeometric2D> getAreas()
        {
            return areas.getShapes();
        }

        /**
         * Returns an iterator over the boundaries of the area polygons.
         * @return an iterator over the boundaries of the area polygons.
         */
        public IEnumerator<Rect2D> getAreaBoundaries()
        {
            return areas.getBoundaries();
        }

        /**
         * Checks whether a map was loaded.
         * @return {@code true} if a map was loaded.
         */
        public bool isLoaded()
        {
            return !areas.isEmpty();
        }

        public P randomPose()
        {
            Point2D point;
            do
            {
                point = areas.randomPoint();
            } while (obstacles.isPointInsideShape(point));
            return poseFactory.getPose(point);
        }

        public AbstractRangeReading rayCast(P pose)
        {
            Ray2D ray = new Ray2D(new Point2D(pose.getX(), pose.getY()), Vector2D.calculateFromPolar(1, -pose.getHeading()));
            return rangeReadingFactory.getRangeReading(obstacles.rayCast(ray));
        }

        public bool isPoseValid(P pose)
        {
            if (!poseFactory.isHeadingValid(pose)) return false;
            Point2D point = new Point2D(pose.getX(), pose.getY());
            return areas.isPointInsideBorderShape(point) && !obstacles.isPointInsideShape(point);
        }
    }

}
