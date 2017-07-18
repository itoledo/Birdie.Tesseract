namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepGoal : AbstractProofStep
    {
    //
    private static final IQueue<ProofStep> _noPredecessors = Factory.CreateQueue<ProofStep>();
    //
    private object proof = "";

    public ProofStepGoal(object proof)
    {
        this.proof = proof;
    }

    //
    // START-ProofStep
     
    public IQueue<ProofStep> getPredecessorSteps()
    {
        return Factory.CreateReadOnlyQueue<>(_noPredecessors);
    }

     
    public string getProof()
    {
        return proof.ToString();
    }

     
    public string getJustification()
    {
        return "Goal";
    }
    // END-ProofStep
    //
}
}
