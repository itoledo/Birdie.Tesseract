using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepPremise : AbstractProofStep
    {
        //
        private static readonly IQueue<ProofStep> _noPredecessors = Factory.CreateQueue<ProofStep>();
        //
        private object proof = "";

        public ProofStepPremise(object proof)
        {
            this.proof = proof;
        }

        //
        // START-ProofStep

        public override IQueue<ProofStep> getPredecessorSteps()
        {
            return Factory.CreateReadOnlyQueue<ProofStep>(_noPredecessors);
        }


        public override string getProof()
        {
            return proof.ToString();
        }


        public override string getJustification()
        {
            return "Premise";
        }
        // END-ProofStep
        //
    }
}
