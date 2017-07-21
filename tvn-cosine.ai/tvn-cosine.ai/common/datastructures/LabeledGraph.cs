using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.common.datastructures
{
    /**
     * Represents a directed labeled graph. Vertices are represented by their unique
     * labels and labeled edges by means of nested hashtables. Variant of class
     * {@code aima.util.Table}. This version is more dynamic, it requires no
     * initialization and can add new items whenever needed.
     * 
     * @author R. Lunde
     * @author Mike Stampone
     */
    public class LabeledGraph<VertexLabelType, EdgeLabelType>
    {
        /**
         * Lookup for edge label information. Contains an entry for every vertex label.
         */
        private readonly IMap<VertexLabelType, IMap<VertexLabelType, EdgeLabelType>> globalEdgeLookup;
        /** List of the labels of all vertices within the graph. */
        private readonly IQueue<VertexLabelType> vertexLabels;

        /** Creates a new empty graph. */
        public LabeledGraph()
        {
            globalEdgeLookup = Factory.CreateInsertionOrderedMap<VertexLabelType, IMap<VertexLabelType, EdgeLabelType>>();
            vertexLabels = Factory.CreateFifoQueue<VertexLabelType>();
        }

        /**
         * Adds a new vertex to the graph if it is not already present.
         * 
         * @param v
         *            the vertex to add
         */
        public void addVertex(VertexLabelType v)
        {
            checkForNewVertex(v);
        }

        /**
         * Adds a directed labeled edge to the graph. The end points of the edge are
         * specified by vertex labels. New vertices are automatically identified and
         * added to the graph.
         * 
         * @param from
         *            the first vertex of the edge
         * @param to
         *            the second vertex of the edge
         * @param el
         *            an edge label
         */
        public void set(VertexLabelType from, VertexLabelType to, EdgeLabelType el)
        {
            IMap<VertexLabelType, EdgeLabelType> localEdgeLookup = checkForNewVertex(from);
            localEdgeLookup.Put(to, el);
            checkForNewVertex(to);
        }

        /** Handles new vertices. */
        private IMap<VertexLabelType, EdgeLabelType> checkForNewVertex(
                VertexLabelType v)
        {
            IMap<VertexLabelType, EdgeLabelType> result = globalEdgeLookup.Get(v);
            if (result == null)
            {
                result = Factory.CreateInsertionOrderedMap<VertexLabelType, EdgeLabelType>();
                globalEdgeLookup.Put(v, result);
                vertexLabels.Add(v);
            }
            return result;
        }

        /**
         * Removes an edge from the graph.
         * 
         * @param from
         *            the first vertex of the edge
         * @param to
         *            the second vertex of the edge
         */
        public void remove(VertexLabelType from, VertexLabelType to)
        {
            IMap<VertexLabelType, EdgeLabelType> localEdgeLookup = globalEdgeLookup.Get(from);
            if (localEdgeLookup != null)
                localEdgeLookup.Remove(to);
        }

        /**
         * Returns the label of the edge between the specified vertices, or null if
         * there is no edge between them.
         * 
         * @param from
         *            the first vertex of the ege
         * @param to
         *            the second vertex of the edge
         * 
         * @return the label of the edge between the specified vertices, or null if
         *         there is no edge between them.
         */
        public EdgeLabelType get(VertexLabelType from, VertexLabelType to)
        {
            IMap<VertexLabelType, EdgeLabelType> localEdgeLookup = globalEdgeLookup.Get(from);
            return localEdgeLookup == null ? default(EdgeLabelType) : localEdgeLookup.Get(to);
        }

        /**
         * Returns the labels of those vertices which can be obtained by following
         * the edges starting at the specified vertex.
         */
        public IQueue<VertexLabelType> getSuccessors(VertexLabelType v)
        {
            IQueue<VertexLabelType> result = Factory.CreateQueue<VertexLabelType>();
            IMap<VertexLabelType, EdgeLabelType> localEdgeLookup = globalEdgeLookup.Get(v);
            if (localEdgeLookup != null)
            {
                result.AddAll(localEdgeLookup.GetKeys());
            }
            return result;
        }

        /** Returns the labels of all vertices within the graph. */
        public IQueue<VertexLabelType> getVertexLabels()
        {
            return vertexLabels;
        }

        /** Checks whether the given label is the label of one of the vertices. */
        public bool isVertexLabel(VertexLabelType v)
        {
            return globalEdgeLookup.Get(v) != null;
        }

        /** Removes all vertices and all edges from the graph. */
        public void clear()
        {
            vertexLabels.Clear();
            globalEdgeLookup.Clear();
        }
    }
}
