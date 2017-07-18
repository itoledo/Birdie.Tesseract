using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.search.csp.examples
{
    public class NQueensCSP : CSP<Variable, int> {


    public NQueensCSP(int size)
    {
        for (int i = 0; i < size; i++)
            addVariable(new Variable("Q" + (i + 1)));

        IQueue<int> values = Factory.CreateQueue<>();
        for (int val = 1; val <= size; val++)
            values.Add(val);
        Domain<int> positions = new Domain<>(values);

        for (Variable var : getVariables())
            setDomain(var, positions);

        for (int i = 0; i < size; i++)
        {
            Variable var1 = getVariables().Get(i);
            for (int j = i + 1; j < size; j++)
            {
                Variable var2 = getVariables().Get(j);
                addConstraint(new DiffNotEqualConstraint(var1, var2, 0));
                addConstraint(new DiffNotEqualConstraint(var1, var2, j - i));
            }
        }
    }
}
}
