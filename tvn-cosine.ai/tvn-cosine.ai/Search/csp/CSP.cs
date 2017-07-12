using System;
using System.Collections.Generic;

namespace tvn.cosine.ai.search.csp
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Ed.): Section 6.1, Page 202. 
     *  
     * A constraint satisfaction problem or CSP consists of three components, X, D,
     * and C:
     * <ul>
     * <li>X is a set of variables, {X1, ... ,Xn}.</li>
     * <li>D is a set of domains, {D1, ... ,Dn}, one for each variable.</li>
     * <li>C is a set of constraints that specify allowable combinations of values.</li>
     * </ul>
     *
     * @param <VAR> Type which is used to represent variables
     * @param <VAL> Type which is used to represent the values in the domains
     *
     * @author Ruediger Lunde
     */
    public class CSP<VAR, VAL>
        where VAR : Variable
    {
        private List<VAR> variables;
        private List<Domain<VAL>> domains;
        private List<Constraint<VAR, VAL>> constraints;

        /**
         * Lookup, which maps a variable to its index in the list of variables.
         */
        private IDictionary<Variable, int> varIndexHash;
        /**
         * Constraint network. Maps variables to those constraints in which they
         * participate.
         */
        private IDictionary<Variable, List<Constraint<VAR, VAL>>> cnet;

        /**
         * Creates a new CSP.
         */
        public CSP()
        {
            variables = new List<VAR>();
            domains = new List<Domain<VAL>>();
            constraints = new List<Constraint<VAR, VAL>>();
            varIndexHash = new Dictionary<Variable, int>();
            cnet = new Dictionary<Variable, List<Constraint<VAR, VAL>>>();
        }

        /**
         * Creates a new CSP.
         */
        public CSP(List<VAR> vars)
            : this()
        {
            foreach (var v in vars)
                addVariable(v);
        }

        /**
         * Adds a new variable only if its name is new.
         */
        protected void addVariable(VAR var)
        {
            if (!varIndexHash.ContainsKey(var))
            {
                Domain<VAL> emptyDomain = new Domain<VAL>(new List<VAL>());
                variables.Add(var);
                domains.Add(emptyDomain);
                varIndexHash.Add(var, variables.Count - 1);
                cnet.Add(var, new List<Constraint<VAR, VAL>>());
            }
            else
            {
                throw new ArgumentException("Variable with same name already exists.");
            }
        }

        public List<VAR> getVariables()
        {
            return variables;
        }

        public int indexOf(Variable var)
        {
            return varIndexHash[var];
        }

        public void setDomain(VAR var, Domain<VAL> domain)
        {
            domains.Insert(indexOf(var), domain);
        }

        public Domain<VAL> getDomain(Variable var)
        {
            return domains[varIndexHash[var]];
        }

        /**
         * Replaces the domain of the specified variable by new domain, which
         * contains all values of the old domain except the specified value.
         */
        public bool removeValueFromDomain(VAR var, VAL value)
        {
            Domain<VAL> currDomain = getDomain(var);
            List<VAL> values = new List<VAL>(currDomain.size());
            foreach (VAL v in currDomain)
                if (!v.Equals(value))
                    values.Add(v);
            if (values.Count < currDomain.size())
            {
                setDomain(var, new Domain<VAL>(values));
                return true;
            }
            return false;
        }

        public void addConstraint(Constraint<VAR, VAL> constraint)
        {
            constraints.Add(constraint);
            foreach (VAR var in constraint.getScope())
                cnet[var].Add(constraint);
        }

        public bool removeConstraint(Constraint<VAR, VAL> constraint)
        {
            bool result = constraints.Remove(constraint);
            if (result)
                foreach (VAR var in constraint.getScope())
                    cnet[var].Remove(constraint);
            return result;
        }

        public List<Constraint<VAR, VAL>> getConstraints()
        {
            return constraints;
        }

        /**
         * Returns all constraints in which the specified variable participates.
         */
        public List<Constraint<VAR, VAL>> getConstraints(Variable var)
        {
            return cnet[var];
        }

        /**
         * Returns for binary constraints the other variable from the scope.
         *
         * @return a variable or null for non-binary constraints.
         */
        public VAR getNeighbor(VAR var, Constraint<VAR, VAL> constraint)
        {
            List<VAR> scope = constraint.getScope();
            if (scope.Count == 2)
            {
                if (var.Equals(scope[0]))
                    return scope[1];
                else if (var.Equals(scope[1]))
                    return scope[0];
            }
            return null;
        }

        /**
         * Returns a copy which contains a copy of the domains list and is in all
         * other aspects a flat copy of this.
         */
        public CSP<VAR, VAL> copyDomains()
        {
            CSP<VAR, VAL> result;

            result = (CSP<VAR, VAL>)MemberwiseClone();
            result.domains = new List<Domain<VAL>>(domains.Count);
            result.domains.AddRange(domains);

            return result;
        }
    }
}
