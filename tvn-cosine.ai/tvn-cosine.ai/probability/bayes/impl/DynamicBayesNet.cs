using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.probability.bayes.impl
{
    /**
     * Default implementation of the DynamicBayesianNetwork interface.
     * 
     * @author Ciaran O'Reilly
     */
    public class DynamicBayesNet<T> : BayesNet<T>, DynamicBayesianNetwork<T>
    {


        private ISet<RandomVariable> X_0 = new HashSet<RandomVariable>();
        private ISet<RandomVariable> X_1 = new HashSet<RandomVariable>();
        private ISet<RandomVariable> E_1 = new HashSet<RandomVariable>();
        private IDictionary<RandomVariable, RandomVariable> X_0_to_X_1 = new Dictionary<RandomVariable, RandomVariable>();
        private IDictionary<RandomVariable, RandomVariable> X_1_to_X_0 = new Dictionary<RandomVariable, RandomVariable>();
        private BayesianNetwork<T> priorNetwork = null;
        private List<RandomVariable> X_1_VariablesInTopologicalOrder = new List<RandomVariable>();

        public DynamicBayesNet(BayesianNetwork<T> priorNetwork,
                IDictionary<RandomVariable, RandomVariable> X_0_to_X_1,
                ISet<RandomVariable> E_1, params Node<T>[] rootNodes)
            : base(rootNodes)
        {


            foreach (var x0_x1 in X_0_to_X_1)
            {
                RandomVariable x0 = x0_x1.Key;
                RandomVariable x1 = x0_x1.Value;
                this.X_0.Add(x0);
                this.X_1.Add(x1);
                this.X_0_to_X_1.Add(x0, x1);
                this.X_1_to_X_0.Add(x1, x0);
            }
            foreach (var v in E_1)
                this.E_1.Add(v);

            // Assert the X_0, X_1, and E_1 sets are of expected sizes
            ISet<RandomVariable> combined = new HashSet<RandomVariable>();

            foreach (var v in X_0)
                combined.Add(v);
            foreach (var v in X_1)
                combined.Add(v);
            foreach (var v in E_1)
                combined.Add(v);
            if (varToNodeMap.Keys.Except(combined).Count() != 0)
            {
                throw new ArgumentException("X_0, X_1, and E_1 do not map correctly to the Nodes describing this Dynamic Bayesian Network.");
            }
            this.priorNetwork = priorNetwork;

            X_1_VariablesInTopologicalOrder.AddRange(getVariablesInTopologicalOrder());
            foreach (var v in X_0)
                X_1_VariablesInTopologicalOrder.Remove(v);

            foreach (var v in E_1)
                X_1_VariablesInTopologicalOrder.Remove(v);
        }

        //
        // START-DynamicBayesianNetwork 
        public BayesianNetwork<T> getPriorNetwork()
        {
            return priorNetwork;
        }

        public ISet<RandomVariable> getX_0()
        {
            return X_0;
        }

        public ISet<RandomVariable> getX_1()
        {
            return X_1;
        }

        public IList<RandomVariable> getX_1_VariablesInTopologicalOrder()
        {
            return X_1_VariablesInTopologicalOrder;
        }

        public IDictionary<RandomVariable, RandomVariable> getX_0_to_X_1()
        {
            return X_0_to_X_1;
        }


        public IDictionary<RandomVariable, RandomVariable> getX_1_to_X_0()
        {
            return X_1_to_X_0;
        }

        public ISet<RandomVariable> getE_1()
        {
            return E_1;
        }

        // END-DynamicBayesianNetwork
        //

        //
        // PRIVATE METHODS
        //
    } 
}
