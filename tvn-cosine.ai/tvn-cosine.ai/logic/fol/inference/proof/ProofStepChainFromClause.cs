namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepChainFromClause : AbstractProofStep
    {

    private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
    private Chain chain = null;
    private Clause fromClause = null;

    public ProofStepChainFromClause(Chain chain, Clause fromClause)
    {
        this.chain = chain;
        this.fromClause = fromClause;
        this.predecessors.Add(fromClause.getProofStep());
    }

    //
    // START-ProofStep
     
    public IQueue<ProofStep> getPredecessorSteps()
    {
        return Factory.CreateReadOnlyQueue<>(predecessors);
    }

     
    public string getProof()
    {
        return chain.ToString();
    }

     
    public string getJustification()
    {
        return "Chain from Clause: "
                + fromClause.getProofStep().getStepNumber();
    }
    // END-ProofStep
    //
}
}
