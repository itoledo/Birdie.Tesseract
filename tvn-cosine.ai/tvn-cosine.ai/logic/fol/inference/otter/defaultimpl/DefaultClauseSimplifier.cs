using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tvn.cosine.ai.logic.fol.kb.data;
using tvn.cosine.ai.logic.fol.parsing.ast;

namespace tvn.cosine.ai.logic.fol.inference.otter.defaultimpl
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public class DefaultClauseSimplifier : ClauseSimplifier
    {
        private Demodulation demodulation = new Demodulation();
        private IList<TermEquality> rewrites = new List<TermEquality>();

        public DefaultClauseSimplifier()
        { }

        public DefaultClauseSimplifier(IList<TermEquality> rewrites)
        {
            foreach (var v in rewrites)
                this.rewrites.Add(v);
        }

        //
        // START-ClauseSimplifier
        public Clause simplify(Clause c)
        {
            Clause simplified = c;

            // Apply each of the rewrite rules to
            // the clause
            foreach (TermEquality te in rewrites)
            {
                Clause dc = simplified;
                // Keep applying the rewrite as many times as it
                // can be applied before moving on to the next one.
                while (null != (dc = demodulation.apply(te, dc)))
                {
                    simplified = dc;
                }
            }

            return simplified;
        }

        // END-ClauseSimplifier
        //
    }
}
