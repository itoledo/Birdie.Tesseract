using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private List<Clause> conjunctionOfClauses = new List<Clause>();

        public CNF(IList<Clause> conjunctionOfClauses)
        {
            this.conjunctionOfClauses.AddRange(conjunctionOfClauses);
        }

        public int getNumberOfClauses()
        {
            return conjunctionOfClauses.Count;
        }

        public IList<Clause> getConjunctionOfClauses()
        {
            return new ReadOnlyCollection<Clause>(conjunctionOfClauses);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < conjunctionOfClauses.Count; i++)
            {
                if (i > 0)
                {
                    sb.Append(",");
                }
                sb.Append(conjunctionOfClauses[i].ToString());
            }

            return sb.ToString();
        }
    }

}
