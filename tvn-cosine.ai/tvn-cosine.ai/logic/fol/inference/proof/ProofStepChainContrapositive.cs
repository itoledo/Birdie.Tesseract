using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepChainContrapositive : AbstractProofStep
    { 
        private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
        private Chain contrapositive = null;
        private Chain contrapositiveOf = null;

        public ProofStepChainContrapositive(Chain contrapositive, Chain contrapositiveOf)
        {
            this.contrapositive = contrapositive;
            this.contrapositiveOf = contrapositiveOf;
            this.predecessors.Add(contrapositiveOf.getProofStep());
        }

        public override IQueue<ProofStep> getPredecessorSteps()
        {
            return Factory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return contrapositive.ToString();
        }

        public override string getJustification()
        {
            return "Contrapositive: " + contrapositiveOf.getProofStep().getStepNumber();
        }
    }
}
