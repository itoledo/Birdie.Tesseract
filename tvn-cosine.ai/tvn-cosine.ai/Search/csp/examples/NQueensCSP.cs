using System.Collections.Generic;

namespace tvn.cosine.ai.search.csp.examples
{
    public class NQueensCSP : CSP<Variable, int>
    {
        public NQueensCSP(int size)
        {
            for (int i = 0; i < size; i++)
                addVariable(new Variable("Q" + (i + 1)));

            List<int> values = new List<int>();
            for (int val = 1; val <= size; val++)
                values.Add(val);
            Domain<int> positions = new Domain<int>(values);

            foreach (Variable var in getVariables())
                setDomain(var, positions);

            for (int i = 0; i < size; i++)
            {
                Variable var1 = getVariables()[i];
                for (int j = i + 1; j < size; j++)
                {
                    Variable var2 = getVariables()[j];
                    addConstraint(new DiffNotEqualConstraint<Variable>(var1, var2, 0));
                    addConstraint(new DiffNotEqualConstraint<Variable>(var1, var2, j - i));
                }
            }
        }
    }
}
