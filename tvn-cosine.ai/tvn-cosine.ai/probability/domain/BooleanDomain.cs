using System.Collections.Generic;

namespace tvn.cosine.ai.probability.domain
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 486.
     * 
     * A bool random variable has the domain {true,false}.
     * 
     * @author Ciaran O'Reilly.
     */
    public class BooleanDomain : AbstractFiniteDomain<bool>
    {
        private static ISet<bool> _possibleValues;

        static BooleanDomain()
        {
            _possibleValues = new HashSet<bool>();
            _possibleValues.Add(true);
            _possibleValues.Add(false);
            //TODO: Ensure cannot be modified
            // readonlyCollection (_possibleValues);
        }

        public BooleanDomain()
        {
            indexPossibleValues(_possibleValues);
        }

        //
        // START-Domain 
        public override int size()
        {
            return 2;
        }

        public override bool isOrdered()
        {
            return false;
        }

        // END-Domain
        //

        //
        // START-DiscreteDomain 
        public override ISet<bool> getPossibleValues()
        {
            return _possibleValues;
        }

        // END-DiscreteDomain
        //

        public override bool Equals(object o)
        {
            return o is BooleanDomain;
        }

        public override int GetHashCode()
        {
            return _possibleValues.GetHashCode();
        }
    } 
}
