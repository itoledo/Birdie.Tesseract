namespace tvn.cosine.ai.probability.bayes.impl
{
    /**
     * Default implementation of the DynamicBayesianNetwork interface.
     * 
     * @author Ciaran O'Reilly
     */
    public class DynamicBayesNet : BayesNet : DynamicBayesianNetwork
    {


    private ISet<RandomVariable> X_0 = Factory.CreateSet<RandomVariable>();
    private ISet<RandomVariable> X_1 = Factory.CreateSet<RandomVariable>();
    private ISet<RandomVariable> E_1 = Factory.CreateSet<RandomVariable>();
    private Map<RandomVariable, RandomVariable> X_0_to_X_1 = Factory.CreateMap<RandomVariable, RandomVariable>();
    private Map<RandomVariable, RandomVariable> X_1_to_X_0 = Factory.CreateMap<RandomVariable, RandomVariable>();
    private BayesianNetwork priorNetwork = null;
    private IQueue<RandomVariable> X_1_VariablesInTopologicalOrder = Factory.CreateQueue<RandomVariable>();

    public DynamicBayesNet(BayesianNetwork priorNetwork,
            Map<RandomVariable, RandomVariable> X_0_to_X_1,
            ISet<RandomVariable> E_1, params Node[] rootNodes)
    {
        base(rootNodes);

        for (Map.Entry<RandomVariable, RandomVariable> x0_x1 : X_0_to_X_1
                .entrySet())
        {
            RandomVariable x0 = x0_x1.getKey();
            RandomVariable x1 = x0_x1.getValue();
            this.X_0.Add(x0);
            this.X_1.Add(x1);
            this.X_0_to_X_1.Put(x0, x1);
            this.X_1_to_X_0.Put(x1, x0);
        }
        this.E_1.addAll(E_1);

        // Assert the X_0, X_1, and E_1 sets are of expected sizes
        ISet<RandomVariable> combined = Factory.CreateSet<RandomVariable>();
        combined.addAll(X_0);
        combined.addAll(X_1);
        combined.addAll(E_1);
        if (SetOps.difference(varToNodeMap.GetKeys(), combined).size() != 0)
        {
            throw new IllegalArgumentException(
                    "X_0, X_1, and E_1 do not map correctly to the Nodes describing this Dynamic Bayesian Network.");
        }
        this.priorNetwork = priorNetwork;

        X_1_VariablesInTopologicalOrder
                .addAll(getVariablesInTopologicalOrder());
        X_1_VariablesInTopologicalOrder.removeAll(X_0);
        X_1_VariablesInTopologicalOrder.removeAll(E_1);
    }

    //
    // START-DynamicBayesianNetwork
     
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

     
    public Map<RandomVariable, RandomVariable> getX_0_to_X_1()
    {
        return X_0_to_X_1;
    }

     
    public Map<RandomVariable, RandomVariable> getX_1_to_X_0()
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
