using System.Collections.Generic;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference.proof
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class ProofFinal : Proof
    {
        private IDictionary<Variable, Term> answerBindings = new Dictionary<Variable, Term>();
        private ProofStep finalStep = null;
        private IList<ProofStep> proofSteps = null;

        public ProofFinal(ProofStep finalStep, IDictionary<Variable, Term> answerBindings)
        {
            this.finalStep = finalStep;
            foreach (var v in answerBindings)
                this.answerBindings.Add(v);
        }

        //
        // START-Proof
        public IList<ProofStep> getSteps()
        {
            // Only calculate if the proof steps are actually requested.
            if (null == proofSteps)
            {
                calcualteProofSteps();
            }
            return proofSteps;
        }

        public IDictionary<Variable, Term> getAnswerBindings()
        {
            return answerBindings;
        }

        public void replaceAnswerBindings(IDictionary<Variable, Term> updatedBindings)
        {
            answerBindings.Clear();
            foreach (var v in updatedBindings)
                answerBindings.Add(v);
        }

        // END-Proof
        //

        public override string ToString()
        {
            return answerBindings.ToString();
        }

        //
        // PRIVATE METHODS
        //
        private void calcualteProofSteps()
        {
            proofSteps = new List<ProofStep>();
            addToProofSteps(finalStep);

            // Move all premises to the front of the
            // list of steps
            int to = 0;
            for (int i = 0; i < proofSteps.Count; ++i)
            {
                if (proofSteps[i] is ProofStepPremise)
                {
                    ProofStep m = proofSteps[i];
                    proofSteps.RemoveAt(i);
                    proofSteps.Insert(to, m);
                    to++;
                }
            }

            // Move the Goals after the premises
            for (int i = 0; i < proofSteps.Count; ++i)
            {
                if (proofSteps[i] is ProofStepGoal)
                {

                    ProofStep m = proofSteps[i];
                    proofSteps.RemoveAt(i);
                    proofSteps.Insert(to, m);
                    to++;
                }
            }

            // Assign the step #s now that all the proof
            // steps have been unwound
            for (int i = 0; i < proofSteps.Count; ++i)
            {
                proofSteps[i].setStepNumber(i + 1);
            }
        }

        private void addToProofSteps(ProofStep step)
        {
            if (!proofSteps.Contains(step))
            {
                proofSteps.Insert(0, step);
            }
            else
            {
                proofSteps.Remove(step);
                proofSteps.Insert(0, step);
            }
            IList<ProofStep> predecessors = step.getPredecessorSteps();
            for (int i = predecessors.Count - 1; i >= 0; i--)
            {
                addToProofSteps(predecessors[i]);
            }
        }
    } 
}
