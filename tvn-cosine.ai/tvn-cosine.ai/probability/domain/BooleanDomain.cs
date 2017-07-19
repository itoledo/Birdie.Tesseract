﻿using tvn.cosine.ai.common.collections;

namespace tvn.cosine.ai.probability.domain
{
    /**
     * Artificial Intelligence A Modern Approach (3rd Edition): page 486.
     * 
     * A Boolean random variable has the domain {true,false}.
     * 
     * @author Ciaran O'Reilly.
     */
    public class BooleanDomain : AbstractFiniteDomain
    {
        private static ISet<bool> _possibleValues;
        static BooleanDomain()
        {
            // Keep consistent order
            _possibleValues = Factory.CreateSet<bool>();
            _possibleValues.Add(true);
            _possibleValues.Add(false);
            // Ensure cannot be modified
            _possibleValues = Factory.CreateReadOnlySet<bool>(_possibleValues);
        }

        public BooleanDomain()
        {
            indexPossibleValues(_possibleValues);
        }
         
        public override int size()
        {
            return 2;
        }


        public override bool isOrdered()
        {
            return false;
        }
         
        public ISet<bool> getPossibleValues()
        {
            return _possibleValues;
        }
         
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
