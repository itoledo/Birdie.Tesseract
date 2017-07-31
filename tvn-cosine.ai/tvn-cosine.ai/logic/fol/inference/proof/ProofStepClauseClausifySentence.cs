using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepClauseClausifySentence : AbstractProofStep
    {
        private ICollection<ProofStep> predecessors = CollectionFactory.CreateQueue<ProofStep>();
        private Clause clausified = null;

        public ProofStepClauseClausifySentence(Clause clausified, Sentence origSentence)
        {
            this.clausified = clausified;
            this.predecessors.Add(new ProofStepPremise(origSentence));
        }

        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(predecessors);
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
