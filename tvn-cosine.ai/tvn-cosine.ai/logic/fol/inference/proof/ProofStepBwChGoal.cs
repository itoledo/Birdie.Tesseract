namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepBwChGoal : AbstractProofStep
    {
    //
    private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
    //
    private Clause toProve = null;
    private Literal currentGoal = null;
    private Map<Variable, Term> bindings = Factory.CreateMap<Variable, Term>();

    public ProofStepBwChGoal(Clause toProve, Literal currentGoal,
            Map<Variable, Term> bindings)
    {
        this.toProve = toProve;
        this.currentGoal = currentGoal;
        this.bindings.putAll(bindings);
    }

    public Map<Variable, Term> getBindings()
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
     
    public IQueue<ProofStep> getPredecessorSteps()
    {
        return Factory.CreateReadOnlyQueue<>(predecessors);
    }

     
    public string getProof()
    {
        StringBuilder sb = new StringBuilder();
        IQueue<Literal> nLits = toProve.getNegativeLiterals();
        for (int i = 0; i < toProve.getNumberNegativeLiterals(); i++)
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

     
    public string getJustification()
    {
        return "Current Goal " + currentGoal.getAtomicSentence().ToString()
                + ", " + bindings;
    }
    // END-ProofStep
    //
}
}
