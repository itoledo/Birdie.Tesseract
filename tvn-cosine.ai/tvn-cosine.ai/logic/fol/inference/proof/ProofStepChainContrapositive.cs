using System.Collections.Generic;
using System.Collections.ObjectModel;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepChainContrapositive : AbstractProofStep
    {
        private IList<ProofStep> predecessors = new List<ProofStep>();
        private Chain contrapositive = null;
        private Chain contrapositiveOf = null;

        public ProofStepChainContrapositive(Chain contrapositive, Chain contrapositiveOf)
        {
            this.contrapositive = contrapositive;
            this.contrapositiveOf = contrapositiveOf;
            this.predecessors.Add(contrapositiveOf.getProofStep());
        }

        //
        // START-ProofStep 
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return contrapositive.ToString();
        }

        public override string getJustification()
        {
            return "Contrapositive: " + contrapositiveOf.getProofStep().getStepNumber();
        }
        // END-ProofStep
        //
    }
}
