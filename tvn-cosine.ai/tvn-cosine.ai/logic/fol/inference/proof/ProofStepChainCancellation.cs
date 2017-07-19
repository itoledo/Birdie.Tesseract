﻿using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepChainCancellation : AbstractProofStep
    { 
        private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
        private Chain cancellation = null;
        private Chain cancellationOf = null;
        private IMap<Variable, Term> subst = null;

        public ProofStepChainCancellation(Chain cancellation, Chain cancellationOf, IMap<Variable, Term> subst)
        {
            this.cancellation = cancellation;
            this.cancellationOf = cancellationOf;
            this.subst = subst;
            this.predecessors.Add(cancellationOf.getProofStep());
        }
         
        public override IQueue<ProofStep> getPredecessorSteps()
        {
            return Factory.CreateReadOnlyQueue<ProofStep>(predecessors);
        }
         
        public override string getProof()
        {
            return cancellation.ToString();
        }
         
        public override string getJustification()
        {
            return "Cancellation: " + cancellationOf.getProofStep().getStepNumber() + " " + subst;
        } 
    } 
}
