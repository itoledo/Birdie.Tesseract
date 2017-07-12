namespace tvn.cosine.ai.agent.impl.aprog.simplerule
{
    /// <summary>
    /// Base abstract class for describing conditions.
    /// </summary>
    public abstract class Condition
    {
        public abstract bool Evaluate(ObjectWithDynamicAttributes<string, object> p);

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
