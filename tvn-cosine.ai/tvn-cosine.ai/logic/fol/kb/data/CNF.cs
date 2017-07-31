using tvn.cosine;
using tvn.cosine.collections;
using tvn.cosine.collections.api;
using tvn.cosine.text;
using tvn.cosine.text.api;

namespace tvn.cosine.ai.logic.fol.kb.data
{
    /// <summary>
    /// Conjunctive Normal Form (CNF) : a conjunction of clauses, where each clause
    /// is a disjunction of literals.
    /// </summary>
    public class CNF
    {
        private ICollection<Clause> conjunctionOfClauses = CollectionFactory.CreateQueue<Clause>();

        public CNF(ICollection<Clause> conjunctionOfClauses)
        {
            this.conjunctionOfClauses.AddAll(conjunctionOfClauses);
        }

        public int getNumberOfClauses()
        {
            return conjunctionOfClauses.Size();
        }

        public ICollection<Clause> getConjunctionOfClauses()
        {
            return CollectionFactory.CreateReadOnlyQueue<Clause>(conjunctionOfClauses);
        }
         
        public override string ToString()
        {
            IStringBuilder sb = TextFactory.CreateStringBuilder();
            for (int i = 0; i < conjunctionOfClauses.Size();++i)
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
