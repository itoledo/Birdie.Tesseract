using System.Collections.Generic;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol
{
    /**
  * @author Ravi Mohan
  * @author Ciaran O'Reilly
  */
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
        public Sentence subst(IDictionary<Variable, Term> theta, Sentence sentence)
        {
            return (Sentence)sentence.accept(this, theta);
        }

        public Term subst(IDictionary<Variable, Term> theta, Term aTerm)
        {
            return (Term)aTerm.accept(this, theta);
        }

        public Function subst(IDictionary<Variable, Term> theta, Function function)
        {
            return (Function)function.accept(this, theta);
        }

        public Literal subst(IDictionary<Variable, Term> theta, Literal literal)
        {
            return literal.newInstance((AtomicSentence)literal.getAtomicSentence().accept(this, theta));
        }

        public override object visitVariable(Variable variable, object arg)
        {
            IDictionary<Variable, Term> substitution = (IDictionary<Variable, Term>)arg;
            if (substitution.ContainsKey(variable))
            {
                return substitution[variable].copy();
            }
            return variable.copy();
        }

        public override object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
        {
            IDictionary<Variable, Term> substitution = (IDictionary<Variable, Term>)arg;

            Sentence quantified = sentence.getQuantified();
            Sentence quantifiedAfterSubs = (Sentence)quantified.accept(this, arg);

            IList<Variable> variables = new List<Variable>();
            foreach (Variable v in sentence.getVariables())
            {
                if (substitution.ContainsKey(v))
                {
                    if (substitution[v] is Variable)
                    {
                        // Only if it is a variable to I replace it, otherwise
                        // I drop it.
                        variables.Add((Variable)substitution[v].copy());
                    }
                }
                else
                {
                    // No substitution for the quantified variable, so keep it.
                    variables.Add(v.copy());
                }
            }

            // If not variables remaining on the quantifier, then drop it
            if (variables.Count == 0)
            {
                return quantifiedAfterSubs;
            }

            return new QuantifiedSentence(sentence.getQuantifier(), variables, quantifiedAfterSubs);
        }
    }
}
