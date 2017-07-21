using System.Text;
using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepBwChGoal : AbstractProofStep
    { 
        private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
        //
        private Clause toProve = null;
        private Literal currentGoal = null;
        private IMap<Variable, Term> bindings = Factory.CreateInsertionOrderedMap<Variable, Term>();

        public ProofStepBwChGoal(Clause toProve, Literal currentGoal, IMap<Variable, Term> bindings)
        {
            this.toProve = toProve;
            this.currentGoal = currentGoal;
            this.bindings.PutAll(bindings);
        }

        public IMap<Variable, Term> getBindings()
        {
            return bindings;
        }

        public void setPredecessor(ProofStep predecessor)
        {
            predecessors.Clear();
            predecessors.Add(predecessor);
        }

        public override IQueue<ProofStep> getPredecessorSteps()
        {
            return Factory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }
         
        public override string getProof()
        {
            StringBuilder sb = new StringBuilder();
            IQueue<Literal> nLits = toProve.getNegativeLiterals();
            for (int i = 0; i < toProve.getNumberNegativeLiterals();++i)
            {
                sb.Append(nLits.Get(i).getAtomicSentence());
                if (i != (toProve.getNumberNegativeLiterals() - 1))
                {
                    sb.Append(" AND ");
                }
            }
            if (toProve.getNumberNegativeLiterals() > 0)
            {
                sb.Append(" => ");
            }
            sb.Append(toProve.getPositiveLiterals().Get(0));
            return sb.ToString();
        }
         
        public override string getJustification()
        {
            return "Current Goal " + currentGoal.getAtomicSentence().ToString() + ", " + bindings;
        }
    }
}
