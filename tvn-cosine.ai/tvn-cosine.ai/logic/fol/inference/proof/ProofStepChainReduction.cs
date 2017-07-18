namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepChainReduction : AbstractProofStep
    {

    private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
    private Chain reduction = null;
    private Chain nearParent, farParent = null;
    private Map<Variable, Term> subst = null;

    public ProofStepChainReduction(Chain reduction, Chain nearParent,
            Chain farParent, Map<Variable, Term> subst)
    {
        this.reduction = reduction;
        this.nearParent = nearParent;
        this.farParent = farParent;
        this.subst = subst;
        this.predecessors.Add(farParent.getProofStep());
        this.predecessors.Add(nearParent.getProofStep());
    }

    //
    // START-ProofStep
     
    public IQueue<ProofStep> getPredecessorSteps()
    {
        return Factory.CreateReadOnlyQueue<>(predecessors);
    }

     
    public string getProof()
    {
        return reduction.ToString();
    }

     
    public string getJustification()
    {
        return "Reduction: " + nearParent.getProofStep().getStepNumber() + ","
                + farParent.getProofStep().getStepNumber() + " " + subst;
    }
    // END-ProofStep
    //
}
}
