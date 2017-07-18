namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepChainContrapositive : AbstractProofStep
    {

    private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
    private Chain contrapositive = null;
    private Chain contrapositiveOf = null;

    public ProofStepChainContrapositive(Chain contrapositive,
            Chain contrapositiveOf)
    {
        this.contrapositive = contrapositive;
        this.contrapositiveOf = contrapositiveOf;
        this.predecessors.Add(contrapositiveOf.getProofStep());
    }

    //
    // START-ProofStep
     
    public IQueue<ProofStep> getPredecessorSteps()
    {
        return Factory.CreateReadOnlyQueue<>(predecessors);
    }

     
    public string getProof()
    {
        return contrapositive.ToString();
    }

     
    public string getJustification()
    {
        return "Contrapositive: "
                + contrapositiveOf.getProofStep().getStepNumber();
    }
    // END-ProofStep
    //
}
}
