using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepPremise : AbstractProofStep
    {
        //
        private static readonly IList<ProofStep> _noPredecessors = new List<ProofStep>();
        //
        private object proof = "";

        public ProofStepPremise(object proof)
        {
            this.proof = proof;
        }

        //
        // START-ProofStep 
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(_noPredecessors);
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
