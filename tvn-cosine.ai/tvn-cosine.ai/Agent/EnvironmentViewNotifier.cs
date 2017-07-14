namespace tvn.cosine.ai.agent
{
    /// <summary>
    /// An EnvironmentView Notifier to notify Environment's registered EnvironmentViews
    /// </summary>
    public interface EnvironmentViewNotifier
    { 
        /// <summary>
        /// A simple notification message, to be forwarded to an Environment's registered EnvironmentViews.
        /// </summary>
        /// <param name="message">the message to be forwarded to the EnvironmentViews.</param>
        void notifyViews(string message);
    } 
}
