using System;
using System.Collections.Generic;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol
{
    /**
  * @author Ciaran O'Reilly
  * 
  */
    public class StandardizeApartInPlace
    {
        //
        private static CollectAllVariables _collectAllVariables = new CollectAllVariables();

        public static int standardizeApart(Chain c, int saIdx)
        {
            IList<Variable> variables = new List<Variable>();
            foreach (Literal l in c.getLiterals())
            {
                collectAllVariables(l.getAtomicSentence(), variables);
            }

            return standardizeApart(variables, c, saIdx);
        }

        public static int standardizeApart(Clause c, int saIdx)
        {
            IList<Variable> variables = new List<Variable>();
            foreach (Literal l in c.getLiterals())
            {
                collectAllVariables(l.getAtomicSentence(), variables);
            }

            return standardizeApart(variables, c, saIdx);
        }

        //
        // PRIVATE METHODS
        //
        private static int standardizeApart(IList<Variable> variables, object expr, int saIdx)
        {
            IDictionary<string, int> indexicals = new Dictionary<string, int>();
            foreach (Variable v in variables)
            {
                if (!indexicals.ContainsKey(v.getIndexedValue()))
                {
                    indexicals[v.getIndexedValue()] = saIdx++;
                }
            }
            foreach (Variable v in variables)
            {
                if (!indexicals.ContainsKey(v.getIndexedValue()))
                {
                    throw new Exception("ERROR: duplicate var=" + v + ", expr=" + expr);
                }
                else
                {
                    v.setIndexical(indexicals[v.getIndexedValue()]);
                }
            }

            return saIdx;
        }

        private static void collectAllVariables(Sentence s, IList<Variable> vars)
        {
            s.accept(_collectAllVariables, vars);
        }
    }

    class CollectAllVariables : FOLVisitor
    {
        public CollectAllVariables()
        {

        }

        public object visitVariable(Variable var, object arg)
        {
            IList<Variable> variables = (List<Variable>)arg;
            variables.Add(var);
            return var;
        }

        public object visitQuantifiedSentence(QuantifiedSentence sentence, object arg)
        {
            // Ensure I collect quantified variables too
            IList<Variable> variables = (IList<Variable>)arg;
            foreach (var v in sentence.getVariables())
                variables.Add(v);

            sentence.getQuantified().accept(this, arg);

            return sentence;
        }

        public object visitPredicate(Predicate predicate, object arg)
        {
            foreach (Term t in predicate.getTerms())
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
            foreach (Term t in function.getTerms())
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
