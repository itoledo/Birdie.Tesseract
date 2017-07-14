using System.Collections.Generic;
using System.Collections.ObjectModel;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepChainReduction : AbstractProofStep
    {
        private IList<ProofStep> predecessors = new List<ProofStep>();
        private Chain reduction = null;
        private Chain nearParent, farParent = null;
        private IDictionary<Variable, Term> subst = null;

        public ProofStepChainReduction(Chain reduction, Chain nearParent, Chain farParent, IDictionary<Variable, Term> subst)
        {
            this.reduction = reduction;
            this.nearParent = nearParent;
            this.farParent = farParent;
            this.subst = subst;
            this.predecessors.Add(farParent.getProofStep());
            this.predecessors.Add(nearParent.getProofStep());
        }

        //
        // START-ProofStep 
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return reduction.ToString();
        }
         
    public override string getJustification()
        {
            return "Reduction: " + nearParent.getProofStep().getStepNumber() + "," + farParent.getProofStep().getStepNumber() + " " + subst.CustomDictionaryWriterToString();
        }
        // END-ProofStep
        //
    } 
}
