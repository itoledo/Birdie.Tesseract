namespace tvn.cosine.ai.logic.fol.kb.data
{
    /**
     * Conjunctive Normal Form (CNF) : a conjunction of clauses, where each clause
     * is a disjunction of literals.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class CNF
    {

        private IQueue<Clause> conjunctionOfClauses = Factory.CreateQueue<Clause>();

        public CNF(IQueue<Clause> conjunctionOfClauses)
        {
            this.conjunctionOfClauses.addAll(conjunctionOfClauses);
        }

        public int getNumberOfClauses()
        {
            return conjunctionOfClauses.size();
        }

        public IQueue<Clause> getConjunctionOfClauses()
        {
            return Factory.CreateReadOnlyQueue<>(conjunctionOfClauses);
        }

         
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < conjunctionOfClauses.size(); i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(conjunctionOfClauses.Get(i).ToString());
            }

            return sb.ToString();
        }
    }
}
