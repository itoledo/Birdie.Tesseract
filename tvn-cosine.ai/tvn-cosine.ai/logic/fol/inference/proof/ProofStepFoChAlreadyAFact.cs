namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepFoChAlreadyAFact : AbstractProofStep
    {
    //
    private static final IQueue<ProofStep> _noPredecessors = Factory.CreateQueue<ProofStep>();
    //
    private Literal fact = null;

    public ProofStepFoChAlreadyAFact(Literal fact)
    {
        this.fact = fact;
    }

    //
    // START-ProofStep
     
    public IQueue<ProofStep> getPredecessorSteps()
    {
        return Factory.CreateReadOnlyQueue<>(_noPredecessors);
    }

     
    public string getProof()
    {
        return fact.ToString();
    }

     
    public string getJustification()
    {
        return "Already a known fact in the KB.";
    }
    // END-ProofStep
    //
}
}
