using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepClauseFactor : AbstractProofStep
    { 
        private ICollection<ProofStep> predecessors = CollectionFactory.CreateQueue<ProofStep>();
        private Clause factor = null;
        private Clause factorOf = null;
        private Literal lx = null;
        private Literal ly = null;
        private IMap<Variable, Term> subst = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();
        private IMap<Variable, Term> renameSubst = CollectionFactory.CreateInsertionOrderedMap<Variable, Term>();

        public ProofStepClauseFactor(Clause factor, Clause factorOf, Literal lx,
                Literal ly, IMap<Variable, Term> subst,
                IMap<Variable, Term> renameSubst)
        {
            this.factor = factor;
            this.factorOf = factorOf;
            this.lx = lx;
            this.ly = ly;
            this.subst.PutAll(subst);
            this.renameSubst.PutAll(renameSubst);
            this.predecessors.Add(factorOf.getProofStep());
        }
         
        public override ICollection<ProofStep> getPredecessorSteps()
        {
            return CollectionFactory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return factor.ToString();
        }

        public override string getJustification()
        {
            return "Factor of " + factorOf.getProofStep().getStepNumber() + "  ["
                    + lx + ", " + ly + "], subst=" + subst + ", renaming="
                    + renameSubst;
        } 
    }
}
