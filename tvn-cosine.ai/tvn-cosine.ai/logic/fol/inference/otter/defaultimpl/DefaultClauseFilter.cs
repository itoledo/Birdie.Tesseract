namespace tvn.cosine.ai.logic.fol.inference.otter.defaultimpl
{
    public class DefaultClauseFilter : ClauseFilter
    {

        public DefaultClauseFilter()
        {

        }

        //
        // START-ClauseFilter
        public Set<Clause> filter(Set<Clause> clauses)
        {
            return clauses;
        }

        // END-ClauseFilter
        //
    }
}
