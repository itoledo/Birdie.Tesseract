namespace tvn.cosine.ai.agent.impl
{
    /**
     * @author Ciaran O'Reilly
     */
    public class NoOpAction : DynamicAction
    {
        public static readonly NoOpAction NO_OP = new NoOpAction();

        //
        // START-Action
        public override bool isNoOp()
        {
            return true;
        }

        // END-Action
        //

        private NoOpAction()
            : base("NoOp")
        { }
    }
}
