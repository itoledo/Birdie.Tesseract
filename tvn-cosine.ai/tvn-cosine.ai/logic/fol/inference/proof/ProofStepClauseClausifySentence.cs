namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepClauseClausifySentence : AbstractProofStep
    {

    private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
    private Clause clausified = null;

    public ProofStepClauseClausifySentence(Clause clausified,
            Sentence origSentence)
    {
        this.clausified = clausified;
        this.predecessors.Add(new ProofStepPremise(origSentence));
    }

    //
    // START-ProofStep
     
    public IQueue<ProofStep> getPredecessorSteps()
    {
        return Factory.CreateReadOnlyQueue<>(predecessors);
    }

     
    public string getProof()
    {
        return clausified.ToString();
    }

     
    public string getJustification()
    {
        return "Clausified " + predecessors.Get(0).getStepNumber();
    }
    // END-ProofStep
    //
}
}
