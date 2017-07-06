namespace tvn.cosine.ai.agent
{
    /**
     * @author Ciaran O'Reilly
     * 
     */
    public interface EnvironmentViewNotifier
    {
        /**
         * A simple notification message, to be forwarded to an Environment's
         * registered EnvironmentViews.
         * 
         * @param msg
         *            the message to be forwarded to the EnvironmentViews.
         */
        void notifyViews(string msg);
    }

}
