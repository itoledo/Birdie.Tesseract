namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepClauseDemodulation : AbstractProofStep
    {

    private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
    private Clause demodulated = null;
    private Clause origClause = null;
    private TermEquality assertion = null;

    public ProofStepClauseDemodulation(Clause demodulated, Clause origClause,
            TermEquality assertion)
    {
        this.demodulated = demodulated;
        this.origClause = origClause;
        this.assertion = assertion;
        this.predecessors.Add(origClause.getProofStep());
    }

    //
    // START-ProofStep
     
    public IQueue<ProofStep> getPredecessorSteps()
    {
        return Factory.CreateReadOnlyQueue<>(predecessors);
    }

     
    public string getProof()
    {
        return demodulated.ToString();
    }

     
    public string getJustification()
    {
        return "Demodulation: " + origClause.getProofStep().getStepNumber()
                + ", [" + assertion + "]";
    }
    // END-ProofStep
    //
}
}
