namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepChainDropped : AbstractProofStep
    {

    private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
    private Chain dropped = null;
    private Chain droppedOff = null;

    public ProofStepChainDropped(Chain dropped, Chain droppedOff)
    {
        this.dropped = dropped;
        this.droppedOff = droppedOff;
        this.predecessors.Add(droppedOff.getProofStep());
    }

    //
    // START-ProofStep
     
    public IQueue<ProofStep> getPredecessorSteps()
    {
        return Factory.CreateReadOnlyQueue<>(predecessors);
    }

     
    public string getProof()
    {
        return dropped.ToString();
    }

     
    public string getJustification()
    {
        return "Dropped: " + droppedOff.getProofStep().getStepNumber();
    }
    // END-ProofStep
    //
}
}
