using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepChainReduction : AbstractProofStep
    { 
        private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
        private Chain reduction = null;
        private Chain nearParent, farParent = null;
        private IMap<Variable, Term> subst = null;

        public ProofStepChainReduction(Chain reduction, Chain nearParent,
                Chain farParent, IMap<Variable, Term> subst)
        {
            this.reduction = reduction;
            this.nearParent = nearParent;
            this.farParent = farParent;
            this.subst = subst;
            this.predecessors.Add(farParent.getProofStep());
            this.predecessors.Add(nearParent.getProofStep());
        }
         
        public override IQueue<ProofStep> getPredecessorSteps()
        {
            return Factory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }
         
        public override string getProof()
        {
            return reduction.ToString();
        }
         
        public override string getJustification()
        {
            return "Reduction: " + nearParent.getProofStep().getStepNumber() + ","
                    + farParent.getProofStep().getStepNumber() + " " + subst;
        } 
    }
}
