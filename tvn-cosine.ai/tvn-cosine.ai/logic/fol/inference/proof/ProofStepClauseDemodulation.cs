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
    public class ProofStepClauseDemodulation : AbstractProofStep
    {
        private IList<ProofStep> predecessors = new List<ProofStep>();
        private Clause demodulated = null;
        private Clause origClause = null;
        private TermEquality assertion = null;

        public ProofStepClauseDemodulation(Clause demodulated, Clause origClause, TermEquality assertion)
        {
            this.demodulated = demodulated;
            this.origClause = origClause;
            this.assertion = assertion;
            this.predecessors.Add(origClause.getProofStep());
        }

        //
        // START-ProofStep 
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return demodulated.ToString();
        }

        public override string getJustification()
        {
            return "Demodulation: " + origClause.getProofStep().getStepNumber() + ", [" + assertion + "]";
        }
        // END-ProofStep
        //
    } 
}
