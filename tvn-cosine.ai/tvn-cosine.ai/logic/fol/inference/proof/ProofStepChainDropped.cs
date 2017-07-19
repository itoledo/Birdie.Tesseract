using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepChainDropped : AbstractProofStep
    { 
        private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
        private Chain dropped = null;
        private Chain droppedOff = null;

        public ProofStepChainDropped(Chain dropped, Chain droppedOff)
        {
            this.dropped = dropped;
            this.droppedOff = droppedOff;
            this.predecessors.Add(droppedOff.getProofStep());
        }

        public override IQueue<ProofStep> getPredecessorSteps()
        {
            return Factory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }
         
        public override string getProof()
        {
            return dropped.ToString();
        }
         
        public override string getJustification()
        {
            return "Dropped: " + droppedOff.getProofStep().getStepNumber();
        }
    }
}
