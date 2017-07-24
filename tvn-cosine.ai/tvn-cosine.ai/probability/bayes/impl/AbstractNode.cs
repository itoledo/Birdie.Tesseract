using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.common.exceptions;

namespace tvn.cosine.ai.probability.bayes.impl
{
    /**
     * Abstract base implementation of the Node interface.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public abstract class AbstractNode : Node
    { 
        private RandomVariable variable = null;
        private ISet<Node> parents = null;
        private ISet<Node> children = null;

        public AbstractNode(RandomVariable var)
                : this(var, (Node[])null)
        { }

        public AbstractNode(RandomVariable var, params Node[] parents)
        {
            if (null == var)
            {
                throw new IllegalArgumentException("Random Variable for Node must be specified.");
            }
            this.variable = var;
            this.parents = CollectionFactory.CreateSet<Node>();
            if (null != parents)
            {
                foreach (Node p in parents)
                {
                    ((AbstractNode)p).addChild(this);
                    this.parents.Add(p);
                }
            }
            this.parents = CollectionFactory.CreateReadOnlySet<Node>(this.parents);
            this.children = CollectionFactory.CreateReadOnlySet<Node>(CollectionFactory.CreateSet<Node>());
        }

        //
        // START-Node

        public RandomVariable getRandomVariable()
        {
            return variable;
        }


        public bool isRoot()
        {
            return 0 == getParents().Size();
        }


        public ISet<Node> getParents()
        {
            return parents;
        }


        public ISet<Node> getChildren()
        {
            return children;
        }


        public ISet<Node> getMarkovBlanket()
        {
            ISet<Node> mb = CollectionFactory.CreateSet<Node>();
            // Given its parents,
            mb.AddAll(getParents());
            // children,
            mb.AddAll(getChildren());
            // and children's parents
            foreach (Node cn in getChildren())
            {
                mb.AddAll(cn.getParents());
            }

            return mb;
        }

        public abstract ConditionalProbabilityDistribution getCPD();

        // END-Node
        //


        public override string ToString()
        {
            return getRandomVariable().getName();
        }


        public override bool Equals(object o)
        {
            if (null == o)
            {
                return false;
            }
            if (o == this)
            {
                return true;
            }

            if (o is Node)
            {
                Node n = (Node)o;

                return getRandomVariable().Equals(n.getRandomVariable());
            }

            return false;
        }


        public override int GetHashCode()
        {
            return variable.GetHashCode();
        }

        //
        // PROTECTED METHODS
        //
        protected void addChild(Node childNode)
        {
            children = CollectionFactory.CreateSet<Node>(children);

            children.Add(childNode);

            children = CollectionFactory.CreateReadOnlySet<Node>(children);
        }
    }

}
