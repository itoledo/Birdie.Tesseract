namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepFoChAlreadyAFact : AbstractProofStep
    {
    //
    private static final List<ProofStep> _noPredecessors = new ArrayList<ProofStep>();
    //
    private Literal fact = null;

    public ProofStepFoChAlreadyAFact(Literal fact)
    {
        this.fact = fact;
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
        return fact.toString();
    }

    @Override
    public String getJustification()
    {
        return "Already a known fact in the KB.";
    }
    // END-ProofStep
    //
}
}
