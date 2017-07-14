namespace tvn.cosine.ai.agent
{
    /// <summary>
    /// Describes an Action that can or has been taken by an Agent via one of its Actuators.
    /// </summary>
    public interface Action
    { 
        /// <summary>
        /// Indicates whether or not this Action is a 'No Operation'. 
        /// Note: AIMA3e - NoOp, or no operation, is the name of an assembly language
        /// instruction that does nothing. 
        /// </summary>
        /// <returns>true if this is a NoOp Action.</returns>
        bool isNoOp();
    }
}
