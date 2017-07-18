using System.Text;
using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    public class NotSentence : Sentence
    {
        private Sentence negated;
        private IQueue<Sentence> args = Factory.CreateQueue<Sentence>();
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

        public IQueue<Sentence> getArgs()
        {
            return Factory.CreateReadOnlyQueue<Sentence>(args);
        }

        public object accept(FOLVisitor v, object arg)
        {
            return v.visitNotSentence(this, arg);
        }

        public NotSentence copy()
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
