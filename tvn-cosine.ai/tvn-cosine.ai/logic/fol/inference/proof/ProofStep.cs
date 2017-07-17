namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public interface ProofStep
    {
        int getStepNumber();

        void setStepNumber(int step);

        List<ProofStep> getPredecessorSteps();

        String getProof();

        String getJustification();
    }
}
