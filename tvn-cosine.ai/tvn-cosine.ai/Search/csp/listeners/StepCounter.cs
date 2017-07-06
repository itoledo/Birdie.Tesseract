using System.Collections.Generic;

namespace tvn.cosine.ai.search.csp.listeners
{
    public class StepCounter<VAR, VAL> : CspListener<VAR, VAL>
        where VAR : Variable
    {
        private int assignmentCount = 0;
        private int inferenceCount = 0;
         
        public void stateChanged(CSP<VAR, VAL> csp, Assignment<VAR, VAL> assignment, VAR variable)
        {
            if (assignment != null)
                ++assignmentCount;
            else
                ++inferenceCount;
        }

        public void reset()
        {
            assignmentCount = 0;
            inferenceCount = 0;
        }

        public IDictionary<string, double> getResults()
        {
            IDictionary<string, double> result = new Dictionary<string, double>();
            result.Add("assignmentCount", assignmentCount);
            if (inferenceCount != 0)
                result.Add("inferenceCount", inferenceCount);
            return result;
        }
    }
}
