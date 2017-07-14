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
    public class ProofStepClauseFactor : AbstractProofStep
    {
        private IList<ProofStep> predecessors = new List<ProofStep>();
        private Clause factor = null;
        private Clause factorOf = null;
        private Literal lx = null;
        private Literal ly = null;
        private IDictionary<Variable, Term> subst = new Dictionary<Variable, Term>();
        private IDictionary<Variable, Term> renameSubst = new Dictionary<Variable, Term>();

        public ProofStepClauseFactor(Clause factor, Clause factorOf, Literal lx,
                Literal ly, IDictionary<Variable, Term> subst,
                IDictionary<Variable, Term> renameSubst)
        {
            this.factor = factor;
            this.factorOf = factorOf;
            this.lx = lx;
            this.ly = ly;
            foreach (var v in subst)
                this.subst.Add(v);
            foreach (var v in renameSubst)
                this.renameSubst.Add(v);
            this.predecessors.Add(factorOf.getProofStep());
        }

        //
        // START-ProofStep
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return factor.ToString();
        }

        public override string getJustification()
        {
            return "Factor of " + factorOf.getProofStep().getStepNumber() + "  ["
                    + lx + ", " + ly + "], subst=" + subst.CustomDictionaryWriterToString() + ", renaming="
                    + renameSubst.CustomDictionaryWriterToString();
        }
        // END-ProofStep
        //
    }

}
