using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.probability.bayes.impl
{
    /**
     * Default implementation of the BayesianNetwork interface.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public class BayesNet : BayesianNetwork
    {
        protected ISet<Node> rootNodes = Factory.CreateSet<Node>();
        protected IQueue<RandomVariable> variables = Factory.CreateQueue<RandomVariable>();
        protected IMap<RandomVariable, Node> varToNodeMap = Factory.CreateInsertionOrderedMap<RandomVariable, Node>();

        public BayesNet(params Node[] rootNodes)
        {
            if (null == rootNodes)
            {
                throw new IllegalArgumentException("Root Nodes need to be specified.");
            }
            foreach (Node n in rootNodes)
            {
                this.rootNodes.Add(n);
            }
            if (this.rootNodes.Size() != rootNodes.Length)
            {
                throw new IllegalArgumentException("Duplicate Root Nodes Passed in.");
            }
            // Ensure is a DAG
            checkIsDAGAndCollectVariablesInTopologicalOrder();
            variables = Factory.CreateReadOnlyQueue<RandomVariable>(variables);
        }

        //
        // START-BayesianNetwork

        public IQueue<RandomVariable> getVariablesInTopologicalOrder()
        {
            return variables;
        }


        public Node getNode(RandomVariable rv)
        {
            return varToNodeMap.Get(rv);
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
            ISet<Node> seenAlready = Factory.CreateSet<Node>();
            IMap<Node, IQueue<Node>> incomingEdges = Factory.CreateMap<Node, IQueue<Node>>();
            IQueue<Node> s = Factory.CreateFifoQueueNoDuplicates<Node>();
            foreach (Node n in this.rootNodes)
            {
                walkNode(n, seenAlready, incomingEdges, s);
            }
            while (!s.IsEmpty())
            {
                Node n = s.Pop();
                variables.Add(n.getRandomVariable());
                varToNodeMap.Put(n.getRandomVariable(), n);
                foreach (Node m in n.getChildren())
                {
                    IQueue<Node> edges = incomingEdges.Get(m);
                    edges.Remove(n);
                    if (edges.IsEmpty())
                    {
                        s.Add(m);
                    }
                }
            }

            foreach (IQueue<Node> edges in incomingEdges.GetValues())
            {
                if (!edges.IsEmpty())
                {
                    throw new IllegalArgumentException("Network contains at least one cycle in it, must be a DAG.");
                }
            }
        }

        private void walkNode(Node n, ISet<Node> seenAlready,
                IMap<Node, IQueue<Node>> incomingEdges, IQueue<Node> rootNodes)
        {
            if (!seenAlready.Contains(n))
            {
                seenAlready.Add(n);
                // Check if has no incoming edges
                if (n.isRoot())
                {
                    rootNodes.Add(n);
                }
                incomingEdges.Put(n, Factory.CreateQueue<Node>(n.getParents()));
                foreach (Node c in n.getChildren())
                {
                    walkNode(c, seenAlready, incomingEdges, rootNodes);
                }
            }
        }
    }

}
