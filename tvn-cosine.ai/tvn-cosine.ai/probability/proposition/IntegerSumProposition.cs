using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tvn.cosine.ai.probability.domain;

namespace tvn.cosine.ai.probability.proposition
{
    public class IntegerSumProposition : AbstractDerivedProposition<int>
    {
        private FiniteIntegerDomain sumsDomain = null;
        private List<RandomVariable> sumVars = new List<RandomVariable>();
        //
        private string toString = null;

        public IntegerSumProposition(string name, FiniteIntegerDomain sumsDomain, IEnumerable<RandomVariable> sums)
            : base(name)
        {
            if (null == sumsDomain)
            {
                throw new ArgumentException("Sum Domain must be specified.");
            }
            if (null == sums || !sums.Any())
            {
                throw new ArgumentException("Sum variables must be specified.");
            }
            this.sumsDomain = sumsDomain;
            foreach (RandomVariable rv in sums)
            {
                addScope(rv);
                sumVars.Add(rv);
            }
        }

        //
        // START-Proposition
        public override bool holds(IDictionary<RandomVariable, int> possibleWorld)
        {
            int sum = 0;

            foreach (RandomVariable rv in sumVars)
            {
                int o = possibleWorld[rv];
                sum += o;
            }

            return sumsDomain.getPossibleValues().Contains(sum);
        }

        // END-Proposition
        //

        public override string ToString()
        {
            if (null == toString)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(getDerivedName());
                sb.Append(" = ");
                sb.Append(sumsDomain.ToString());
                toString = sb.ToString();
            }
            return toString;
        }
    } 
}
