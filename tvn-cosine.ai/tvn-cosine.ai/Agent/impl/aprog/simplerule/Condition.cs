namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /**
     * Base abstract class for describing conditions.
     * 
     * @author Ciaran O'Reilly
     * 
     */
    public abstract class Condition
    {
        public abstract bool evaluate(ObjectWithDynamicAttributes<object, object> p);

        public override bool Equals(object o)
        {
            return o != null && GetType() == o.GetType() && ToString().Equals(o.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

}
