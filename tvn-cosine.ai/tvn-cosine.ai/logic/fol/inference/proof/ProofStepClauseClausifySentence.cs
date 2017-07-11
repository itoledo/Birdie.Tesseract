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
    public class ProofStepClauseClausifySentence : AbstractProofStep
    {
        private IList<ProofStep> predecessors = new List<ProofStep>();
        private Clause clausified = null;

        public ProofStepClauseClausifySentence(Clause clausified, Sentence origSentence)
        {
            this.clausified = clausified;
            this.predecessors.Add(new ProofStepPremise(origSentence));
        }

        //
        // START-ProofStep

        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return clausified.ToString();
        }

        public override string getJustification()
        {
            return "Clausified " + predecessors[0].getStepNumber();
        }
        // END-ProofStep
        //
    }

}
