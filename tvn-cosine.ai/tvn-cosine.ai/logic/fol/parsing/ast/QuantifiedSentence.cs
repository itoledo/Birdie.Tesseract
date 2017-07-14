using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    /**
   * @author Ravi Mohan
   * @author Ciaran O'Reilly
   */
    public class QuantifiedSentence : Sentence
    {
        private string quantifier;
        private IList<Variable> variables = new List<Variable>();
        private Sentence quantified;
        private IList<FOLNode> args = new List<FOLNode>();
        private string stringRep = null;
        private int hashCode = 0;

        public QuantifiedSentence(string quantifier, IList<Variable> variables, Sentence quantified)
        {
            this.quantifier = quantifier;
            foreach (var v in variables)
                this.variables.Add(v);
            this.quantified = quantified;
            foreach (var v in variables)
                this.args.Add(v);
            this.args.Add(quantified);
        }

        public string getQuantifier()
        {
            return quantifier;
        }

        public IList<Variable> getVariables()
        {
            return new ReadOnlyCollection<Variable>(variables);
        }

        public Sentence getQuantified()
        {
            return quantified;
        }

        //
        // START-Sentence
        public string getSymbolicName()
        {
            return getQuantifier();
        }

        public bool isCompound()
        {
            return true;
        }

        public IList<FOLNode> getArgs()
        {
            return new ReadOnlyCollection<FOLNode>(args);
        }

        public object accept(FOLVisitor v, object arg)
        {
            return v.visitQuantifiedSentence(this, arg);
        }

        Sentence Sentence.copy()
        {
            return copy();
        }

        public IList<T> getArgs<T>() where T : FOLNode
        {
            return new ReadOnlyCollection<T>(args.Select(x => (T)x).ToList());
        }

        FOLNode FOLNode.copy()
        {
            return copy();
        }

        public QuantifiedSentence copy()
        {
            IList<Variable> copyVars = new List<Variable>();
            foreach (Variable v in variables)
            {
                copyVars.Add(v.copy());
            }
            return new QuantifiedSentence(quantifier, copyVars, quantified.copy());
        }

        // END-Sentence
        //

        public override bool Equals(object o)
        {

            if (this == o)
            {
                return true;
            }
            if ((o == null) || (this.GetType() != o.GetType()))
            {
                return false;
            }
            QuantifiedSentence cs = (QuantifiedSentence)o;
            return cs.quantifier.Equals(quantifier)
                    && cs.variables.Equals(variables)
                    && cs.quantified.Equals(quantified);
        }

        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode = 37 * hashCode + quantifier.GetHashCode();
                foreach (Variable v in variables)
                {
                    hashCode = 37 * hashCode + v.GetHashCode();
                }
                hashCode = hashCode * 37 + quantified.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            if (null == stringRep)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(quantifier);
                sb.Append(" ");
                foreach (Variable v in variables)
                {
                    sb.Append(v.ToString());
                    sb.Append(" ");
                }
                sb.Append(quantified.ToString());
                stringRep = sb.ToString();
            }
            return stringRep;
        }

    }
}
