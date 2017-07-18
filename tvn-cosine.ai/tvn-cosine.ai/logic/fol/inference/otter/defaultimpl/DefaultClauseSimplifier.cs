namespace tvn.cosine.ai.logic.fol.inference.otter.defaultimpl
{
    public class DefaultClauseSimplifier : ClauseSimplifier
    {


    private Demodulation demodulation = new Demodulation();
    private IQueue<TermEquality> rewrites = Factory.CreateQueue<TermEquality>();

    public DefaultClauseSimplifier()
    {

    }

    public DefaultClauseSimplifier(IQueue<TermEquality> rewrites)
    {
        this.rewrites.addAll(rewrites);
    }

    //
    // START-ClauseSimplifier
    public Clause simplify(Clause c)
    {
        Clause simplified = c;

        // Apply each of the rewrite rules to
        // the clause
        for (TermEquality te : rewrites)
        {
            Clause dc = simplified;
            // Keep applying the rewrite as many times as it
            // can be applied before moving on to the next one.
            while (null != (dc = demodulation.apply(te, dc)))
            {
                simplified = dc;
            }
        }

        return simplified;
    }

    // END-ClauseSimplifier
    //
}
}
