using System.Collections.Generic;
using System.Collections.ObjectModel;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepChainDropped : AbstractProofStep
    {
        private IList<ProofStep> predecessors = new List<ProofStep>();
        private Chain dropped = null;
        private Chain droppedOff = null;

        public ProofStepChainDropped(Chain dropped, Chain droppedOff)
        {
            this.dropped = dropped;
            this.droppedOff = droppedOff;
            this.predecessors.Add(droppedOff.getProofStep());
        }

        //
        // START-ProofStep 
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return dropped.ToString();
        }

        public override string getJustification()
        {
            return "Dropped: " + droppedOff.getProofStep().getStepNumber();
        }
        // END-ProofStep
        //
    } 
}
