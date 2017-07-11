using System.Collections.Generic;
using System.Collections.ObjectModel;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepChainFromClause : AbstractProofStep
    {
        private IList<ProofStep> predecessors = new List<ProofStep>();
        private Chain chain = null;
        private Clause fromClause = null;

        public ProofStepChainFromClause(Chain chain, Clause fromClause)
        {
            this.chain = chain;
            this.fromClause = fromClause;
            this.predecessors.Add(fromClause.getProofStep());
        }

        //
        // START-ProofStep 
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return chain.ToString();
        }

        public override string getJustification()
        {
            return "Chain from Clause: " + fromClause.getProofStep().getStepNumber();
        }
        // END-ProofStep
        //
    }

}
