namespace tvn.cosine.ai.logic.fol
{
    public class SubstVisitor : AbstractFOLVisitor
    { 
        public SubstVisitor()
        {
        }

        /**
         * Note: Refer to Artificial Intelligence A Modern Approach (3rd Edition):
         * page 323.
         * 
         * @param theta
         *            a substitution.
         * @param sentence
         *            the substitution has been applied to.
         * @return a new Sentence representing the result of applying the
         *         substitution theta to aSentence.
         * 
         */
        public Sentence subst(IMap<Variable, Term> theta, Sentence sentence)
        {
            return (Sentence)sentence.accept(this, theta);
        }

        public Term subst(IMap<Variable, Term> theta, Term aTerm)
        {
            return (Term)aTerm.accept(this, theta);
        }

        public Function subst(IMap<Variable, Term> theta, Function function)
        {
            return (Function)function.accept(this, theta);
        }

        public Literal subst(IMap<Variable, Term> theta, Literal literal)
        {
            return literal.newInstance((AtomicSentence)literal
                    .getAtomicSentence().accept(this, theta));
        }


    @SuppressWarnings("unchecked")

     
    public object visitVariable(Variable variable, object arg)
        {
            Map<Variable, Term> substitution = (IMap<Variable, Term>)arg;
            if (substitution.containsKey(variable))
            {
                return substitution.Get(variable).copy();
            }
            return variable.copy();
        }


    @SuppressWarnings("unchecked")

     
    public object visitQuantifiedSentence(QuantifiedSentence sentence,
            object arg)
        {

            Map<Variable, Term> substitution = (IMap<Variable, Term>)arg;

            Sentence quantified = sentence.getQuantified();
            Sentence quantifiedAfterSubs = (Sentence)quantified.accept(this, arg);

            IQueue<Variable> variables = Factory.CreateQueue<Variable>();
            for (Variable v : sentence.getVariables())
            {
                Term st = substitution.Get(v);
                if (null != st)
                {
                    if (st is Variable) {
                // Only if it is a variable to I replace it, otherwise
                // I drop it.
                variables.Add((Variable)st.copy());
            }
        } else {
				// No substitution for the quantified variable, so
				// keep it.
				variables.Add(v.copy());
			}
}

		// If not variables remaining on the quantifier, then drop it
		if (variables.size() == 0) {
			return quantifiedAfterSubs;
		}

		return new QuantifiedSentence(sentence.getQuantifier(), variables,
                quantifiedAfterSubs);
	}
}
}
