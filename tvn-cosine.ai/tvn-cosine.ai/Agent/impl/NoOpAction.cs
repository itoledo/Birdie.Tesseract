namespace tvn.cosine.ai.agent.impl
{
    public class NoOpAction : DynamicAction
    {
        public static readonly NoOpAction NO_OP = new NoOpAction();

        public override bool IsNoOp
        {
            get
            {
                return true;
            }
        }

        // END-Action
        //

        private NoOpAction()
            : base("NoOp")
        { }
    }
}
