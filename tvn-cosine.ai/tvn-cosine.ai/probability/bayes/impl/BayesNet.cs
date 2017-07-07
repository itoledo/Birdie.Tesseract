using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.probability.bayes.impl
{
    /**
     * Default implementation of the BayesianNetwork interface.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class BayesNet<T> : BayesianNetwork<T>
    {
        protected ISet<Node<T>> rootNodes = new HashSet<Node<T>>();
        protected IList<RandomVariable> variables = new List<RandomVariable>();
        protected IDictionary<RandomVariable, Node<T>> varToNodeMap = new Dictionary<RandomVariable, Node<T>>();

        public BayesNet(IEnumerable<Node<T>> rootNodes)
        {
            if (null == rootNodes)
            {
                throw new ArgumentException("Root Nodes need to be specified.");
            }
            foreach (Node<T> n in rootNodes)
            {
                this.rootNodes.Add(n);
            }
            if (this.rootNodes.Count != rootNodes.Count())
            {
                throw new ArgumentException("Duplicate Root Nodes Passed in.");
            }
            // Ensure is a DAG
            checkIsDAGAndCollectVariablesInTopologicalOrder();

            //TODO : make immutable
            //variables = Collections.unmodifiableList(variables);
        }

        //
        // START-BayesianNetwork

        public IList<RandomVariable> getVariablesInTopologicalOrder()
        {
            return variables;
        }

        public Node<T> getNode(RandomVariable rv)
        {
            return varToNodeMap[rv];
        }

        // END-BayesianNetwork
        //

        //
        // PRIVATE METHODS
        //
        private void checkIsDAGAndCollectVariablesInTopologicalOrder()
        {

            // Topological sort based on logic described at:
            // http://en.wikipedia.org/wiki/Topoligical_sorting
            ISet<Node<T>> seenAlready = new HashSet<Node<T>>();
            IDictionary<Node<T>, IList<Node<T>>> incomingEdges = new Dictionary<Node<T>, IList<Node<T>>>();
            ISet<Node<T>> s = new HashSet<Node<T>>();
            foreach (Node<T> n in this.rootNodes)
            {
                walkNode(n, seenAlready, incomingEdges, s);
            }
            while (!(s.Count == 0))
            {
                Node<T> n = s.First();
                s.Remove(n);
                variables.Add(n.getRandomVariable());
                varToNodeMap.Add(n.getRandomVariable(), n);
                foreach (Node<T> m in n.getChildren())
                {
                    IList<Node<T>> edges = incomingEdges[m];
                    edges.Remove(n);
                    if (edges.Count == 0)
                    {
                        s.Add(m);
                    }
                }
            }

            foreach (IList<Node<T>> edges in incomingEdges.Values)
            {
                if (!(edges.Count == 0))
                {
                    throw new ArgumentException("Network contains at least one cycle in it, must be a DAG.");
                }
            }
        }

        private void walkNode(Node<T> n, ISet<Node<T>> seenAlready,
                IDictionary<Node<T>, IList<Node<T>>> incomingEdges, ISet<Node<T>> rootNodes)
        {
            if (!seenAlready.Contains(n))
            {
                seenAlready.Add(n);
                // Check if has no incoming edges
                if (n.isRoot())
                {
                    rootNodes.Add(n);
                }
                incomingEdges.Add(n, new List<Node<T>>(n.getParents()));
                foreach (Node<T> c in n.getChildren())
                {
                    walkNode(c, seenAlready, incomingEdges, rootNodes);
                }
            }
        }
    }

}
