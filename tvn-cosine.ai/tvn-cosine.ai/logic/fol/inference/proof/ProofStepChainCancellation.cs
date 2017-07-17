namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepChainCancellation : AbstractProofStep
    {

    private List<ProofStep> predecessors = new ArrayList<ProofStep>();
    private Chain cancellation = null;
    private Chain cancellationOf = null;
    private Map<Variable, Term> subst = null;

    public ProofStepChainCancellation(Chain cancellation, Chain cancellationOf,
            Map<Variable, Term> subst)
    {
        this.cancellation = cancellation;
        this.cancellationOf = cancellationOf;
        this.subst = subst;
        this.predecessors.add(cancellationOf.getProofStep());
    }

    //
    // START-ProofStep
    @Override
    public List<ProofStep> getPredecessorSteps()
    {
        return Collections.unmodifiableList(predecessors);
    }

    @Override
    public String getProof()
    {
        return cancellation.toString();
    }

    @Override
    public String getJustification()
    {
        return "Cancellation: " + cancellationOf.getProofStep().getStepNumber()
                + " " + subst;
    }
    // END-ProofStep
    //
}
Contact GitHub API Training Shop Blog About
© 2017 GitHub, Inc.Terms Privacy Security Status Help
}
