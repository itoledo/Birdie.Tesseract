using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.environment.map
{
    /**
     * Provides a general interface for maps.
     * 
     * @author Ruediger Lunde
     */
    public interface Map
    {
        /** Returns a list of all locations. */
        IQueue<string> getLocations();

        /**
         * Answers to the question: Where can I get, following one of the
         * connections starting at the specified location?
         */
        IQueue<string> getPossibleNextLocations(string location);

        /**
         * Answers to the question: From where can I reach a specified location,
         * following one of the map connections?
         */
        IQueue<string> getPossiblePrevLocations(string location);

        /**
         * Returns the travel distance between the two specified locations if they
         * are linked by a connection and null otherwise.
         */
        double getDistance(string fromLocation, string toLocation);

        /**
         * Returns the position of the specified location. The position is
         * represented by two coordinates, e.g. latitude and longitude values.
         */
        Point2D getPosition(string loc);

        /**
         * Returns a location which is selected by random.
         */
        string randomlyGenerateDestination();
    }
}
