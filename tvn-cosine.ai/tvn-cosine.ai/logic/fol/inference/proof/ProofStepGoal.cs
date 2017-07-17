namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepGoal : AbstractProofStep
    {
    //
    private static final List<ProofStep> _noPredecessors = new ArrayList<ProofStep>();
    //
    private Object proof = "";

    public ProofStepGoal(Object proof)
    {
        this.proof = proof;
    }

    //
    // START-ProofStep
    @Override
    public List<ProofStep> getPredecessorSteps()
    {
        return Collections.unmodifiableList(_noPredecessors);
    }

    @Override
    public String getProof()
    {
        return proof.toString();
    }

    @Override
    public String getJustification()
    {
        return "Goal";
    }
    // END-ProofStep
    //
}
}
