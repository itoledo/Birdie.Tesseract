using System.Collections.Generic;
using System.Collections.ObjectModel;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepFoChAlreadyAFact : AbstractProofStep
    {
        //
        private static readonly IList<ProofStep> _noPredecessors = new List<ProofStep>();
        //
        private Literal fact = null;

        public ProofStepFoChAlreadyAFact(Literal fact)
        {
            this.fact = fact;
        }

        //
        // START-ProofStep 
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(_noPredecessors);
        }

        public override string getProof()
        {
            return fact.ToString();
        }

        public override string getJustification()
        {
            return "Already a known fact in the KB.";
        }
        // END-ProofStep
        //
    } 
}
