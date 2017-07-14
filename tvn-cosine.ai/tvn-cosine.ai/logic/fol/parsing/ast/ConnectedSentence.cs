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
    public class ConnectedSentence : Sentence
    {
        private string connector;
        private Sentence first, second;
        private IList<Sentence> args = new List<Sentence>();
        private string stringRep = null;
        private int hashCode = 0;

        public ConnectedSentence(string connector, Sentence first, Sentence second)
        {
            this.connector = connector;
            this.first = first;
            this.second = second;
            args.Add(first);
            args.Add(second);
        }

        public string getConnector()
        {
            return connector;
        }

        public Sentence getFirst()
        {
            return first;
        }

        public Sentence getSecond()
        {
            return second;
        }

        //
        // START-Sentence
        public string getSymbolicName()
        {
            return getConnector();
        }

        public bool isCompound()
        {
            return true;
        }

        public IList<Sentence> getArgs()
        {
            return new ReadOnlyCollection<Sentence>(args);
        }

        public object accept(FOLVisitor v, object arg)
        {
            return v.visitConnectedSentence(this, arg);
        }

        public IList<T> getArgs<T>() where T : FOLNode
        {
            return new ReadOnlyCollection<T>(args.Select(x => (T)x).ToList());
        }

        FOLNode FOLNode.copy()
        {
            return copy();
        }

        Sentence Sentence.copy()
        {
            return copy();
        }

        public ConnectedSentence copy()
        {
            return new ConnectedSentence(connector, first.copy(), second.copy());
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
            ConnectedSentence cs = (ConnectedSentence)o;
            return cs.getConnector().Equals(getConnector())
                    && cs.getFirst().Equals(getFirst())
                    && cs.getSecond().Equals(getSecond());
        }

        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode = 37 * hashCode + getConnector().GetHashCode();
                hashCode = 37 * hashCode + getFirst().GetHashCode();
                hashCode = 37 * hashCode + getSecond().GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            if (null == stringRep)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                sb.Append(first.ToString());
                sb.Append(" ");
                sb.Append(connector);
                sb.Append(" ");
                sb.Append(second.ToString());
                sb.Append(")");
                stringRep = sb.ToString();
            }
            return stringRep;
        }

    }
}
