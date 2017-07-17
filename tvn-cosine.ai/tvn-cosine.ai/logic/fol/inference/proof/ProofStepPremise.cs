namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepPremise extends AbstractProofStep
    {
    //
    private static final List<ProofStep> _noPredecessors = new ArrayList<ProofStep>();
    //
    private Object proof = "";

    public ProofStepPremise(Object proof)
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
        return "Premise";
    }
    // END-ProofStep
    //
}
}
