using System.Collections.Generic;

namespace tvn.cosine.ai.logic.fol.parsing.ast
{
    /**
  * @author Ravi Mohan
  * @author Ciaran O'Reilly
  */
    public class Constant : Term
    {
        private string value;
        private int hashCode = 0;

        public Constant(string s)
        {
            value = s;
        }

        public string getValue()
        {
            return value;
        }

        //
        // START-Term
        public string getSymbolicName()
        {
            return getValue();
        }

        public bool isCompound()
        {
            return false;
        }

        public IList<Term> getArgs()
        {
            // Is not Compound, therefore should
            // return null for its arguments
            return null;
        }

        public object accept(FOLVisitor v, object arg)
        {
            return v.visitConstant(this, arg);
        }

        public IList<T> getArgs<T>() where T : FOLNode
        {
            // Is not Compound, therefore should
            // return null for its arguments
            return null;
        }

        FOLNode FOLNode.copy()
        {
            return copy();
        }

        public Constant copy()
        {
            return new Constant(value);
        }

        Term Term.copy()
        {
            return new Constant(value);
        }

        // END-Term
        //

        public override bool Equals(object o)
        {

            if (this == o)
            {
                return true;
            }
            if (!(o is Constant))
            {
                return false;
            }
            Constant c = (Constant)o;
            return c.getValue().Equals(getValue());

        }

        public override int GetHashCode()
        {
            if (0 == hashCode)
            {
                hashCode = 17;
                hashCode = 37 * hashCode + value.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            return value;
        }

    }
}
