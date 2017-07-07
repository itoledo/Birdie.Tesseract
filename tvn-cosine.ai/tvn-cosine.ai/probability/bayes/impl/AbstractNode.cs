using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.probability.bayes.impl
{
    /**
     * Abstract base implementation of the Node interface.
     * 
     * @author Ciaran O'Reilly
     * @author Ravi Mohan
     */
    public abstract class AbstractNode<T> : Node<T>
    {
        private RandomVariable variable = null;
        private ISet<Node<T>> parents = null;
        private ISet<Node<T>> children = null;

        public AbstractNode(RandomVariable var)
                : this(var, (Node<T>[])null)
        { }

        public AbstractNode(RandomVariable var, IEnumerable<Node<T>> parents)
        {
            if (null == var)
            {
                throw new ArgumentException("Random Variable for Node must be specified.");
            }
            this.variable = var;
            this.parents = new HashSet<Node<T>>();
            if (null != parents)
            {
                foreach (Node<T> p in parents)
                {
                    ((AbstractNode<T>)p).addChild(this);
                    this.parents.Add(p);
                }
            }

            //TODO: Make immutabe
            ////this.parents = Collections.unmodifiableSet(this.parents);
            ////this.children = Collections.unmodifiableSet(new LinkedHashSet<Node>());
        }

        //
        // START-Node 
        public RandomVariable getRandomVariable()
        {
            return variable;
        }

        public bool isRoot()
        {
            return 0 == getParents().Count;
        }

        public ISet<Node<T>> getParents()
        {
            return parents;
        }

        public ISet<Node<T>> getChildren()
        {
            return children;
        }

        public ISet<Node<T>> getMarkovBlanket()
        {
            ISet<Node<T>> mb = new HashSet<Node<T>>();
            // Given its parents,
            foreach (var v in getParents())
                mb.Add(v);
            // children,
            foreach (var v in getChildren())
                mb.Add(v);
            // and children's parents
            foreach (Node<T> cn in getChildren())
            {
                foreach (var v in cn.getParents())
                    mb.Add(v);
            }

            return mb;
        }

        public abstract ConditionalProbabilityDistribution<T> getCPD();

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

            if (o is Node<T>)
            {
                Node<T> n = (Node<T>)o;

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
        protected void addChild(Node<T> childNode)
        {
            children = new HashSet<Node<T>>(children);

            children.Add(childNode);

            //TODO: Make immutable
            // children = Collections.unmodifiableSet(children);
        }
    } 
}
