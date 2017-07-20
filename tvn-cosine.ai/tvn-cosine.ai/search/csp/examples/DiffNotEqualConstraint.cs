using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.search.csp.examples
{
    /**
     * Represents a binary constraint which forbids equal values. 
     */
    public class DiffNotEqualConstraint : Constraint<Variable, int>
    { 
        private Variable var1;
        private Variable var2;
        private int diff;
        private IQueue<Variable> scope;

        public DiffNotEqualConstraint(Variable var1, Variable var2, int diff)
        {
            this.var1 = var1;
            this.var2 = var2;
            this.diff = diff;
            scope = Factory.CreateQueue<Variable>();
            scope.Add(var1);
            scope.Add(var2);
        }


        public IQueue<Variable> getScope()
        {
            return scope;
        }


        public bool isSatisfiedWith(Assignment<Variable, int> assignment)
        {
            int value1 = assignment.getValue(var1);
            int value2 = assignment.getValue(var2);
            return (System.Math.Abs(value1 - value2) != diff);
        }
    } 
}
