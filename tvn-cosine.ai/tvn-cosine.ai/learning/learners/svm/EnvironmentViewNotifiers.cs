using tvn.cosine.ai.agent.api;

namespace tvn.cosine.ai.learning.learners.svm
{ 
    public class EnvironmentViewNotifierConsole : IEnvironmentViewNotifier
    {
        public void NotifyViews(string message)
        {
            System.Console.WriteLine(message);
        }
    }
     
    public class EnvironmentViewNotifierNone : IEnvironmentViewNotifier
    {
        public void NotifyViews(string message)
        { } 
    }
}
