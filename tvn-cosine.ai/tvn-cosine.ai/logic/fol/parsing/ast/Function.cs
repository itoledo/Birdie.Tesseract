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
    public class Function : Term
    {

        private string functionName;
        private IList<Term> terms = new List<Term>();
        private string stringRep = null;
        private int hashCode = 0;

        public Function(string functionName, IList<Term> terms)
        {
            this.functionName = functionName;
            foreach (var v in terms)
                this.terms.Add(v);
        }

        public string getFunctionName()
        {
            return functionName;
        }

        public IList<Term> getTerms()
        {
            return new ReadOnlyCollection<Term>(terms);
        }

        //
        // START-Term
        public string getSymbolicName()
        {
            return getFunctionName();
        }

        public bool isCompound()
        {
            return true;
        }

        public IList<Term> getArgs()
        {
            return getTerms();
        }

        public object accept(FOLVisitor v, object arg)
        {
            return v.visitFunction(this, arg);
        }

        public IList<T> getArgs<T>() where T : FOLNode
        {
            return new ReadOnlyCollection<T>(terms.Select(x => (T)x).ToList());
        }

        FOLNode FOLNode.copy()
        {
            return copy();
        }

        Term Term.copy()
        {
            return copy();
        }

        public Function copy()
        {
            IList<Term> copyTerms = new List<Term>();
            foreach (Term t in terms)
            {
                copyTerms.Add(t.copy());
            }
            return new Function(functionName, copyTerms);
        }

        // END-Term
        //

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }
            if (!(o is Function))
            {
                return false;
            }

            Function f = (Function)o;

            return f.getFunctionName().Equals(getFunctionName())
                    && f.getTerms().Equals(getTerms());
        }

        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode = 37 * hashCode + functionName.GetHashCode();
                foreach (Term t in terms)
                {
                    hashCode = 37 * hashCode + t.GetHashCode();
                }
            }
            return hashCode;
        }

        public override string ToString()
        {
            if (null == stringRep)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(functionName);
                sb.Append("(");

                bool first = true;
                foreach (Term t in terms)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        sb.Append(",");
                    }
                    sb.Append(t.ToString());
                }

                sb.Append(")");

                stringRep = sb.ToString();
            }
            return stringRep;
        }

    }
}
