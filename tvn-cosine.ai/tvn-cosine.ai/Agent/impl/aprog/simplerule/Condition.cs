using tvn.cosine.ai.common;

namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /**
     * Base abstract class for describing conditions.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public abstract class Condition : IEquatable, IHashable, IToString
    {
        public abstract bool evaluate(ObjectWithDynamicAttributes p);
        public new abstract string ToString();

        public override bool Equals(object o)
        {
            return o != null 
                && GetType() == o.GetType() 
                && ToString().Equals(o.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
