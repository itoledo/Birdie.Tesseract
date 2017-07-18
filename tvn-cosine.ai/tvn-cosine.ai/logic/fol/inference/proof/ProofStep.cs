using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public interface ProofStep
    {
        int getStepNumber();

        void setStepNumber(int step);

        IQueue<ProofStep> getPredecessorSteps();

        string getProof();

        string getJustification();
    }
}
