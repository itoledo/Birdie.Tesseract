namespace tvn_cosine.ai.Agents
{
    public interface IEnvironmentViewNotifier
    {
        /// <summary>
        /// A simple notification message, to be forwarded to an Environment's
        /// registered Environment Views.
        /// </summary>
        /// <param name="message">the message to notify the registered EnvironmentViews with.</param>
        void NotifyViews(string message);
    }
}
