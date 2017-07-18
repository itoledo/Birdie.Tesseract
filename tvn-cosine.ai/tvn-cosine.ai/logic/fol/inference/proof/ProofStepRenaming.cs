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

        //
        // START-ProofStep
         
    public IQueue<ProofStep> getPredecessorSteps()
        {
            return Factory.CreateReadOnlyQueue<>(predecessors);
        }

         
    public string getProof()
        {
            return proof.ToString();
        }

         
    public string getJustification()
        {
            return "Renaming of " + predecessors.Get(0).getStepNumber();
        }
        // END-ProofStep
        //
    }
}
