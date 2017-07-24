using tvn.cosine.ai.common.collections.api;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public abstract class AbstractProofStep : ProofStep
    { 
        private int step = 0;

        public AbstractProofStep()
        { }
         
        public int getStepNumber()
        {
            return step;
        }

        public void setStepNumber(int step)
        {
            this.step = step;
        }

        public abstract ICollection<ProofStep> getPredecessorSteps(); 
        public abstract string getProof(); 
        public abstract string getJustification(); 
    }
}
