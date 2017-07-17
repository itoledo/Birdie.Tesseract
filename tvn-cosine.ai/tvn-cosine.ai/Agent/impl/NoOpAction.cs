namespace tvn.cosine.ai.agent.impl
{
    /**
     * @author Ciaran O'Reilly
     */
    public class NoOpAction : DynamicAction
    {
        public static readonly NoOpAction NO_OP = new NoOpAction();
         
        public override bool isNoOp()
        {
            return true;
        }
         
        private NoOpAction()
            : base("NoOp")
        { }
    } 
}
