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
    public class NotSentence : Sentence
    {

        private Sentence negated;
        private List<Sentence> args = new List<Sentence>();
        private string stringRep = null;
        private int hashCode = 0;

        public NotSentence(Sentence negated)
        {
            this.negated = negated;
            args.Add(negated);
        }

        public Sentence getNegated()
        {
            return negated;
        }

        //
        // START-Sentence
        public string getSymbolicName()
        {
            return Connectors.NOT;
        }

        public bool isCompound()
        {
            return true;
        }

        public IList<FOLNode> getArgs()
        {
            return new ReadOnlyCollection<FOLNode>(args.Select(x => x as FOLNode).ToList());
        }

        public object accept(FOLVisitor v, object arg)
        {
            return v.visitNotSentence(this, arg);
        }

        FOLNode FOLNode.copy()
        {
            return copy() as FOLNode;
        }

        public Sentence copy()
        {
            return new NotSentence(negated.copy());
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
            NotSentence ns = (NotSentence)o;
            return (ns.negated.Equals(negated));
        }

        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode = 37 * hashCode + negated.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            if (null == stringRep)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("NOT(");
                sb.Append(negated.ToString());
                sb.Append(")");
                stringRep = sb.ToString();
            }
            return stringRep;
        }
    }

}
