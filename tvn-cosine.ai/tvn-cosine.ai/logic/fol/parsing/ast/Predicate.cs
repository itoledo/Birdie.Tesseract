using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    /**
     * @author Ravi Mohan
     * @author Ciaran O'Reilly
     */
    public class Predicate : AtomicSentence
    {
        private string predicateName;
        private List<Term> terms = new List<Term>();
        private string stringRep = null;
        private int hashCode = 0;

        public Predicate(string predicateName, IList<Term> terms)
        {
            this.predicateName = predicateName;
            this.terms.AddRange(terms);
        }

        public string getPredicateName()
        {
            return predicateName;
        }

        public IList<Term> getTerms()
        {
            return new ReadOnlyCollection<Term>(terms);
        }

        //
        // START-AtomicSentence
        public string getSymbolicName()
        {
            return getPredicateName();
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
            return v.visitPredicate(this, arg);
        }


        Sentence Sentence.copy()
        {
            return copy() as Sentence;
        }
        FOLNode FOLNode.copy()
        {
            return copy() as FOLNode;
        }

        public Predicate copy()
        {
            List<Term> copyTerms = new List<Term>();
            foreach (Term t in terms)
            {
                copyTerms.Add(t.copy());
            }
            return new Predicate(predicateName, copyTerms);
        }

        // END-AtomicSentence
        //

        public override bool Equals(object o)
        {

            if (this == o)
            {
                return true;
            }
            if (!(o is Predicate))
            {
                return false;
            }
            Predicate p = (Predicate)o;
            return p.getPredicateName().Equals(getPredicateName())
                    && p.getTerms().Equals(getTerms());
        }

        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode = 37 * hashCode + predicateName.GetHashCode();
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
                sb.Append(predicateName);
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
