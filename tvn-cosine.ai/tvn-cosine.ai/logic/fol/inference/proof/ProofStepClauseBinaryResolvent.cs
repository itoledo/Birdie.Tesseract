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
    public class ProofStepClauseBinaryResolvent : AbstractProofStep
    {
        private IList<ProofStep> predecessors = new List<ProofStep>();
        private Clause resolvent = null;
        private Literal posLiteral = null;
        private Literal negLiteral = null;
        private Clause parent1, parent2 = null;
        private IDictionary<Variable, Term> subst = new Dictionary<Variable, Term>();
        private IDictionary<Variable, Term> renameSubst = new Dictionary<Variable, Term>();

        public ProofStepClauseBinaryResolvent(Clause resolvent, Literal pl,
                Literal nl, Clause parent1, Clause parent2,
                IDictionary<Variable, Term> subst, IDictionary<Variable, Term> renameSubst)
        {
            this.resolvent = resolvent;
            this.posLiteral = pl;
            this.negLiteral = nl;
            this.parent1 = parent1;
            this.parent2 = parent2;
            foreach (var v in subst)
            {
                this.subst[v.Key] = v.Value;
            }

            foreach (var v in renameSubst)
            {
                this.renameSubst[v.Key] = v.Value;
            }
            this.predecessors.Add(parent1.getProofStep());
            this.predecessors.Add(parent2.getProofStep());
        }

        //
        // START-ProofStep
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            return resolvent.ToString();
        }

        public override string getJustification()
        {
            int lowStep = parent1.getProofStep().getStepNumber();
            int highStep = parent2.getProofStep().getStepNumber();

            if (lowStep > highStep)
            {
                lowStep = highStep;
                highStep = parent1.getProofStep().getStepNumber();
            }

            return "Resolution: " + lowStep + ", " + highStep + "  [" + posLiteral
                    + ", " + negLiteral + "], subst=" + subst.CustomDictionaryWriterToString() + ", renaming="
                    + renameSubst.CustomDictionaryWriterToString();
        }
        // END-ProofStep
        //
    }

}
