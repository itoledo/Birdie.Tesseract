using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol.kb.data;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepFoChAlreadyAFact : AbstractProofStep
    { 
        private static readonly IQueue<ProofStep> _noPredecessors = Factory.CreateQueue<ProofStep>();
        //
        private Literal fact = null;

        public ProofStepFoChAlreadyAFact(Literal fact)
        {
            this.fact = fact;
        }
         
        public override IQueue<ProofStep> getPredecessorSteps()
        {
            return Factory.CreateReadOnlyQueue<ProofStep>(_noPredecessors);
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
