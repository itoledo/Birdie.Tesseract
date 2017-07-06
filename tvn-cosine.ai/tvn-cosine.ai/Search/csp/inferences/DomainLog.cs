using System.Collections.Generic;
using System.Text;
using tvn.cosine.ai.util.datastructure;

namespace tvn.cosine.ai.search.csp.inferences
{
    /**
     * Provides information which might be useful for a caller of a constraint
     * propagation algorithm. It maintains old domains for variables and provides
     * means to restore the initial state of the CSP (before domain reduction
     * started). Additionally, a flag indicates whether an empty domain has been
     * found during propagation.
     * 
     * @author Ruediger Lunde
     * 
     */
    public class DomainLog<VAR, VAL> : InferenceLog<VAR, VAL>
     where VAR : Variable
    {
        private List<Pair<VAR, Domain<VAL>>> savedDomains;
        private ISet<VAR> affectedVariables;
        private bool emptyDomainObserved;

        public DomainLog()
        {
            savedDomains = new List<Pair<VAR, Domain<VAL>>>();
            affectedVariables = new HashSet<VAR>();
        }

        public void clear()
        {
            savedDomains.Clear();
            affectedVariables.Clear();
        }

        /**
         * Stores the specified domain for the specified variable if a domain has
         * not yet been stored for the variable.
         */
        public void storeDomainFor(VAR var, Domain<VAL> domain)
        {
            if (!affectedVariables.Contains(var))
            {
                savedDomains.Add(new Pair<VAR, Domain<VAL>>(var, domain));
                affectedVariables.Add(var);
            }
        }

        public void setEmptyDomainFound(bool b)
        {
            emptyDomainObserved = b;
        }

        /**
         * Can be called after all domain information has been collected to reduce
         * storage consumption.
         * 
         * @return this object, after removing one hashtable.
         */
        public DomainLog<VAR, VAL> compactify()
        {
            affectedVariables = null;
            return this;
        }

        public bool isEmpty()
        {
            return savedDomains.Count == 0;
        }

        public void undo(CSP<VAR, VAL> csp)
        {
            foreach (Pair<VAR, Domain<VAL>> pair in getSavedDomains())
                csp.setDomain(pair.First, pair.Second);
        }

        public bool inconsistencyFound()
        {
            ;
            return emptyDomainObserved;
        }

        private List<Pair<VAR, Domain<VAL>>> getSavedDomains()
        {
            return savedDomains;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (Pair<VAR, Domain<VAL>> pair in savedDomains)
                result.Append(pair.First)
                      .Append("=")
                      .Append(pair.Second)
                      .Append(" ");
            if (emptyDomainObserved)
                result.Append("!");

            return result.ToString();
        }
    }

}
