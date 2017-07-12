namespace tvn.cosine.ai.search.nondeterministic
{
    /// <summary>
    /// Represents an if-state-then-plan statement for use with AND-OR search; explanation given on page 135 of AIMA3e.
    /// </summary>
    public class IfStateThenPlan
    {
        private object state;
        private Plan plan;
 
        public IfStateThenPlan(object state, Plan plan)
        {
            this.state = state;
            this.plan = plan;
        }
        /// <summary>
        /// Uses this if-state-then-plan return a result based on the given state
        /// </summary>
        /// <param name="state"></param>
        /// <returns>a result based on the given state</returns>
        public Plan ifStateMatches(object state)
        {
            if (this.state.Equals(state))
            {
                return plan;
            }
            else
            {
                return null;
            }
        }
         
        /// <summary>
        /// Return string representation of this if-then-else 
        /// </summary>
        /// <returns>a string representation of this if-then-else.</returns>
        public override string ToString()
        {
            return "if " + state + " then " + plan;
        }
    }
}
