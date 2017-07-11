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
    public class ProofStepClauseParamodulation : AbstractProofStep
    {
        private IList<ProofStep> predecessors = new List<ProofStep>();
        private Clause paramodulated = null;
        private Clause topClause = null;
        private Clause equalityClause = null;
        private TermEquality assertion = null;

        public ProofStepClauseParamodulation(Clause paramodulated, Clause topClause, Clause equalityClause, TermEquality assertion)
        {
            this.paramodulated = paramodulated;
            this.topClause = topClause;
            this.equalityClause = equalityClause;
            this.assertion = assertion;
            this.predecessors.Add(topClause.getProofStep());
            this.predecessors.Add(equalityClause.getProofStep());
        }

        //
        // START-ProofStep 
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }
         
    public override string getProof()
        {
            return paramodulated.ToString();
        }
         
    public override string getJustification()
        {
            return "Paramodulation: " + topClause.getProofStep().getStepNumber()
                    + ", " + equalityClause.getProofStep().getStepNumber() + ", ["
                    + assertion + "]";

        }
        // END-ProofStep
        //
    } 
}
