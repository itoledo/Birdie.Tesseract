using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.exceptions;
using tvn.cosine.ai.util;

namespace tvn.cosine.ai.probability.bayes.impl
{
    /**
     * Default implementation of the DynamicBayesianNetwork interface.
     * 
     * @author Ciaran O'Reilly
     */
    public class DynamicBayesNet : BayesNet, DynamicBayesianNetwork
    {
        private ISet<RandomVariable> X_0 = Factory.CreateSet<RandomVariable>();
        private ISet<RandomVariable> X_1 = Factory.CreateSet<RandomVariable>();
        private ISet<RandomVariable> E_1 = Factory.CreateSet<RandomVariable>();
        private IMap<RandomVariable, RandomVariable> X_0_to_X_1 = Factory.CreateMap<RandomVariable, RandomVariable>();
        private IMap<RandomVariable, RandomVariable> X_1_to_X_0 = Factory.CreateMap<RandomVariable, RandomVariable>();
        private BayesianNetwork priorNetwork = null;
        private IQueue<RandomVariable> X_1_VariablesInTopologicalOrder = Factory.CreateQueue<RandomVariable>();

        public DynamicBayesNet(BayesianNetwork priorNetwork,
                IMap<RandomVariable, RandomVariable> X_0_to_X_1,
                ISet<RandomVariable> E_1, params Node[] rootNodes)
            : base(rootNodes)
        {
            foreach (var x0_x1 in X_0_to_X_1)
            {
                RandomVariable x0 = x0_x1.GetKey();
                RandomVariable x1 = x0_x1.GetValue();
                this.X_0.Add(x0);
                this.X_1.Add(x1);
                this.X_0_to_X_1.Put(x0, x1);
                this.X_1_to_X_0.Put(x1, x0);
            }
            this.E_1.AddAll(E_1);

            // Assert the X_0, X_1, and E_1 sets are of expected sizes
            ISet<RandomVariable> combined = Factory.CreateSet<RandomVariable>();
            combined.AddAll(X_0);
            combined.AddAll(X_1);
            combined.AddAll(E_1);
            if (SetOps.difference(Factory.CreateSet<RandomVariable>(varToNodeMap.GetKeys()), combined).Size() != 0)
            {
                throw new IllegalArgumentException("X_0, X_1, and E_1 do not map correctly to the Nodes describing this Dynamic Bayesian Network.");
            }
            this.priorNetwork = priorNetwork;

            X_1_VariablesInTopologicalOrder.AddAll(getVariablesInTopologicalOrder());
            X_1_VariablesInTopologicalOrder.RemoveAll(X_0);
            X_1_VariablesInTopologicalOrder.RemoveAll(E_1);
        }

        public BayesianNetwork getPriorNetwork()
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


        public IQueue<RandomVariable> getX_1_VariablesInTopologicalOrder()
        {
            return X_1_VariablesInTopologicalOrder;
        }


        public IMap<RandomVariable, RandomVariable> getX_0_to_X_1()
        {
            return X_0_to_X_1;
        }


        public IMap<RandomVariable, RandomVariable> getX_1_to_X_0()
        {
            return X_1_to_X_0;
        }


        public ISet<RandomVariable> getE_1()
        {
            return E_1;
        }
    }
}
