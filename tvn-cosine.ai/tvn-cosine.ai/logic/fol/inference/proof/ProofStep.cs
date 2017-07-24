using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public interface ProofStep
    {
        int getStepNumber();

        void setStepNumber(int step);

        ICollection<ProofStep> getPredecessorSteps();

        string getProof();

        string getJustification();
    }
}
