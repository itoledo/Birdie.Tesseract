using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofStepBwChGoal : AbstractProofStep
    {
        //
        private IList<ProofStep> predecessors = new List<ProofStep>();
        //
        private Clause toProve = null;
        private Literal currentGoal = null;
        private IDictionary<Variable, Term> bindings = new Dictionary<Variable, Term>();

        public ProofStepBwChGoal(Clause toProve, Literal currentGoal, IDictionary<Variable, Term> bindings)
        {
            this.toProve = toProve;
            this.currentGoal = currentGoal;
            foreach (var v in bindings)
                this.bindings.Add(v);
        }

        public IDictionary<Variable, Term> getBindings()
        {
            return bindings;
        }

        public void setPredecessor(ProofStep predecessor)
        {
            predecessors.Clear();
            predecessors.Add(predecessor);
        }

        //
        // START-ProofStep 
        public override IList<ProofStep> getPredecessorSteps()
        {
            return new ReadOnlyCollection<ProofStep>(predecessors);
        }

        public override string getProof()
        {
            StringBuilder sb = new StringBuilder();
            IList<Literal> nLits = toProve.getNegativeLiterals();
            for (int i = 0; i < toProve.getNumberNegativeLiterals(); i++)
            {
                sb.Append(nLits[i].getAtomicSentence());
                if (i != (toProve.getNumberNegativeLiterals() - 1))
                {
                    sb.Append(" AND ");
                }
            }
            if (toProve.getNumberNegativeLiterals() > 0)
            {
                sb.Append(" => ");
            }
            sb.Append(toProve.getPositiveLiterals()[0]);
            return sb.ToString();
        }

        public override string getJustification()
        {
            return "Current Goal " + currentGoal.getAtomicSentence().ToString() + ", " + bindings.CustomDictionaryWriterToString();
        }
        // END-ProofStep
        //
    } 
}
