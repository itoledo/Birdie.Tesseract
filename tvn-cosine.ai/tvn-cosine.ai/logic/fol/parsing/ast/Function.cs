using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<Term> terms = new List<Term>();
        private string stringRep = null;
        private int hashCode = 0;

        public Function(string functionName, IList<Term> terms)
        {
            this.functionName = functionName;
            this.terms.AddRange(terms);
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


        IList<FOLNode> FOLNode.getArgs()
        {
            return getTerms() as IList<FOLNode>;
        }

        public object accept(FOLVisitor v, object arg)
        {
            return v.visitFunction(this, arg);
        }

        public Term copy()
        {
            List<Term> copyTerms = new List<Term>();
            foreach (Term t in terms)
            {
                copyTerms.Add(t.copy() as Term);
            }
            return new Function(functionName, copyTerms);
        }

        FOLNode FOLNode.copy()
        {
            return copy() as FOLNode;
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
