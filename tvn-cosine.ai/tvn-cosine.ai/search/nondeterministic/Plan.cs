﻿namespace tvn.cosine.ai.search.nondeterministic
{
    /**
     * Represents a solution plan for an AND-OR search; according to page 135
     * AIMA3e, the plan must be "a subtree that (1) has a goal node at every leaf,
     * (2) specifies one object at each of its OR nodes, and (3) includes every
     * outcome branch at each of its AND nodes." As demonstrated on page 136, this
     * subtree is implemented as a linked list where every OR node is an Object--
     * satisfying (2)--and every AND node is an if-state-then-plan-else
     * chain--satisfying (3).
     * 
     * @author Andrew Brown
     */
    public class Plan : LinkedList<object> {


    private static final long serialVersionUID = 1L;

    /**
	 * Empty constructor
	 */
    public Plan()
    {
    }

    /**
	 * Construct a plan based on a sequence of steps (IfStateThenPlan or a
	 * Plan).
	 */
    public Plan(params object[] steps)
    {
        for (object step : steps)
            add(step);
    }

    /**
	 * Prepend an action to the plan and return itself.
	 * 
	 * @param action
	 *            the action to be prepended to this plan.
	 * @return this plan with action prepended to it.
	 */
    public Plan prepend(object action)
    {
        offerFirst(action);
        return this;
    }

    /**
	 * Returns the string representation of this plan
	 * 
	 * @return a string representation of this plan.
	 */
     
    public override string ToString()
    {
        StringBuilder s = new StringBuilder();
        s.Append("[");
        int count = 0;
        int size = this.size();
        for (object step : this)
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
