using System;
using System.Collections.Generic;
using System.Text;

namespace tvn.cosine.ai.nlp.parsing.grammers
{
    /**
     * A derivation rule that is contained within a grammar. This rule is probabilistic, in that it 
     * has an associated probability representing the likelihood that the RHS follows from the LHS, given 
     * the presence of the LHS.
     * @author Jonathon (thundergolfer)
     *
     */
    public class Rule
    {
        public readonly float PROB;
        public readonly IList<string> lhs; // Left hand side of derivation rule
        public readonly IList<string> rhs; // Right hand side of derivation rule

        // Basic constructor
        public Rule(IList<string> lhs, IList<string> rhs, float probability)
        {
            this.lhs = lhs;
            this.rhs = rhs;
            this.PROB = validateProb(probability);
        }

        // null RHS rule constructor
        public Rule(List<String> lhs, float probability)
        {
            this.lhs = lhs;
            this.rhs = null;
            this.PROB = validateProb(probability);
        }

        // string split constructor
        public Rule(string lhs, string rhs, float probability)
        {
            if (lhs.Equals(""))
            {
                this.lhs = new List<string>();
            }
            else
            {
                this.lhs = new List<string>(lhs.Split(' ', '\n'));
            }
            if (rhs.Equals(""))
            {
                this.rhs = new List<string>();
            }
            else
            {
                this.rhs = new List<string>(rhs.Split(' ', '\n'));
            }
            this.PROB = validateProb(probability);

        }

        /**
         * Currently a hack to ensure rule has a valid probablity value.
         * Don't really want to throw an exception.
         */
        private float validateProb(float prob)
        {
            if (prob >= 0.0 && prob <= 1.0)
            {
                return prob;
            }
            else
            {
                return (float)0.5; // probably should throw exception
            }
        }

        public bool derives(IList<string> sentForm)
        {
            if (this.rhs.Count != sentForm.Count)
            {
                return false;
            }
            for (int i = 0; i < sentForm.Count; ++i)
            {
                if (!this.rhs[i].Equals(sentForm[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public bool derives(string terminal)
        {
            if (this.rhs.Count == 1 && this.rhs[0].Equals(terminal))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            foreach (string lh in lhs)
            {
                output.Append(lh);
            }

            output.Append(" -> ");

            foreach (string rh in rhs)
            {
                output.Append(rh);
            }

            output.Append(" ").Append(PROB.ToString());

            return output.ToString();
        }
    }

}
