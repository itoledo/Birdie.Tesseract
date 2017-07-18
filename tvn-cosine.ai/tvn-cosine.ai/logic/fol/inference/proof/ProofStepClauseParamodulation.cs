namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepClauseParamodulation : AbstractProofStep
    {
        private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
        private Clause paramodulated = null;
        private Clause topClause = null;
        private Clause equalityClause = null;
        private TermEquality assertion = null;

        public ProofStepClauseParamodulation(Clause paramodulated,
                Clause topClause, Clause equalityClause, TermEquality assertion)
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
         
    public IQueue<ProofStep> getPredecessorSteps()
        {
            return Factory.CreateReadOnlyQueue<>(predecessors);
        }

         
    public string getProof()
        {
            return paramodulated.ToString();
        }

         
    public string getJustification()
        {
            return "Paramodulation: " + topClause.getProofStep().getStepNumber()
                    + ", " + equalityClause.getProofStep().getStepNumber() + ", ["
                    + assertion + "]";

        }
        // END-ProofStep
        //
    }
}
