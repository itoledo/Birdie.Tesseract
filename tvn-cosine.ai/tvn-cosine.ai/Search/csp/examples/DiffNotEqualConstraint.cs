using System;
using System.Collections.Generic;

namespace tvn.cosine.ai.search.csp.examples
{
    /**
     * Represents a binary constraint which forbids equal values.
     * 
     * @author Ruediger Lunde
     */
    public class DiffNotEqualConstraint<VAR> : Constraint<VAR, int>
        where VAR : Variable
    {
        private VAR var1;
        private VAR var2;
        private int diff;
        private List<VAR> scope;

        public DiffNotEqualConstraint(VAR var1, VAR var2, int diff)
        {
            this.var1 = var1;
            this.var2 = var2;
            this.diff = diff;
            scope = new List<VAR>(2);
            scope.Add(var1);
            scope.Add(var2);
        }

        public List<VAR> getScope()
        {
            return scope;
        }

        public bool isSatisfiedWith(Assignment<VAR, int> assignment)
        {
            int value1 = assignment.getValue(var1);
            int value2 = assignment.getValue(var2);
            return (Math.Abs(value1 - value2) != diff);
        }
    }
}
