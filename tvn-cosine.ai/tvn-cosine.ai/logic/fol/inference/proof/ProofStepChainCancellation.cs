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
    public class ProofStepChainCancellation : AbstractProofStep
    {

        private IList<ProofStep> predecessors = new List<ProofStep>();
        private Chain cancellation = null;
        private Chain cancellationOf = null;
        private IDictionary<Variable, Term> subst = null;

        public ProofStepChainCancellation(Chain cancellation, Chain cancellationOf, IDictionary<Variable, Term> subst)
        {
            this.cancellation = cancellation;
            this.cancellationOf = cancellationOf;
            this.subst = subst;
            this.predecessors.Add(cancellationOf.getProofStep());
        }

        //
        // START-ProofStep 
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return cancellation.ToString();
        }

        public override string getJustification()
        {
            return "Cancellation: " + cancellationOf.getProofStep().getStepNumber() + " " + subst;
        }
        // END-ProofStep
        //
    } 
}
