using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepRenaming : AbstractProofStep
    { 
        private IList<ProofStep> predecessors = new List<ProofStep>();
        private object proof = "";

        public ProofStepRenaming(object proof, ProofStep predecessor)
        {
            this.proof = proof;
            this.predecessors.Add(predecessor);
        }

        //
        // START-ProofStep 
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return proof.ToString();
        }

        public override string getJustification()
        {
            return "Renaming of " + predecessors[0].getStepNumber();
        }
        // END-ProofStep
        //
    } 
}
