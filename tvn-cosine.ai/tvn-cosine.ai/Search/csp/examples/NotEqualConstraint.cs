using System;
using System.Collections.Generic;

namespace tvn.cosine.ai.search.csp.examples
{
    /**
     * Represents a binary constraint which forbids equal values.
     * 
     * @author Ruediger Lunde
     */
    public class NotEqualConstraint<VAR, VAL> : Constraint<VAR, VAL>
        where VAR : Variable
    {
        private VAR var1;
        private VAR var2;
        private List<VAR> scope;

        public NotEqualConstraint(VAR var1, VAR var2)
        {
            this.var1 = var1;
            this.var2 = var2;
            scope = new List<VAR>(2);
            scope.Add(var1);
            scope.Add(var2);
        }

        public List<VAR> getScope()
        {
            return scope;
        }

        public bool isSatisfiedWith(Assignment<VAR, VAL> assignment)
        {
            Object value1 = assignment.getValue(var1);
            return value1 == null || !value1.Equals(assignment.getValue(var2));
        }
    }

}
