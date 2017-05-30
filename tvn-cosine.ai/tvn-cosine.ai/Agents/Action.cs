namespace tvn_cosine.ai.Agents
{
    /// <summary>
    /// Describes an Action that can or has been taken by an Agent via one of its Actuators.
    /// </summary>
    public class Action : IAction
    {
        public static IAction NO_OP = new Action(true, "No Operation");

        public Action(bool isNoOp, string name)
        {
            this.IsNoOp = IsNoOp;
            this.Name = name;
        }

        public Action(string name)
            : this(false, name)
        { }

        /// <summary>
        /// Indicates whether or not this Action is a 'No Operation'.
        /// 
        /// Note: AIMA3e - NoOp, or no operation, is the name of an assembly language
        /// instruction that does nothing.
        /// </summary>
        public bool IsNoOp { get; }

        /// <summary>
        /// The name of the Action.
        /// </summary>
        public string Name { get; }
    }
}
