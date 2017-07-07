using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.probability.bayes;

namespace tvn.cosine.ai.probability.bayes.impl
{
    /**
     * Default implementation of the FiniteNode interface that uses a fully
     * specified Conditional Probability Table to represent the Node's conditional
     * distribution.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class FullCPTNode<T> : AbstractNode<T>, FiniteNode<T>
    {
        private ConditionalProbabilityTable<T> cpt = null;

        public FullCPTNode(RandomVariable var, double[] distribution)
                : this(var, distribution, (Node<T>[])null)
        { }

        public FullCPTNode(RandomVariable var, double[] values, params Node<T>[] parents)
            : base(var, parents)
        {
            RandomVariable[] conditionedOn = new RandomVariable[getParents().Count];
            int i = 0;
            foreach (Node<T> p in getParents())
            {
                conditionedOn[i++] = p.getRandomVariable();
            }

            cpt = new CPT<T>(var, values, conditionedOn);
        }

        //
        // START-Node 
        public override ConditionalProbabilityDistribution<T> getCPD()
        {
            return getCPT();
        }

        // END-Node
        //

        //
        // START-FiniteNode

        public ConditionalProbabilityTable<T> getCPT()
        {
            return cpt;
        }

        // END-FiniteNode
        //
    }

}
