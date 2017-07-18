namespace tvn.cosine.ai.logic.fol.inference.proof
{
    public class ProofStepFoChAssertFact : AbstractProofStep
    {
    //
    private IQueue<ProofStep> predecessors = Factory.CreateQueue<ProofStep>();
    //
    private Clause implication = null;
    private Literal fact = null;
    private Map<Variable, Term> bindings = null;

    public ProofStepFoChAssertFact(Clause implication, Literal fact,
            Map<Variable, Term> bindings, ProofStep predecessor)
    {
        this.implication = implication;
        this.fact = fact;
        this.bindings = bindings;
        if (null != predecessor)
        {
            predecessors.Add(predecessor);
        }
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
        IQueue<Literal> nLits = implication.getNegativeLiterals();
        for (int i = 0; i < implication.getNumberNegativeLiterals(); i++)
        {
            sb.Append(nLits.Get(i).getAtomicSentence());
            if (i != (implication.getNumberNegativeLiterals() - 1))
            {
                sb.Append(" AND ");
            }
        }
        sb.Append(" => ");
        sb.Append(implication.getPositiveLiterals().Get(0));
        return sb.ToString();
    }

     
    public string getJustification()
    {
        return "Assert fact " + fact.ToString() + ", " + bindings;
    }
    // END-ProofStep
    //
}
}
