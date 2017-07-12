using System.Collections.Generic;
using System.Text;

namespace tvn.cosine.ai.search.nondeterministic
{
    /// <summary>
    /// Represents a solution plan for an AND-OR search; according to page 135
    /// AIMA3e, the plan must be "a subtree that (1) has a goal node at every leaf,
    /// (2) specifies one object at each of its OR nodes, and (3) includes every
    /// outcome branch at each of its AND nodes." As demonstrated on page 136, this
    /// subtree is implemented as a linked list where every OR node is an Object--
    /// satisfying (2)--and every AND node is an if-state-then-plan-else
    /// chain--satisfying (3).
    /// </summary> 
    public class Plan : List<object>
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public Plan()
        { }

        /// <summary>
        /// Construct a plan based on a sequence of steps (IfStateThenPlan or a Plan).
        /// </summary>
        /// <param name="steps"></param>
        public Plan(params object[] steps)
        {
            AddRange(steps);
        }

        /// <summary>
        /// Prepend an action to the plan and return itself.
        /// </summary>
        /// <param name="action">the action to be prepended to this plan.</param>
        /// <returns>this plan with action prepended to it.</returns>
        public Plan prepend(object action)
        {
            Insert(0, action);
            return this;
        }

        /// <summary>
        /// Returns the string representation of this plan
        /// </summary>
        /// <returns>a string representation of this plan.</returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append("[");
            int count = 0;
            int size = this.Count;
            foreach (object step in this)
            {
                s.Append(step);
                if (count < size - 1)
                {
                    s.Append(", ");
                }
                count++;
            }
            s.Append("]");
            return s.ToString();
        }
    }
}
