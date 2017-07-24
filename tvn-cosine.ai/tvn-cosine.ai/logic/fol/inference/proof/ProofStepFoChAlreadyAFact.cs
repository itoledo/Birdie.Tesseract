using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.common.collections.api;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepFoChAlreadyAFact : AbstractProofStep
    { 
        private static readonly ICollection<ProofStep> _noPredecessors = CollectionFactory.CreateQueue<ProofStep>(); 
        private Literal fact = null;

        public ProofStepFoChAlreadyAFact(Literal fact)
        {
            this.fact = fact;
        }
         
        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(_noPredecessors);
        }
         
        public override string getProof()
        {
            return fact.ToString();
        }
         
        public override string getJustification()
        {
            return "Already a known fact in the KB.";
        } 
    }
}
