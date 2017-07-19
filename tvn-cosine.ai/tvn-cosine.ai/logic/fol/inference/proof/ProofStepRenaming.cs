using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepRenaming : AbstractProofStep
    {
        private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
        private object proof = "";

        public ProofStepRenaming(object proof, ProofStep predecessor)
        {
            this.proof = proof;
            this.predecessors.Add(predecessor);
        }
         
        public override IQueue<ProofStep> getPredecessorSteps()
        {
            return Factory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }
         
        public override string getProof()
        {
            return proof.ToString();
        }
         
        public override string getJustification()
        {
            return "Renaming of " + predecessors.Get(0).getStepNumber();
        }
    }
}
