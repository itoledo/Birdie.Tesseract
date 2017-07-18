namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepChainCancellation : AbstractProofStep
    {

    private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
    private Chain cancellation = null;
    private Chain cancellationOf = null;
    private Map<Variable, Term> subst = null;

    public ProofStepChainCancellation(Chain cancellation, Chain cancellationOf,
            Map<Variable, Term> subst)
    {
        this.cancellation = cancellation;
        this.cancellationOf = cancellationOf;
        this.subst = subst;
        this.predecessors.Add(cancellationOf.getProofStep());
    }

    //
    // START-ProofStep
     
    public IQueue<ProofStep> getPredecessorSteps()
    {
        return Factory.CreateReadOnlyQueue<>(predecessors);
    }

     
    public string getProof()
    {
        return cancellation.ToString();
    }

     
    public string getJustification()
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
