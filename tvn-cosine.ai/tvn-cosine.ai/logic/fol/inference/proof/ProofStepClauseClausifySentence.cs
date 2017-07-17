namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepClauseClausifySentence extends AbstractProofStep
    {

    private List<ProofStep> predecessors = new ArrayList<ProofStep>();
    private Clause clausified = null;

    public ProofStepClauseClausifySentence(Clause clausified,
            Sentence origSentence)
    {
        this.clausified = clausified;
        this.predecessors.add(new ProofStepPremise(origSentence));
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
        return clausified.toString();
    }

    @Override
    public String getJustification()
    {
        return "Clausified " + predecessors.get(0).getStepNumber();
    }
    // END-ProofStep
    //
}
}
