using System.Collections.Generic;

namespace tvn.cosine.ai.Util.DataStructures
{
    /// <summary>
    /// Represents a directed labeled graph.Vertices are represented by their unique
    /// labels and labeled edges by means of nested hashtables. Variant of class
    /// <see cref="Table"/>. This version is more dynamic, it requires no
    /// initialization and can add new items whenever needed.
    /// </summary>
    /// <typeparam name="VertexLabelType"></typeparam>
    /// <typeparam name="EdgeLabelType"></typeparam>
    public class LabeledGraph<VertexLabelType, EdgeLabelType>
    {
        /// <summary>
        /// Lookup for edge label information. Contains an entry for every vertex label.
        /// </summary>
        private readonly IDictionary<VertexLabelType, IDictionary<VertexLabelType, EdgeLabelType>> globalEdgeLookup;

        /// <summary>
        /// List of the labels of all vertices within the graph.
        /// </summary>
        private readonly IList<VertexLabelType> vertexLabels;

        /// <summary>
        /// Creates a new empty graph.
        /// </summary>
        public LabeledGraph()
        {
            globalEdgeLookup = new Dictionary<VertexLabelType, IDictionary<VertexLabelType, EdgeLabelType>>();
            vertexLabels = new List<VertexLabelType>();
        }

        /// <summary>
        /// Adds a new vertex to the graph if it is not already present.
        /// </summary>
        /// <param name="v">the vertex to add</param>
        public void AddVertex(VertexLabelType v)
        {
            checkForNewVertex(v);
        }

        /// <summary>
        /// Adds a directed labeled edge to the graph.The end points of the edge are
        /// specified by vertex labels. New vertices are automatically identified and
        /// added to the graph.
        /// </summary>
        /// <param name="from">the first vertex of the edge</param>
        /// <param name="to">the second vertex of the edge</param>
        /// <param name="el">an edge label</param>
        public void Set(VertexLabelType from, VertexLabelType to, EdgeLabelType el)
        {
            var localEdgeLookup = checkForNewVertex(from);
            localEdgeLookup[to] = el;
            checkForNewVertex(to);
        }


        /// <summary>
        /// Handles new vertices.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private IDictionary<VertexLabelType, EdgeLabelType> checkForNewVertex(VertexLabelType v)
        {
            if (!globalEdgeLookup.ContainsKey(v))
            {
                globalEdgeLookup[v] = new Dictionary<VertexLabelType, EdgeLabelType>();
                vertexLabels.Add(v);
            }
            return globalEdgeLookup[v];
        }

        /// <summary>
        /// Removes an edge from the graph.
        /// </summary>
        /// <param name="from">the first vertex of the edge</param>
        /// <param name="to">the second vertex of the edge</param>
        public void Remove(VertexLabelType from, VertexLabelType to)
        {
            if (globalEdgeLookup.ContainsKey(from))
                globalEdgeLookup[from].Remove(to);
        }

        /// <summary>
        /// Returns the label of the edge between the specified vertices, or null if
        /// there is no edge between them.
        /// </summary>
        /// <param name="from">the first vertex of the edge</param>
        /// <param name="to">the second vertex of the edge</param>
        /// <returns>the label of the edge between the specified vertices, or null if there is no edge between them.</returns>
        public EdgeLabelType Get(VertexLabelType from, VertexLabelType to)
        {
            return globalEdgeLookup.ContainsKey(from) ? globalEdgeLookup[from][to] : default(EdgeLabelType);
        }


        /// <summary>
    /// Returns the labels of those vertices which can be obtained by following
    /// the edges starting at the specified vertex.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public IList<VertexLabelType> GetSuccessors(VertexLabelType v)
        {
            var result = new List<VertexLabelType>(); 
            if (globalEdgeLookup.ContainsKey(v))
                result.AddRange(globalEdgeLookup[v].Keys);
            return result;
        }

        /// <summary>
        /// Returns the labels of all vertices within the graph.
        /// </summary>
        /// <returns></returns>
        public IList<VertexLabelType> GetVertexLabels()
        {
            return vertexLabels;
        }

        /// <summary>
        /// Checks whether the given label is the label of one of the vertices.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool IsVertexLabel(VertexLabelType v)
        {
            return globalEdgeLookup.ContainsKey(v);
        }

        /// <summary>
        /// Removes all vertices and all edges from the graph. 
        /// </summary>
        public void Clear()
        {
            vertexLabels.Clear();
            globalEdgeLookup.Clear();
        }
    } 
}
