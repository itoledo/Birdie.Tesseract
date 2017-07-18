namespace tvn.cosine.ai.logic.fol
{
    public class StandardizeApartInPlace
    {
        //
        private static CollectAllVariables _collectAllVariables = new CollectAllVariables();

        public static int standardizeApart(Chain c, int saIdx)
        {
            IQueue<Variable> variables = Factory.CreateQueue<Variable>();
            for (Literal l : c.getLiterals())
            {
                collectAllVariables(l.getAtomicSentence(), variables);
            }

            return standardizeApart(variables, c, saIdx);
        }

        public static int standardizeApart(Clause c, int saIdx)
        {
            IQueue<Variable> variables = Factory.CreateQueue<Variable>();
            for (Literal l : c.getLiterals())
            {
                collectAllVariables(l.getAtomicSentence(), variables);
            }

            return standardizeApart(variables, c, saIdx);
        }

        //
        // PRIVATE METHODS
        //
        private static int standardizeApart(IQueue<Variable> variables, object expr,
                int saIdx)
        {
            Map<string, int> indexicals = Factory.CreateMap<string, int>();
            for (Variable v : variables)
            {
                if (!indexicals.containsKey(v.getIndexedValue()))
                {
                    indexicals.Put(v.getIndexedValue(), saIdx++);
                }
            }
            for (Variable v : variables)
            {
                int i = indexicals.Get(v.getIndexedValue());
                if (null == i)
                {
                    throw new RuntimeException("ERROR: duplicate var=" + v
                            + ", expr=" + expr);
                }
                else
                {
                    v.setIndexical(i);
                }
            }

            return saIdx;
        }

        private static void collectAllVariables(Sentence s, IQueue<Variable> vars)
        {
            s.accept(_collectAllVariables, vars);
        }
    }

    class CollectAllVariables : FOLVisitor
    {

    public CollectAllVariables()
    {

    }


    @SuppressWarnings("unchecked")

    public object visitVariable(Variable var, object arg)
    {
        IQueue<Variable> variables = (IQueue<Variable>)arg;
        variables.Add(var);
        return var;
    }


    @SuppressWarnings("unchecked")

    public object visitQuantifiedSentence(QuantifiedSentence sentence,
            object arg)
    {
        // Ensure I collect quantified variables too
        IQueue<Variable> variables = (IQueue<Variable>)arg;
        variables.addAll(sentence.getVariables());

        sentence.getQuantified().accept(this, arg);

        return sentence;
    }

    public object visitPredicate(Predicate predicate, object arg)
    {
        for (Term t : predicate.getTerms())
        {
            t.accept(this, arg);
        }
        return predicate;
    }

    public object visitTermEquality(TermEquality equality, object arg)
    {
        equality.getTerm1().accept(this, arg);
        equality.getTerm2().accept(this, arg);
        return equality;
    }

    public object visitConstant(Constant constant, object arg)
    {
        return constant;
    }

    public object visitFunction(Function function, object arg)
    {
        for (Term t : function.getTerms())
        {
            t.accept(this, arg);
        }
        return function;
    }

    public object visitNotSentence(NotSentence sentence, object arg)
    {
        sentence.getNegated().accept(this, arg);
        return sentence;
    }

    public object visitConnectedSentence(ConnectedSentence sentence, object arg)
    {
        sentence.getFirst().accept(this, arg);
        sentence.getSecond().accept(this, arg);
        return sentence;
    }
}
}
