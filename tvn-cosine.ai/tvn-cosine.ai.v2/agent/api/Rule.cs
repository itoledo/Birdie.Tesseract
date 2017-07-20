using tvn_cosine.ai.v2.common;

namespace tvn_cosine.ai.v2.agent.api
{
    /// <summary>
    /// A simple condition-action rule definition.
    /// </summary>
    /// <typeparam name="A">the action type that the rule triggers.</typeparam>
    /// <typeparam name="S">the state type that the condition predicate tests.</typeparam>
    public interface Rule<A, S>
    {
        Predicate<S> condition();

        A action();
    }
}
