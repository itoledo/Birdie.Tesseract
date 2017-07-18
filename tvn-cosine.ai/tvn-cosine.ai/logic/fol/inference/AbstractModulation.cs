using tvn.cosine.ai.common.collections;
using tvn.cosine.ai.logic.fol.parsing;

namespace tvn.cosine.ai.logic.fol.inference
{
    /**
     * Abstract base class for Demodulation and Paramodulation algorithms.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public abstract class AbstractModulation
    {
        //
        // PROTECTED ATTRIBUTES
        protected VariableCollector variableCollector = new VariableCollector();
        protected Unifier unifier = new Unifier();
        protected SubstVisitor substVisitor = new SubstVisitor();

        //
        // PROTECTED METODS
        //
        protected abstract bool isValidMatch(Term toMatch,
                ISet<Variable> toMatchVariables, Term possibleMatch,
                Map<Variable, Term> substitution);

        protected IdentifyCandidateMatchingTerm getMatchingSubstitution(
                Term toMatch, AtomicSentence expression)
        {

            IdentifyCandidateMatchingTerm icm = new IdentifyCandidateMatchingTerm(
                    toMatch, expression);

            if (icm.isMatch())
            {
                return icm;
            }

            // indicates no match
            return null;
        }

        protected class IdentifyCandidateMatchingTerm : FOLVisitor
        { 
            private Term toMatch = null;
            private ISet<Variable> toMatchVariables = null;
            private Term matchingTerm = null;
            private Map<Variable, Term> substitution = null;

            public IdentifyCandidateMatchingTerm(Term toMatch,
                    AtomicSentence expression)
            {
                this.toMatch = toMatch;
                this.toMatchVariables = variableCollector
                        .collectAllVariables(toMatch);

                expression.accept(this, null);
            }

            public bool isMatch()
            {
                return null != matchingTerm;
            }

            public Term getMatchingTerm()
            {
                return matchingTerm;
            }

            public Map<Variable, Term> getMatchingSubstitution()
            {
                return substitution;
            }

            //
            // START-FOLVisitor
            public object visitPredicate(Predicate p, object arg)
            {
                for (Term t : p.getArgs())
                {
                    // Finish processing if have found a match
                    if (null != matchingTerm)
                    {
                        break;
                    }
                    t.accept(this, null);
                }
                return p;
            }

            public object visitTermEquality(TermEquality equality, object arg)
            {
                for (Term t : equality.getArgs())
                {
                    // Finish processing if have found a match
                    if (null != matchingTerm)
                    {
                        break;
                    }
                    t.accept(this, null);
                }
                return equality;
            }

            public object visitVariable(Variable variable, object arg)
            {

                if (null != (substitution = unifier.unify(toMatch, variable)))
                {
                    if (isValidMatch(toMatch, toMatchVariables, variable,
                            substitution))
                    {
                        matchingTerm = variable;
                    }
                }

                return variable;
            }

            public object visitConstant(Constant constant, object arg)
            {
                if (null != (substitution = unifier.unify(toMatch, constant)))
                {
                    if (isValidMatch(toMatch, toMatchVariables, constant,
                            substitution))
                    {
                        matchingTerm = constant;
                    }
                }

                return constant;
            }

            public object visitFunction(Function function, object arg)
            {
                if (null != (substitution = unifier.unify(toMatch, function)))
                {
                    if (isValidMatch(toMatch, toMatchVariables, function,
                            substitution))
                    {
                        matchingTerm = function;
                    }
                }

                if (null == matchingTerm)
                {
                    // Try the Function's arguments
                    for (Term t : function.getArgs())
                    {
                        // Finish processing if have found a match
                        if (null != matchingTerm)
                        {
                            break;
                        }
                        t.accept(this, null);
                    }
                }

                return function;
            }

            public object visitNotSentence(NotSentence sentence, object arg)
            {
                throw new IllegalStateException(
                        "visitNotSentence() should not be called.");
            }

            public object visitConnectedSentence(ConnectedSentence sentence,
                    object arg)
            {
                throw new IllegalStateException(
                        "visitConnectedSentence() should not be called.");
            }

            public object visitQuantifiedSentence(QuantifiedSentence sentence,
                    object arg)
            {
                throw new IllegalStateException(
                        "visitQuantifiedSentence() should not be called.");
            }

            // END-FOLVisitor
            //
        }

        protected class ReplaceMatchingTerm : FOLVisitor
        {

        private Term toReplace = null;
        private Term replaceWith = null;
        private bool replaced = false;

        public ReplaceMatchingTerm()
        {
        }

        public AtomicSentence replace(AtomicSentence expression,
                Term toReplace, Term replaceWith)
        {
            this.toReplace = toReplace;
            this.replaceWith = replaceWith;

            return (AtomicSentence)expression.accept(this, null);
        }

        //
        // START-FOLVisitor
        public object visitPredicate(Predicate p, object arg)
        {
            IQueue<Term> newTerms = Factory.CreateQueue<Term>();
            for (Term t : p.getTerms())
            {
                Term subsTerm = (Term)t.accept(this, arg);
                newTerms.Add(subsTerm);
            }
            return new Predicate(p.getPredicateName(), newTerms);
        }

        public object visitTermEquality(TermEquality equality, object arg)
        {
            Term newTerm1 = (Term)equality.getTerm1().accept(this, arg);
            Term newTerm2 = (Term)equality.getTerm2().accept(this, arg);
            return new TermEquality(newTerm1, newTerm2);
        }

        public object visitVariable(Variable variable, object arg)
        {
            if (!replaced)
            {
                if (toReplace.Equals(variable))
                {
                    replaced = true;
                    return replaceWith;
                }
            }
            return variable;
        }

        public object visitConstant(Constant constant, object arg)
        {
            if (!replaced)
            {
                if (toReplace.Equals(constant))
                {
                    replaced = true;
                    return replaceWith;
                }
            }
            return constant;
        }

        public object visitFunction(Function function, object arg)
        {
            if (!replaced)
            {
                if (toReplace.Equals(function))
                {
                    replaced = true;
                    return replaceWith;
                }
            }

            IQueue<Term> newTerms = Factory.CreateQueue<Term>();
            for (Term t : function.getTerms())
            {
                Term subsTerm = (Term)t.accept(this, arg);
                newTerms.Add(subsTerm);
            }
            return new Function(function.getFunctionName(), newTerms);
        }

        public object visitNotSentence(NotSentence sentence, object arg)
        {
            throw new IllegalStateException(
                    "visitNotSentence() should not be called.");
        }

        public object visitConnectedSentence(ConnectedSentence sentence,
                object arg)
        {
            throw new IllegalStateException(
                    "visitConnectedSentence() should not be called.");
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence,
                object arg)
        {
            throw new IllegalStateException(
                    "visitQuantifiedSentence() should not be called.");
        }

        // END-FOLVisitor
        //
    }
}
}
