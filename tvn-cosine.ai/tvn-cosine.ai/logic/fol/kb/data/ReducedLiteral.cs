﻿using tvn.cosine;
using tvn.cosine.api;
using tvn.cosine.text;
using tvn.cosine.text.api;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.kb.data
{
    /**
     * @see <see href="http://logic.stanford.edu/classes/cs157/2008/lectures/lecture13.pdf">Reduced Literal</a>
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public class ReducedLiteral : Literal
    {

        private string strRep = null;

        public ReducedLiteral(AtomicSentence atom)
            : base(atom)
        { }

        public ReducedLiteral(AtomicSentence atom, bool negated)
            : base(atom, negated)
        { }


        public override Literal newInstance(AtomicSentence atom)
        {
            return new ReducedLiteral(atom, isNegativeLiteral());
        }
         
        public override string ToString()
        {
            if (null == strRep)
            {
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                sb.Append("[");
                if (isNegativeLiteral())
                {
                    sb.Append("~");
                }
                sb.Append(getAtomicSentence().ToString());
                sb.Append("]");
                strRep = sb.ToString();
            }

            return strRep;
        }
    }
}
