using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepClauseClausifySentence : AbstractProofStep
    {
        private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
        private Clause clausified = null;

        public ProofStepClauseClausifySentence(Clause clausified, Sentence origSentence)
        {
            this.clausified = clausified;
            this.predecessors.Add(new ProofStepPremise(origSentence));
        }

        public override IQueue<ProofStep> getPredecessorSteps()
        {
            return Factory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return clausified.ToString();
        }

        public override string getJustification()
        {
            return "Clausified " + predecessors.Get(0).getStepNumber();
        }
    }
}
