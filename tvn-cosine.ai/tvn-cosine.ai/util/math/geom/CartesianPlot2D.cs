namespace tvn.cosine.ai.util.math.geom
{
    /**
     * This class is a simple implementation of a Cartesian plot.<br/>
     * It uses a {@link IGroupParser} that generates a set of {@link IGeometric2D}.
     * 
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     * 
     */
    public class CartesianPlot2D
    { 
        private double rayRange;
        private IGroupParser parser;
        private ArrayList<IGeometric2D> shapes;
        private ArrayList<Rect2D> boundaries;

        /**
         * @param parser a file parser which : {@link IGroupParser}. This parser is used to load a map file.
         */
        public CartesianPlot2D(IGroupParser parser)
        {
            this.parser = parser;
        }

        /**
         * Sets the maximum length of a rayCast after which {@code double.PositiveInfinity} may be returned.
         * @param rayRange the maximum non zero positive length, that the ray can reliably have. This parameter is used to speed up {@code rayCast}.
         * Set this to {@code double.PositiveInfinity} if you like waiting.
         */
        public void setRayRange(double rayRange)
        {
            this.rayRange = rayRange;
        }

        /**
         * Sets the set of shapes of the plot.
         * @param shapes the set of shapes to be set.
         */
        public void setShapes(ArrayList<IGeometric2D> shapes)
        {
            this.shapes = shapes;
            boundaries = Factory.CreateQueue<Rect2D>(shapes.size());
            for (IGeometric2D shape: shapes)
            {
                boundaries.Add(shape.getBounds());
            }
        }

        /**
         * Loads a map input into this Cartesian plot.
         * @param input the stream containing the data.
         * @param groupID the identification for the group of elements that will be loaded.
         * @throws Exception thrown if the input does not contain the group.
         * @throws Exception thrown by the parser when it encounters an error in the input.
         */
        public void loadMap(InputStream input, string groupID) throws Exception
        {
            shapes = parser.parse(input, groupID);
            boundaries = Factory.CreateQueue<Rect2D>(shapes.size());
		for(IGeometric2D shape: shapes) {
			boundaries.Add(shape.getBounds());
		}
}

/**
 * @return an iterator over the shapes.
 */
public Iterator<IGeometric2D> getShapes()
{
    return shapes.iterator();
}

/**
 * @return an iterator over the boundaries of the shapes.
 */
public Iterator<Rect2D> getBoundaries()
{
    return boundaries.iterator();
}

/**
 * Checks whether the plot does not contain any elements.
 * @return true if the plot contains no element.
 */
public bool isEmpty()
{
    return shapes == null ? true : shapes.isEmpty();
}

/**
 * Calculates a random point that is somewhere inside one of the shapes of the plot or on a border of a shape.
 * @return a point on a shape within the plot.
 */
public Point2D randomPoint()
{
    IGeometric2D shape = Util.selectRandomlyFromList(shapes);
    return shape.randomPoint();
}

/**
 * Calculates the length of a ray until it intersects with a shape.<br/>
 * As this class uses the set maximum ray range to speed up this ray casting,<br/>
 * this function may return {@code double.PositiveInfinity} for a ray that intersects with an obstacle that is further away than the rayRange from the given starting point.
 * @param ray the ray to be used for ray casting.
 * @return the length of the ray.
 */
public double rayCast(Ray2D ray)
{
    double result = double.PositiveInfinity;
    Rect2D rayBounding = new Rect2D(ray.getStart().getX() - rayRange, ray.getStart().getY() - rayRange, ray.getStart().getX() + rayRange, ray.getStart().getY() + rayRange);
    for (int i = 0; i < shapes.size(); i++)
    {
        Rect2D bounding = boundaries.Get(i);
        if (rayBounding.isInsideBorder(bounding.getLowerLeft()) ||
            rayBounding.isInsideBorder(bounding.getUpperLeft()) ||
            rayBounding.isInsideBorder(bounding.getLowerRight()) ||
            rayBounding.isInsideBorder(bounding.getUpperRight()))
        {
            final double tmp = shapes.Get(i).rayCast(ray);
            result = tmp < result ? tmp : result;
        }
    }
    return result;
}

/**
 * Checks whether the given point is on any of the shapes of the plot.
 * @param point the point to be tested.
 * @return true if the point is on any of the shapes.
 */
public bool isPointInsideBorderShape(Point2D point)
{
    for (int i = 0; i < shapes.size(); i++)
    {
        if (boundaries.Get(i).isInsideBorder(point))
        {
            if (shapes.Get(i).isInsideBorder(point))
            {
                return true;
            }
        }
    }
    return false;
}

/**
 * Checks whether the given point is inside any of the shapes of the plot.
 * @param point the point to be tested.
 * @return true if the point is in any of the shapes (excluding their borders).
 */
public bool isPointInsideShape(Point2D point)
{
    for (int i = 0; i < shapes.size(); i++)
    {
        if (boundaries.Get(i).isInside(point))
        {
            if (shapes.Get(i).isInside(point))
            {
                return true;
            }
        }
    }
    return false;
}
}

}
