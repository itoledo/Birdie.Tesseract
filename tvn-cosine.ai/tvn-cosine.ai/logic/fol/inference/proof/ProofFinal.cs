namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofFinal : Proof
    {

    private Map<Variable, Term> answerBindings = Factory.CreateMap<Variable, Term>();
    private ProofStep finalStep = null;
    private IQueue<ProofStep> proofSteps = null;

    public ProofFinal(ProofStep finalStep, Map<Variable, Term> answerBindings)
    {
        this.finalStep = finalStep;
        this.answerBindings.putAll(answerBindings);
    }

    //
    // START-Proof
    public IQueue<ProofStep> getSteps()
    {
        // Only calculate if the proof steps are actually requested.
        if (null == proofSteps)
        {
            calcualteProofSteps();
        }
        return proofSteps;
    }

    public Map<Variable, Term> getAnswerBindings()
    {
        return answerBindings;
    }

    public void replaceAnswerBindings(IMap<Variable, Term> updatedBindings)
    {
        answerBindings.Clear();
        answerBindings.putAll(updatedBindings);
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
        proofSteps = Factory.CreateQueue<ProofStep>();
        addToProofSteps(finalStep);

        // Move all premises to the front of the
        // list of steps
        int to = 0;
        for (int i = 0; i < proofSteps.size(); i++)
        {
            if (proofSteps.Get(i) is ProofStepPremise) {
            ProofStep m = proofSteps.Remove(i);
            proofSteps.Add(to, m);
            to++;
        }
    }

		// Move the Goals after the premises
		for (int i = 0; i<proofSteps.size(); i++) {
			if (proofSteps.Get(i) is ProofStepGoal) {
				ProofStep m = proofSteps.Remove(i);
    proofSteps.Add(to, m);
				to++;
			}
		}

		// Assign the step #s now that all the proof
		// steps have been unwound
		for (int i = 0; i<proofSteps.size(); i++) {
			proofSteps.Get(i).setStepNumber(i + 1);
		}
	}

	private void addToProofSteps(ProofStep step)
{
    if (!proofSteps.contains(step))
    {
        proofSteps.Add(0, step);
    }
    else
    {
        proofSteps.Remove(step);
        proofSteps.Add(0, step);
    }
    IQueue<ProofStep> predecessors = step.getPredecessorSteps();
    for (int i = predecessors.size() - 1; i >= 0; i--)
    {
        addToProofSteps(predecessors.Get(i));
    }
}
}
}
