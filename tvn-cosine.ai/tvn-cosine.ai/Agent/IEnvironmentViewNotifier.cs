namespace tvn.cosine.ai.agent
{
    public interface IEnvironmentViewNotifier
    {
        /// <summary>
        /// A simple notification message, to be forwarded to an Environment's registered EnvironmentViews.
        /// </summary>
        /// <param name="message">the message to be forwarded to the EnvironmentViews.</param>
        void NotifyViews(string message);
    }
}
