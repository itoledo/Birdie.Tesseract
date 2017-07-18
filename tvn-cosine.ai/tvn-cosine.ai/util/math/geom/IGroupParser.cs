﻿namespace tvn.cosine.ai.util.math.geom
{
    /**
     * This interface defines a parser that creates {@code ArrayList}s for geometric shapes represented through {@link IGeometric2D}
     * and their boundaries represented through {@link Rect2D} out of a given file.<br/>
     * The parser uses a {@link File} as an input and a {@code String} representing the ID of a group of shapes.
     * 
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     * 
     */
    public interface IGroupParser
    {
        /**
         * Parses the given {@link File} into a group of geometric shapes.
         * @param input the given input stream.
         * @param groupID the identifier for the group.
         * @throws Exception if an error is found while parsing the input.
         * @return the constructed list of geometric shapes.
         */
        ArrayList<IGeometric2D> parse(InputStream input, string groupID) throws Exception;
    }
}
